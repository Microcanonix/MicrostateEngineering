using System.Text.Json;

namespace Engine.Persistance.Models
{
    public sealed record ContextSetEvent(
     long Sequence,
     DateTimeOffset UtcTimestamp,
     string ContextKey,
     JsonElement? Value)
     : WorkflowEvent(Sequence, UtcTimestamp, "context-set");
}
