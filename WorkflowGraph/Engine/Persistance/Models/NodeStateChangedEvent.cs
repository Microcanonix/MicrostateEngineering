using Engine.Workflow;

namespace Engine.Persistance.Models
{
    public sealed record NodeStateChangedEvent<TKey>(
                            long Sequence,
                            DateTimeOffset UtcTimestamp,
                            string EncodedNodeId,
                            NodeState State,
                            string? Message,
                            string? WaitingForSignal = null)
    : WorkflowEvent(Sequence, UtcTimestamp, "node-state-changed")
    where TKey : notnull;
}
