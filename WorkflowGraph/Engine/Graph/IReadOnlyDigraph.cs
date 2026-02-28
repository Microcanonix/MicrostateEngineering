namespace Engine.Graph
{
    public interface IReadOnlyDigraph<TKey>
     where TKey : notnull
    {
        /// <summary>
        /// Gets all vertices currently stored in the graph.
        /// </summary>
        IReadOnlyCollection<TKey> Vertices { get; }

        /// <summary>
        /// Checks whether the specified vertex exists.
        /// </summary>
        bool ContainsVertex(TKey key);

        /// <summary>
        /// Checks whether a directed edge exists from <paramref name="from"/> to <paramref name="to"/>.
        /// </summary>
        bool ContainsEdge(TKey from, TKey to);

        /// <summary>
        /// Gets all vertices that can be reached directly from <paramref name="from"/>.
        /// </summary>
        IEnumerable<TKey> GetOutgoing(TKey from);

        /// <summary>
        /// Gets all vertices that have a direct edge into <paramref name="to"/>.
        /// </summary>
        IEnumerable<TKey> GetIncoming(TKey to);
    }
}
