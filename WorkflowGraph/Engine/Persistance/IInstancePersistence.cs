using Engine.Persistance.Models;

namespace Engine.Persistance
{
    public interface IInstancePersistence<TKey>
        where TKey : notnull
    {
        Task<WorkflowInstanceState<TKey>> LoadAsync(Guid instanceId, CancellationToken ct);

        Task AppendEventAsync(Guid instanceId, WorkflowEvent evt, CancellationToken ct);

        Task SaveSnapshotAsync(WorkflowInstanceState<TKey> snapshot, CancellationToken ct);
    }
}
