namespace Engine.Persistance.Models
{
    public abstract record WorkflowEvent(long Sequence, DateTimeOffset UtcTimestamp, string Type);
}
