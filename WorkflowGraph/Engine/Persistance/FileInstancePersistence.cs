using Engine.Persistance.Models;
using Engine.Workflow;
using System.Text;
using System.Text.Json;

namespace Engine.Persistance;

public sealed class FileInstancePersistence<TKey> : IInstancePersistence<TKey>
    where TKey : notnull
{
    private const string SnapshotName = "instance.snapshot.json";
    private const string EventsName = "instance.events.jsonl";
    private readonly string _baseDirectory;
    private readonly IKeyCodec<TKey> _keyCodec;
    private readonly JsonSerializerOptions _jsonOptions;

    public FileInstancePersistence(string rootDirectory, IKeyCodec<TKey> keyCodec, JsonSerializerOptions? jsonOptions = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        _keyCodec = keyCodec ?? throw new ArgumentNullException(nameof(keyCodec));
        _baseDirectory = Path.Combine(rootDirectory, "workflows", "instances");
        _jsonOptions = jsonOptions ?? new JsonSerializerOptions(JsonSerializerDefaults.Web) { PropertyNameCaseInsensitive = true };
    }

    public async Task<WorkflowInstanceState<TKey>> LoadAsync(Guid instanceId, CancellationToken ct)
    {
        var instanceDir = GetInstanceDirectory(instanceId);
        Directory.CreateDirectory(instanceDir);

        await using var instanceLock = await AcquireLockAsync(instanceDir, ct).ConfigureAwait(false);

        WorkflowInstanceState<TKey>? snapshot = null;
        var snapshotPath = Path.Combine(instanceDir, SnapshotName);
        if (File.Exists(snapshotPath))
        {
            await using var snapshotStream = File.OpenRead(snapshotPath);
            snapshot = await JsonSerializer.DeserializeAsync<WorkflowInstanceState<TKey>>(snapshotStream, _jsonOptions, ct).ConfigureAwait(false);
        }

        if (snapshot is null)
        {
            return new WorkflowInstanceState<TKey>(
                instanceId,
                new WorkflowDefinitionRef("unknown"),
                WorkflowInstanceStatus.Running,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                0,
                new Dictionary<string, NodeExecutionState<TKey>>(),
                new Dictionary<string, JsonElement>());
        }

        var nodes = snapshot.Nodes.ToDictionary(x => x.Key, x => x.Value);
        var context = snapshot.Context.ToDictionary(x => x.Key, x => x.Value);
        var lastSequence = snapshot.LastAppliedEventSequence;

        var eventsPath = Path.Combine(instanceDir, EventsName);
        if (File.Exists(eventsPath))
        {
            foreach (var evt in await ReadEventsAsync(eventsPath, ct).ConfigureAwait(false))
            {
                if (evt.Sequence <= lastSequence)
                {
                    continue;
                }

                ApplyEvent(nodes, context, evt);
                lastSequence = evt.Sequence;
            }
        }

        var status = RecomputeStatus(nodes.Values);
        return snapshot with
        {
            Nodes = nodes,
            Context = context,
            LastAppliedEventSequence = lastSequence,
            Status = status,
            UpdatedUtc = DateTimeOffset.UtcNow
        };
    }

    public async Task AppendEventAsync(Guid instanceId, WorkflowEvent evt, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(evt);

        var instanceDir = GetInstanceDirectory(instanceId);
        Directory.CreateDirectory(instanceDir);

        await using var instanceLock = await AcquireLockAsync(instanceDir, ct).ConfigureAwait(false);

        var eventsPath = Path.Combine(instanceDir, EventsName);
        await using var stream = new FileStream(eventsPath, FileMode.Append, FileAccess.Write, FileShare.Read, 4096, useAsync: true);
        await JsonSerializer.SerializeAsync(stream, evt, evt.GetType(), _jsonOptions, ct).ConfigureAwait(false);
        await stream.WriteAsync("\n"u8.ToArray(), ct).ConfigureAwait(false);
        await stream.FlushAsync(ct).ConfigureAwait(false);
        stream.Flush(true);
    }

    public async Task SaveSnapshotAsync(WorkflowInstanceState<TKey> snapshot, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        var instanceDir = GetInstanceDirectory(snapshot.InstanceId);
        Directory.CreateDirectory(instanceDir);

        await using var instanceLock = await AcquireLockAsync(instanceDir, ct).ConfigureAwait(false);

        var snapshotPath = Path.Combine(instanceDir, SnapshotName);
        var tmpPath = snapshotPath + ".tmp";

        await using (var stream = new FileStream(tmpPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
        {
            await JsonSerializer.SerializeAsync(stream, snapshot, _jsonOptions, ct).ConfigureAwait(false);
            await stream.FlushAsync(ct).ConfigureAwait(false);
            stream.Flush(true);
        }

        File.Move(tmpPath, snapshotPath, overwrite: true);
    }

    private static WorkflowInstanceStatus RecomputeStatus(IEnumerable<NodeExecutionState<TKey>> nodes)
    {
        var all = nodes.ToArray();
        if (all.Any(x => x.State == NodeState.WaitingForInput))
        {
            return WorkflowInstanceStatus.Suspended;
        }

        if (all.Any(x => x.State == NodeState.Failed))
        {
            return WorkflowInstanceStatus.Failed;
        }

        if (all.All(x => x.State is NodeState.Succeeded or NodeState.Skipped or NodeState.Canceled))
        {
            return WorkflowInstanceStatus.Completed;
        }

        return WorkflowInstanceStatus.Running;
    }

    private void ApplyEvent(Dictionary<string, NodeExecutionState<TKey>> nodes, Dictionary<string, JsonElement> context, WorkflowEvent evt)
    {
        switch (evt)
        {
            case NodeStateChangedEvent<TKey> nodeChanged:
                var decoded = _keyCodec.Decode(nodeChanged.EncodedNodeId);
                var prior = nodes.TryGetValue(nodeChanged.EncodedNodeId, out var state) ? state : null;
                LeaseInfo? lease = prior?.Lease;
                if (nodeChanged.State == NodeState.Running)
                {
                    lease = new LeaseInfo(Environment.MachineName, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMinutes(1));
                }

                if (nodeChanged.State != NodeState.Running && lease is not null && lease.ExpiresUtc < DateTimeOffset.UtcNow)
                {
                    // Policy: expired running leases are retried by setting back to pending on recovery.
                    nodes[nodeChanged.EncodedNodeId] = new NodeExecutionState<TKey>(decoded, NodeState.Pending, "Recovered from expired lease.");
                    break;
                }

                nodes[nodeChanged.EncodedNodeId] = new NodeExecutionState<TKey>(decoded, nodeChanged.State, nodeChanged.Message, nodeChanged.WaitingForSignal, lease);
                break;
            case ContextSetEvent contextSet when contextSet.Value is JsonElement value:
                context[contextSet.ContextKey] = value;
                break;
            default:
                break;
        }
    }

    private async Task<IReadOnlyList<WorkflowEvent>> ReadEventsAsync(string eventsPath, CancellationToken ct)
    {
        var events = new List<WorkflowEvent>();
        using var stream = new FileStream(eventsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(stream, Encoding.UTF8);

        while (!reader.EndOfStream)
        {
            ct.ThrowIfCancellationRequested();
            var line = await reader.ReadLineAsync(ct).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            using var doc = JsonDocument.Parse(line);
            if (!doc.RootElement.TryGetProperty("sequence", out _) || !doc.RootElement.TryGetProperty("type", out var typeProperty))
            {
                continue;
            }

            var type = typeProperty.GetString();
            WorkflowEvent? evt = type switch
            {
                "node-state-changed" => JsonSerializer.Deserialize<NodeStateChangedEvent<TKey>>(line, _jsonOptions),
                "signal-posted" => JsonSerializer.Deserialize<SignalPostedEvent>(line, _jsonOptions),
                "signal-consumed" => JsonSerializer.Deserialize<SignalConsumedEvent>(line, _jsonOptions),
                "context-set" => JsonSerializer.Deserialize<ContextSetEvent>(line, _jsonOptions),
                "instance-status-changed" => JsonSerializer.Deserialize<InstanceStatusChangedEvent>(line, _jsonOptions),
                _ => null
            };

            if (evt is not null)
            {
                events.Add(evt);
            }
        }

        return events;
    }

    private string GetInstanceDirectory(Guid instanceId) => Path.Combine(_baseDirectory, instanceId.ToString("N"));

    private static async Task<IAsyncDisposable> AcquireLockAsync(string instanceDir, CancellationToken ct)
    {
        var lockPath = Path.Combine(instanceDir, ".instance.lock");

        while (true)
        {
            ct.ThrowIfCancellationRequested();
            try
            {
                var stream = new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1, FileOptions.DeleteOnClose);
                return stream;
            }
            catch (IOException)
            {
                await Task.Delay(50, ct).ConfigureAwait(false);
            }
        }
    }
}
