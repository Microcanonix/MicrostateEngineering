namespace Engine.Workflow
{
    public readonly record struct NodeResult(bool IsSuccess, string? Message = null)
    {
        /// <summary>
        /// Creates a successful node result with an optional message.
        /// </summary>
        public static NodeResult Success(string? message = null) => new(true, message);

        /// <summary>
        /// Creates a failed node result with an optional message.
        /// </summary>
        public static NodeResult Failure(string? message = null) => new(false, message);
    }
}
