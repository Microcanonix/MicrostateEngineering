using Engine.Workflow;

namespace Engine.WorkflowExecution
{
    public sealed class WorkflowNodeStateChangedEventArgs<TKey> : EventArgs
    where TKey : notnull
    {
        /// <summary>
        /// Creates state change event data for a workflow node.
        /// </summary>
        public WorkflowNodeStateChangedEventArgs(TKey nodeId, NodeState state, string? message = null)
        {
            NodeId = nodeId;
            State = state;
            Message = message;
        }

        public TKey NodeId { get; }

        public NodeState State { get; }

        public string? Message { get; }
    }
}
