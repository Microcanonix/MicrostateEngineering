using Engine.Persistance;
using Engine.Persistance.Models;
using Engine.Workflow;
using Engine.WorkflowExecution;
using System.Text.Json;

namespace TaskGraphLib.Tests;

public class WorkflowPersistenceTests
{
    [Fact]
    public void SnapshotAndEvent_RoundTripSerialization()
    {
        var snapshot = new WorkflowInstanceState<string>(
                    Guid.NewGuid(),
                    new WorkflowDefinitionRef("demo", "v1"),
                    WorkflowInstanceStatus.Running,
                    DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow,
                    2,
                    new Dictionary<string, NodeExecutionState<string>>
                    {
                        ["A"] = new("A", NodeState.WaitingForInput, WaitingForSignal: "A")
                    },
                    new Dictionary<string, JsonElement>
                    {
                        ["answer"] = JsonSerializer.SerializeToElement(42)
                    });

        var snapshotJson = JsonSerializer.Serialize(snapshot);
        var snapshotDeserialized = JsonSerializer.Deserialize<WorkflowInstanceState<string>>(snapshotJson);

        Assert.NotNull(snapshotDeserialized);
        Assert.Equal(snapshot.InstanceId, snapshotDeserialized.InstanceId);
        Assert.Equal(NodeState.WaitingForInput, snapshotDeserialized.Nodes["A"].State);

        WorkflowEvent evt = new SignalPostedEvent(3, DateTimeOffset.UtcNow, "approval", JsonSerializer.SerializeToElement("yes"));
        var evtJson = JsonSerializer.Serialize(evt, evt.GetType());
        var evtDeserialized = JsonSerializer.Deserialize<SignalPostedEvent>(evtJson);

        Assert.NotNull(evtDeserialized);
        Assert.Equal("signal-posted", evtDeserialized.Type);
        Assert.Equal("approval", evtDeserialized.SignalKey);
    }

    [Fact]
    public async Task Persistence_CanResumeAfterRestart_WithSignalFlow()
    {
        var root = Path.Combine(Path.GetTempPath(), "workflow-persistence-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);

        try
        {
            var persistence = new FileInstancePersistence<string>(root, StringKeyCodec.Instance);
            var instanceId = Guid.NewGuid();
            var aRuns = 0;
            var approved = false;
            var waiting = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            Workflow<string> BuildWorkflow() => new Workflow<string>()
                .AddNode("A", (_, _) =>
                {
                    Interlocked.Increment(ref aRuns);
                    return Task.FromResult(WorkflowNodeResult.Success);
                })
                .AddNode("B", (_, _) => Task.FromResult(approved ? WorkflowNodeResult.Success : WorkflowNodeResult.WaitingForInput))
                .AddDependency("A", "B");

            var firstExecutor = new WorkflowExecutor<string>(persistence, StringKeyCodec.Instance);
            firstExecutor.NodeStateChanged += (_, args) =>
            {
                if (args.NodeId == "B" && args.State == NodeState.WaitingForInput)
                {
                    waiting.TrySetResult();
                }
            };

            using var firstRunCts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            var firstRun = firstExecutor.RunAsync(
                BuildWorkflow(),
                new WorkflowExecutorOptions { MaxDegreeOfParallelism = 1 },
                firstRunCts.Token,
                instanceId,
                new WorkflowDefinitionRef("test", "v1"));

            await waiting.Task;
            firstRunCts.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await firstRun);

            var secondExecutor = new WorkflowExecutor<string>(persistence, StringKeyCodec.Instance);
            var resumedRun = secondExecutor.RunAsync(
                                BuildWorkflow(),
                                new WorkflowExecutorOptions { MaxDegreeOfParallelism = 1 },
                                CancellationToken.None,
                                instanceId,
                                new WorkflowDefinitionRef("test", "v1"));

            approved = true;
            var posted = await secondExecutor.PostSignalAsync(instanceId, "B", JsonSerializer.SerializeToElement("approved"));
            Assert.True(posted);

            var report = await resumedRun;
            Assert.Equal(1, aRuns);
            Assert.Equal(NodeState.Succeeded, report.NodeStates["A"]);
            Assert.Equal(NodeState.Succeeded, report.NodeStates["B"]);
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }
}
