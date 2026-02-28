namespace Engine.Workflow
{
    public sealed class WorkflowNode<TKey>
       where TKey : notnull
    {
        /// <summary>
        /// Creates a workflow node with an id and asynchronous execution delegate.
        /// </summary>
        public WorkflowNode(TKey id, Func<WorkflowContext, CancellationToken, Task<WorkflowNodeResult>> runAsync)
        {
            Id = id;
            RunAsync = runAsync ?? throw new ArgumentNullException(nameof(runAsync));
        }

        /// <summary>
        /// Creates a workflow node using the legacy delegate shape and maps successful completion to <see cref="WorkflowNodeResult.Success"/>.
        /// </summary>
        public WorkflowNode(TKey id, Func<CancellationToken, Task> runAsync)
            : this(id, WrapLegacy(runAsync))
        {
        }

        /// <summary>
        /// Creates a workflow node using the legacy result type and maps it to <see cref="WorkflowNodeResult"/>.
        /// </summary>
        public WorkflowNode(TKey id, Func<CancellationToken, Task<NodeResult>> runAsync)
            : this(id, WrapLegacy(runAsync))
        {
        }

        /// <summary>
        /// Gets the unique node id.
        /// </summary>
        public TKey Id { get; }

        /// <summary>
        /// Gets the asynchronous operation that executes this node.
        /// </summary>
        public Func<WorkflowContext, CancellationToken, Task<WorkflowNodeResult>> RunAsync { get; }

        /// <summary>
        /// Wraps a legacy delegate that only indicates completion by not throwing.
        /// </summary>
        public static Func<WorkflowContext, CancellationToken, Task<WorkflowNodeResult>> WrapLegacy(Func<CancellationToken, Task> runAsync)
        {
            ArgumentNullException.ThrowIfNull(runAsync);
            return async (_, cancellationToken) =>
            {
                await runAsync(cancellationToken).ConfigureAwait(false);
                return WorkflowNodeResult.Success;
            };
        }

        /// <summary>
        /// Wraps a legacy delegate that returns <see cref="NodeResult"/> into the newer <see cref="WorkflowNodeResult"/> contract.
        /// </summary>
        public static Func<WorkflowContext, CancellationToken, Task<WorkflowNodeResult>> WrapLegacy(Func<CancellationToken, Task<NodeResult>> runAsync)
        {
            ArgumentNullException.ThrowIfNull(runAsync);
            return async (_, cancellationToken) =>
            {
                var result = await runAsync(cancellationToken).ConfigureAwait(false);
                return result.IsSuccess ? WorkflowNodeResult.Success : WorkflowNodeResult.Failure;
            };
        }
    }
}
