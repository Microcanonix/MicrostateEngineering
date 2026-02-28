namespace Engine.Graph
{
    public interface IDigraph<TKey> : IReadOnlyDigraph<TKey>
    where TKey : notnull
    {
        /// <summary>
        /// Adds a vertex to the graph if it does not already exist.
        /// </summary>
        bool AddVertex(TKey key);

        /// <summary>
        /// Adds a directed edge from one existing vertex to another.
        /// </summary>
        bool AddEdge(TKey from, TKey to);

        /// <summary>
        /// Removes a directed edge when it exists.
        /// </summary>
        bool RemoveEdge(TKey from, TKey to);

        /// <summary>
        /// Removes a vertex and all connected edges when it exists.
        /// </summary>
        bool RemoveVertex(TKey key);
    }
}
