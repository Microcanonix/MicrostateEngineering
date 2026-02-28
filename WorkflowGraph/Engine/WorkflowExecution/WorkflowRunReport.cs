using Engine.Workflow;

namespace Engine.WorkflowExecution
{
    public sealed class WorkflowRunReport<TKey>
     where TKey : notnull
    {
        /// <summary>
        /// Gets the final state for each node.
        /// </summary>
        public required IReadOnlyDictionary<TKey, NodeState> NodeStates { get; init; }

        /// <summary>
        /// Gets the detected cycle when the workflow graph is invalid; otherwise <see langword="null"/>.
        /// </summary>
        public required IReadOnlyList<TKey>? Cycle { get; init; }

        /// <summary>
        /// Gets the node ids that are currently waiting for external input.
        /// </summary>
        public required IReadOnlyList<TKey> WaitingNodes { get; init; }

        /// <summary>
        /// Gets the number of nodes currently waiting for external input.
        /// </summary>
        public int WaitingCount => WaitingNodes.Count;

        /// <summary>
        /// Gets whether the workflow completed without cycle errors and without failed/canceled/waiting nodes.
        /// </summary>
        public bool Succeeded =>
            Cycle is null
            && NodeStates.Values.All(x => x == NodeState.Succeeded || x == NodeState.Skipped);
    }
}
