using System.Text.Json;

namespace Engine.Persistance.Models
{
    public sealed record SignalPostedEvent(
    long Sequence,
    DateTimeOffset UtcTimestamp,
    string SignalKey,
    JsonElement? Payload)
    : WorkflowEvent(Sequence, UtcTimestamp, "signal-posted");
}
