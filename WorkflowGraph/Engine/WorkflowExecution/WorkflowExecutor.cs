using Engine.Graph.Extensions;
using Engine.Persistance;
using Engine.Persistance.Models;
using Engine.Workflow;
using System.Text.Json;

namespace Engine.WorkflowExecution
{
    public sealed class WorkflowExecutor<TKey>
        where TKey : notnull
    {
        private readonly object _sync = new();
        private readonly SemaphoreSlim _resumeSignal = new(0, int.MaxValue);
        private readonly IInstancePersistence<TKey>? _persistence;
        private readonly IKeyCodec<TKey>? _keyCodec;

        private Workflow<TKey>? _activeWorkflow;
        private Dictionary<TKey, NodeState>? _activeStates;
        private Dictionary<TKey, int>? _activeRemainingDeps;
        private Queue<TKey>? _activeReady;
        private WorkflowContext? _activeContext;
        private Dictionary<TKey, string?>? _activeWaitingSignals;
        private Dictionary<string, Queue<JsonElement?>>? _activeSignals;
        private Guid? _activeInstanceId;
        private WorkflowDefinitionRef? _activeDefinitionRef;
        private long _eventSequence;
        private DateTimeOffset _createdUtc;

        public WorkflowExecutor(IInstancePersistence<TKey>? persistence = null, IKeyCodec<TKey>? keyCodec = null)
        {
            _persistence = persistence;
            _keyCodec = keyCodec;
        }

        /// <summary>
        /// Raised whenever a node transitions to a new execution state.
        /// </summary>
        public event EventHandler<WorkflowNodeStateChangedEventArgs<TKey>>? NodeStateChanged;

        public Guid? ActiveInstanceId => _activeInstanceId;

        /// <summary>
        /// Executes a workflow respecting dependency order and configured parallelism.
        /// </summary>
        public async Task<WorkflowRunReport<TKey>> RunAsync(
                                    Workflow<TKey> workflow,
                                        WorkflowExecutorOptions? options = null,
                                            CancellationToken cancellationToken = default,
                                            Guid? instanceId = null,
                                            WorkflowDefinitionRef? definitionRef = null)
        {
            ArgumentNullException.ThrowIfNull(workflow);

            options ??= new WorkflowExecutorOptions();
            if (options.MaxDegreeOfParallelism <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.MaxDegreeOfParallelism));
            }

            if (workflow.Graph.TryGetCycle(out var cycle))
            {
                var cycleStates = workflow.Graph.Vertices.ToDictionary(v => v, _ => NodeState.Pending);
                return new WorkflowRunReport<TKey>
                {
                    NodeStates = cycleStates,
                    Cycle = cycle,
                    WaitingNodes = []
                };
            }

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var ct = linkedCts.Token;

            var runInstanceId = instanceId ?? Guid.NewGuid();
            var states = workflow.Graph.Vertices.ToDictionary(v => v, _ => NodeState.Pending);
            var context = new WorkflowContext();
            var waitingSignals = workflow.Graph.Vertices.ToDictionary(v => v, _ => (string?)null);
            _eventSequence = 0;
            _createdUtc = DateTimeOffset.UtcNow;

            if (_persistence is not null && _keyCodec is not null)
            {
                var persisted = await _persistence.LoadAsync(runInstanceId, ct).ConfigureAwait(false);
                _eventSequence = persisted.LastAppliedEventSequence;
                _createdUtc = persisted.CreatedUtc;
                definitionRef ??= persisted.DefinitionRef.Name == "unknown" ? definitionRef : persisted.DefinitionRef;
                foreach (var node in persisted.Nodes.Values)
                {
                    states[node.NodeId] = node.State == NodeState.Running && node.Lease?.ExpiresUtc < DateTimeOffset.UtcNow
                        ? NodeState.Pending
                        : node.State;
                    waitingSignals[node.NodeId] = node.WaitingForSignal;
                }

                context.Restore(persisted.Context);
            }

            var remainingDeps = ComputeRemainingDependencies(workflow, states);
            var ready = new Queue<TKey>(remainingDeps.Where(x => x.Value == 0 && states[x.Key] == NodeState.Pending).Select(x => x.Key));
            var running = new List<Task<(TKey NodeId, WorkflowNodeResult? Result, Exception? Error, bool Canceled)>>();

            lock (_sync)
            {
                _activeWorkflow = workflow;
                _activeStates = states;
                _activeRemainingDeps = remainingDeps;
                _activeReady = ready;
                _activeContext = context;
                _activeWaitingSignals = waitingSignals;
                _activeSignals = new Dictionary<string, Queue<JsonElement?>>(StringComparer.Ordinal);
                _activeInstanceId = runInstanceId;
                _activeDefinitionRef = definitionRef ?? new WorkflowDefinitionRef(typeof(TKey).Name);
            }

            try
            {
                PersistSnapshot(ct);

                while (!ct.IsCancellationRequested)
                {
                    while (TryScheduleReadyNode(workflow, options, ready, remainingDeps, states, running, context, ct))
                    {
                    }

                    if (running.Count > 0)
                    {
                        var completed = await Task.WhenAny(running).ConfigureAwait(false);
                        running.Remove(completed);
                        var result = await completed.ConfigureAwait(false);

                        HandleExecutionResult(workflow, options, linkedCts, remainingDeps, ready, states, result, ct);
                        continue;
                    }

                    if (ready.Count > 0)
                    {
                        continue;
                    }

                    if (states.Values.Any(x => x == NodeState.WaitingForInput))
                    {
                        DrainSignals(states, ct);
                        if (ready.Count > 0)
                        {
                            continue;
                        }

                        await _resumeSignal.WaitAsync(ct).ConfigureAwait(false);
                        continue;
                    }

                    break;
                }

                if (ct.IsCancellationRequested)
                {
                    foreach (var pending in states.Where(x => x.Value == NodeState.Pending).Select(x => x.Key).ToArray())
                    {
                        SetState(states, pending, NodeState.Canceled, null, ct);
                    }
                }

                return new WorkflowRunReport<TKey>
                {
                    NodeStates = states,
                    Cycle = null,
                    WaitingNodes = states.Where(x => x.Value == NodeState.WaitingForInput).Select(x => x.Key).ToArray()
                };
            }
            finally
            {
                lock (_sync)
                {
                    _activeWorkflow = null;
                    _activeStates = null;
                    _activeRemainingDeps = null;
                    _activeReady = null;
                    _activeContext = null;
                    _activeWaitingSignals = null;
                    _activeSignals = null;
                    _activeInstanceId = null;
                    _activeDefinitionRef = null;
                }
            }
        }

        public async Task<bool> PostSignalAsync(Guid instanceId, string key, JsonElement? payload = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Signal key is required.", nameof(key));
            }

            Dictionary<string, Queue<JsonElement?>>? signals = null;
            lock (_sync)
            {
                if (_activeInstanceId != instanceId || _activeSignals is null)
                {
                    return false;
                }

                signals = _activeSignals;
                if (!signals.TryGetValue(key, out var queue))
                {
                    queue = new Queue<JsonElement?>();
                    signals[key] = queue;
                }

                queue.Enqueue(payload);
                _resumeSignal.Release();
            }

            await AppendEventAsync(new SignalPostedEvent(NextSequence(), DateTimeOffset.UtcNow, key, payload), ct).ConfigureAwait(false);
            PersistSnapshot(ct);
            return true;
        }

        /// <summary>
        /// Attempts to resume a node that is currently waiting for external input.
        /// </summary>
        public bool TryResume(TKey nodeKey)
        {
            return TryResume(nodeKey, CancellationToken.None);
        }

        private bool TryResume(TKey nodeKey, CancellationToken ct)
        {
            Dictionary<TKey, NodeState>? statesToUpdate = null;
            lock (_sync)
            {
                if (_activeWorkflow is null
                    || _activeStates is null
                    || _activeReady is null
                    || _activeRemainingDeps is null
                    || !_activeStates.TryGetValue(nodeKey, out var state)
                    || !_activeWorkflow.Graph.ContainsVertex(nodeKey)
                    || state != NodeState.WaitingForInput)
                {
                    return false;
                }

                if (_activeRemainingDeps[nodeKey] != 0)
                {
                    return false;
                }

                _activeStates[nodeKey] = NodeState.Pending;
                _activeWaitingSignals![nodeKey] = null;
                statesToUpdate = _activeStates;
                _activeReady.Enqueue(nodeKey);
                _resumeSignal.Release();
            }

            SetState(statesToUpdate!, nodeKey, NodeState.Pending, null, ct);
            return true;
        }

        public void Resume(TKey nodeKey)
        {
            if (!TryResume(nodeKey))
            {
                throw new InvalidOperationException($"Node '{nodeKey}' cannot be resumed.");
            }
        }

        public int ResumeAllWaiting()
        {
            lock (_sync)
            {
                if (_activeStates is null)
                {
                    return 0;
                }

                var resumed = 0;
                foreach (var node in _activeStates.Where(x => x.Value == NodeState.WaitingForInput).Select(x => x.Key).ToArray())
                {
                    if (TryResume(node))
                    {
                        resumed++;
                    }
                }

                return resumed;
            }
        }

        public WorkflowContext? ActiveContext
        {
            get
            {
                lock (_sync)
                {
                    return _activeContext;
                }
            }
        }

        private static Dictionary<TKey, int> ComputeRemainingDependencies(Workflow<TKey> workflow, Dictionary<TKey, NodeState> states)
        {
            var result = workflow.Graph.Vertices.ToDictionary(v => v, _ => 0);
            foreach (var vertex in workflow.Graph.Vertices)
            {
                result[vertex] = workflow.Graph.GetIncoming(vertex)
                    .Count(dep => states[dep] is NodeState.Pending or NodeState.Running or NodeState.WaitingForInput);
            }

            return result;
        }

        private void DrainSignals(Dictionary<TKey, NodeState> states, CancellationToken ct)
        {
            if (_activeSignals is null || _activeWaitingSignals is null)
            {
                return;
            }

            foreach (var waiting in states.Where(x => x.Value == NodeState.WaitingForInput).Select(x => x.Key).ToArray())
            {
                var signalKey = _activeWaitingSignals[waiting];
                if (signalKey is null || !_activeSignals.TryGetValue(signalKey, out var payloads) || payloads.Count == 0)
                {
                    continue;
                }

                payloads.Dequeue();
                _ = AppendEventAsync(new SignalConsumedEvent(NextSequence(), DateTimeOffset.UtcNow, signalKey, _keyCodec?.Encode(waiting) ?? waiting!.ToString()!), ct);
                TryResume(waiting, ct);
            }
        }

        private bool TryScheduleReadyNode(
            Workflow<TKey> workflow,
            WorkflowExecutorOptions options,
            Queue<TKey> ready,
            Dictionary<TKey, int> remainingDeps,
            Dictionary<TKey, NodeState> states,
            List<Task<(TKey NodeId, WorkflowNodeResult? Result, Exception? Error, bool Canceled)>> running,
            WorkflowContext context,
            CancellationToken ct)
        {
            if (ready.Count == 0 || running.Count >= options.MaxDegreeOfParallelism || ct.IsCancellationRequested)
            {
                return false;
            }

            var nodeId = ready.Dequeue();
            if (states[nodeId] != NodeState.Pending)
            {
                return true;
            }

            if (options.SkipDependentsOnFailure)
            {
                var hasFailedDep = workflow.Graph.GetIncoming(nodeId).Any(dep =>
                    states[dep] is NodeState.Failed or NodeState.Skipped or NodeState.Canceled);
                if (hasFailedDep)
                {
                    SetState(states, nodeId, NodeState.Skipped, "Dependency failed.", ct);
                    foreach (var child in workflow.Graph.GetOutgoing(nodeId))
                    {
                        remainingDeps[child]--;
                        if (remainingDeps[child] == 0)
                        {
                            ready.Enqueue(child);
                        }
                    }

                    return true;
                }
            }

            SetState(states, nodeId, NodeState.Running, null, ct);
            var node = workflow.GetNode(nodeId);
            running.Add(ExecuteNode(nodeId, node, context, ct));
            return true;
        }

        private void HandleExecutionResult(
            Workflow<TKey> workflow,
            WorkflowExecutorOptions options,
            CancellationTokenSource linkedCts,
            Dictionary<TKey, int> remainingDeps,
            Queue<TKey> ready,
            Dictionary<TKey, NodeState> states,
            (TKey NodeId, WorkflowNodeResult? Result, Exception? Error, bool Canceled) result,
            CancellationToken ct)
        {
            if (result.Canceled)
            {
                SetState(states, result.NodeId, NodeState.Canceled, null, ct);
            }
            else if (result.Error is not null || result.Result == WorkflowNodeResult.Failure)
            {
                SetState(states, result.NodeId, NodeState.Failed, result.Error?.Message, ct);
                if (options.FailFast)
                {
                    linkedCts.Cancel();
                }
            }
            else if (result.Result == WorkflowNodeResult.WaitingForInput)
            {
                SetState(states, result.NodeId, NodeState.WaitingForInput, null, ct);
                return;
            }
            else
            {
                SetState(states, result.NodeId, NodeState.Succeeded, null, ct);
            }

            foreach (var child in workflow.Graph.GetOutgoing(result.NodeId))
            {
                remainingDeps[child]--;
                if (remainingDeps[child] == 0)
                {
                    ready.Enqueue(child);
                }
            }
        }

        private static async Task<(TKey NodeId, WorkflowNodeResult? Result, Exception? Error, bool Canceled)> ExecuteNode(
            TKey nodeId,
            WorkflowNode<TKey> node,
            WorkflowContext context,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await node.RunAsync(context, cancellationToken).ConfigureAwait(false);
                return (nodeId, result, null, false);
            }
            catch (OperationCanceledException)
            {
                return (nodeId, null, null, true);
            }
            catch (Exception ex)
            {
                return (nodeId, null, ex, false);
            }
        }

        private void SetState(Dictionary<TKey, NodeState> states, TKey nodeId, NodeState state, string? message, CancellationToken ct)
        {
            states[nodeId] = state;
            if (_activeWaitingSignals is not null)
            {
                _activeWaitingSignals[nodeId] = state == NodeState.WaitingForInput
                    ? (_keyCodec?.Encode(nodeId) ?? nodeId?.ToString())
                    : null;
            }

            NodeStateChanged?.Invoke(this, new WorkflowNodeStateChangedEventArgs<TKey>(nodeId, state, message));

            if (_persistence is null || _keyCodec is null)
            {
                return;
            }

            var evt = new NodeStateChangedEvent<TKey>(
                NextSequence(),
                DateTimeOffset.UtcNow,
                _keyCodec.Encode(nodeId),
                state,
                message,
                _activeWaitingSignals?[nodeId]);
            AppendEventAsync(evt, ct).GetAwaiter().GetResult();
            PersistSnapshot(ct);
        }

        private long NextSequence() => Interlocked.Increment(ref _eventSequence);

        private Task AppendEventAsync(WorkflowEvent evt, CancellationToken ct)
        {
            if (_persistence is null || _activeInstanceId is null)
            {
                return Task.CompletedTask;
            }

            return _persistence.AppendEventAsync(_activeInstanceId.Value, evt, ct);
        }

        private void PersistSnapshot(CancellationToken ct)
        {
            if (_persistence is null || _activeInstanceId is null || _activeStates is null || _activeContext is null || _activeDefinitionRef is null || _keyCodec is null)
            {
                return;
            }

            var nodes = _activeStates.ToDictionary(
                x => _keyCodec.Encode(x.Key),
                x => new NodeExecutionState<TKey>(x.Key, x.Value, WaitingForSignal: _activeWaitingSignals?[x.Key]));

            var status = WorkflowInstanceStatus.Running;
            if (_activeStates.Values.Any(x => x == NodeState.WaitingForInput))
            {
                status = WorkflowInstanceStatus.Suspended;
            }
            else if (_activeStates.Values.Any(x => x == NodeState.Failed))
            {
                status = WorkflowInstanceStatus.Failed;
            }
            else if (_activeStates.Values.All(x => x is NodeState.Succeeded or NodeState.Skipped or NodeState.Canceled))
            {
                status = WorkflowInstanceStatus.Completed;
            }

            var snapshot = new WorkflowInstanceState<TKey>(
                _activeInstanceId.Value,
                _activeDefinitionRef,
                status,
                _createdUtc,
                DateTimeOffset.UtcNow,
                _eventSequence,
                nodes,
                _activeContext.ToJsonMap());

            _persistence.SaveSnapshotAsync(snapshot, ct).GetAwaiter().GetResult();
        }
    }
}
