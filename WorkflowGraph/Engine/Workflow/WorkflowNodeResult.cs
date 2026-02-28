namespace Engine.Workflow
{
    /// <summary>
    /// Represents the outcome of a workflow node execution attempt.
    /// </summary>
    public enum WorkflowNodeResult
    {
        /// <summary>
        /// The node completed successfully.
        /// </summary>
        Success,

        /// <summary>
        /// The node execution failed.
        /// </summary>
        Failure,

        /// <summary>
        /// The node is intentionally paused and waiting for external input before continuing.
        /// </summary>
        WaitingForInput
    }
}
