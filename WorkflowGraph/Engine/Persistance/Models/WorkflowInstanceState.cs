using System.Text.Json;

namespace Engine.Persistance.Models
{
    public sealed record WorkflowInstanceState<TKey>(
    Guid InstanceId,
    WorkflowDefinitionRef DefinitionRef,
    WorkflowInstanceStatus Status,
    DateTimeOffset CreatedUtc,
    DateTimeOffset UpdatedUtc,
    long LastAppliedEventSequence,
    IReadOnlyDictionary<string, NodeExecutionState<TKey>> Nodes,
    IReadOnlyDictionary<string, JsonElement> Context)
    where TKey : notnull;
}
