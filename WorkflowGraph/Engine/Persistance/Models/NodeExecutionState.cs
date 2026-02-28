using Engine.Workflow;

namespace Engine.Persistance.Models
{
    public sealed record NodeExecutionState<TKey>(
                        TKey NodeId,
                         NodeState State,
                        string? Message = null,
                        string? WaitingForSignal = null,
                        LeaseInfo? Lease = null)
    where TKey : notnull;
}
