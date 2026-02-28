namespace Engine.WorkflowExecution
{
    public sealed class WorkflowExecutorOptions
    {
        public int MaxDegreeOfParallelism { get; init; } = Environment.ProcessorCount;

        public bool FailFast { get; init; } = true;

        public bool SkipDependentsOnFailure { get; init; } = true;
    }
}
