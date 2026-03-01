using Engine.Workflow;
using Engine.WorkflowExecution;

namespace GraphAndWorkflowTests;

public class WorkflowExecutorTests
{
    [Fact]
    public async Task RunAsync_ExecutesThreeNodeChain()
    {
        var workflow = new Workflow<string>(StringComparer.OrdinalIgnoreCase)
            .AddNode(new WorkflowNode<string>("A", async ct =>
            {
                await Task.Delay(5, ct);
                return NodeResult.Success();
            }))
            .AddNode(new WorkflowNode<string>("B", async ct =>
            {
                await Task.Delay(5, ct);
                return NodeResult.Success();
            }))
            .AddNode(new WorkflowNode<string>("C", async ct =>
            {
                await Task.Delay(5, ct);
                return NodeResult.Success();
            }))
            .AddDependency("A", "B")
            .AddDependency("B", "C");

        var executor = new WorkflowExecutor<string>();
        var report = await executor.RunAsync(workflow, new WorkflowExecutorOptions
        {
            MaxDegreeOfParallelism = 2,
            FailFast = true,
            SkipDependentsOnFailure = true
        });

        Assert.Null(report.Cycle);
        Assert.Equal(NodeState.Succeeded, report.NodeStates["A"]);
        Assert.Equal(NodeState.Succeeded, report.NodeStates["B"]);
        Assert.Equal(NodeState.Succeeded, report.NodeStates["C"]);
        Assert.True(report.Succeeded);
    }

    [Fact]
    public async Task RunAsync_NodeWaitingForInput_DoesNotAdvanceDependents()
    {
        var workflow = new Workflow<string>()
            .AddNode("A", (_, _) => Task.FromResult(WorkflowNodeResult.WaitingForInput))
            .AddNode("B", (_, _) => Task.FromResult(WorkflowNodeResult.Success))
            .AddDependency("A", "B");

        var executor = new WorkflowExecutor<string>();
        var report = await executor.RunAsync(workflow, new WorkflowExecutorOptions
        {
            MaxDegreeOfParallelism = 1,
            FailFast = true,
            SkipDependentsOnFailure = true
        }, CancellationToken.None);

        Assert.Equal(NodeState.WaitingForInput, report.NodeStates["A"]);
        Assert.Equal(NodeState.Pending, report.NodeStates["B"]);
        Assert.Equal(1, report.WaitingCount);
        Assert.Contains("A", report.WaitingNodes);
        Assert.False(report.Succeeded);
    }

    [Fact]
    public async Task RunAsync_ResumeWaitingNode_CompletesWorkflow()
    {
        var readyForApproval = false;
        var waitingObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var workflow = new Workflow<string>()
            .AddNode("A", (_, _) =>
            {
                return Task.FromResult(readyForApproval ? WorkflowNodeResult.Success : WorkflowNodeResult.WaitingForInput);
            })
            .AddNode("B", (_, _) => Task.FromResult(WorkflowNodeResult.Success))
            .AddDependency("A", "B");

        var executor = new WorkflowExecutor<string>();
        executor.NodeStateChanged += (_, args) =>
        {
            if (args.NodeId == "A" && args.State == NodeState.WaitingForInput)
            {
                waitingObserved.TrySetResult();
            }
        };

        var runTask = executor.RunAsync(workflow, new WorkflowExecutorOptions
        {
            MaxDegreeOfParallelism = 1,
            FailFast = true,
            SkipDependentsOnFailure = true
        });

        await waitingObserved.Task;
        readyForApproval = true;

        var resumed = executor.TryResume("A");
        Assert.True(resumed);

        var report = await runTask;

        Assert.Equal(NodeState.Succeeded, report.NodeStates["A"]);
        Assert.Equal(NodeState.Succeeded, report.NodeStates["B"]);
        Assert.Empty(report.WaitingNodes);
        Assert.True(report.Succeeded);
    }
}
