namespace Engine.Persistance.Models
{
    public sealed record SignalConsumedEvent(
    long Sequence,
    DateTimeOffset UtcTimestamp,
    string SignalKey,
    string EncodedNodeId)
    : WorkflowEvent(Sequence, UtcTimestamp, "signal-consumed");
}
