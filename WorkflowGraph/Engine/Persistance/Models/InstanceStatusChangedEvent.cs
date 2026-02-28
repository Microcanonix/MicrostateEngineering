namespace Engine.Persistance.Models
{
    public sealed record InstanceStatusChangedEvent(
    long Sequence,
    DateTimeOffset UtcTimestamp,
    WorkflowInstanceStatus Status)
    : WorkflowEvent(Sequence, UtcTimestamp, "instance-status-changed");
}
