namespace Engine.Workflow
{
    public enum NodeState
    {
        Pending,
        Running,
        WaitingForInput,
        Succeeded,
        Failed,
        Skipped,
        Canceled
    }
}
