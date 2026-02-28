namespace Engine.Graph.Extensions
{
    public static class DigraphExtensions
    {
        /// <summary>
        /// Invokes an action once for each vertex in the graph.
        /// </summary>
        public static void ForEachVertex<TKey>(this IReadOnlyDigraph<TKey> graph, Action<TKey> action)
            where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(graph);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var vertex in graph.Vertices)
            {
                action(vertex);
            }
        }

        /// <summary>
        /// Invokes an action for every directed edge in the graph.
        /// </summary>
        public static void ForEachEdge<TKey>(this IReadOnlyDigraph<TKey> graph, Action<Edge<TKey>> action)
            where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(graph);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var from in graph.Vertices)
            {
                foreach (var to in graph.GetOutgoing(from))
                {
                    action(new Edge<TKey>(from, to));
                }
            }
        }

        /// <summary>
        /// Invokes an action for each edge that starts at the specified source vertex.
        /// </summary>
        public static void ForEachEmanatingEdge<TKey>(this IReadOnlyDigraph<TKey> graph, TKey from, Action<Edge<TKey>> action)
            where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(graph);
            ArgumentNullException.ThrowIfNull(action);

            foreach (var to in graph.GetOutgoing(from))
            {
                action(new Edge<TKey>(from, to));
            }
        }
    }
}
