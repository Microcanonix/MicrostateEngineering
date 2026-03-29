namespace Engine.WorkflowExecution
{
    /// <summary>
    /// Options that control execution behaviour of <see cref="WorkflowExecutor{TKey}"/>.
    /// </summary>
    public sealed class WorkflowExecutorOptions
    {
        /// <summary>
        /// Maximum number of nodes that may run in parallel.
        /// Default: <see cref="Environment.ProcessorCount"/>.
        /// </summary>
        /// <remarks>
        /// Set this to control concurrency used by the executor. Must be greater than zero.
        /// Lower this value to conserve CPU / limit resource contention or increase it to
        /// allow more parallel node execution when nodes are I/O bound.
        /// </remarks>
        public int MaxDegreeOfParallelism { get; init; } = Environment.ProcessorCount;

        /// <summary>
        /// When true the executor will cancel the entire run on the first node failure.
        /// Default: <c>true</c>.
        /// </summary>
        /// <remarks>
        /// - If <c>true</c>, a failing node causes the linked cancellation token to be cancelled
        ///   and remaining running work will be requested to stop.
        /// - If <c>false</c>, failures are recorded per-node and execution continues for other ready nodes
        ///   (subject to <see cref="SkipDependentsOnFailure"/>).
        /// </remarks>
        public bool FailFast { get; init; } = true;

        /// <summary>
        /// When true, nodes that depend on a failed/skipped/canceled node will be marked as <c>Skipped</c>.
        /// Default: <c>true</c>.
        /// </summary>
        /// <remarks>
        /// - Useful when downstream work should not run if an upstream dependency failed.
        /// - If <c>false</c>, dependents may still run even if a parent failed (unless prevented by
        ///   other logic or <see cref="FailFast"/> cancelling the run).
        /// - Typical combination: <c>FailFast = true</c> and <c>SkipDependentsOnFailure = true</c>
        ///   to abort quickly and avoid unnecessary downstream work.
        /// </remarks>
        public bool SkipDependentsOnFailure { get; init; } = true;
    }
}
