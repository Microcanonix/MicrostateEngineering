namespace Engine.Graph.Extensions
{
    public static class TopologyExtensions
    {
        /// <summary>
        /// Returns topological layers where each layer contains nodes with no unmet dependencies.
        /// </summary>
        public static IReadOnlyList<IReadOnlyList<TKey>> GetLayers<TKey>(this IReadOnlyDigraph<TKey> graph)
      where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(graph);

            var inDegree = graph.Vertices.ToDictionary(v => v, v => graph.GetIncoming(v).Count());
            var frontier = new List<TKey>(inDegree.Where(x => x.Value == 0).Select(x => x.Key));
            var layers = new List<IReadOnlyList<TKey>>();
            var processed = 0;

            while (frontier.Count > 0)
            {
                layers.Add(frontier.ToArray());
                processed += frontier.Count;

                var next = new List<TKey>();
                foreach (var vertex in frontier)
                {
                    foreach (var child in graph.GetOutgoing(vertex))
                    {
                        inDegree[child]--;
                        if (inDegree[child] == 0)
                        {
                            next.Add(child);
                        }
                    }
                }

                frontier = next;
            }

            if (processed != graph.Vertices.Count)
            {
                throw new InvalidOperationException("Graph contains a cycle.");
            }

            return layers;
        }

        /// <summary>
        /// Detects whether the graph contains a cycle and returns one cycle path when found.
        /// </summary>
        public static bool TryGetCycle<TKey>(this IReadOnlyDigraph<TKey> graph, out IReadOnlyList<TKey>? cycle)
            where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(graph);

            var state = new Dictionary<TKey, int>();
            var stack = new Stack<TKey>();
            var stackSet = new HashSet<TKey>();

            foreach (var vertex in graph.Vertices)
            {
                if (state.TryGetValue(vertex, out var seen) && seen != 0)
                {
                    continue;
                }

                if (DepthFirstSearch(vertex, graph, state, stack, stackSet, out cycle))
                {
                    return true;
                }
            }

            cycle = null;
            return false;
        }

        /// <summary>
        /// Performs DFS from a node to find a back-edge and reconstruct a cycle path.
        /// </summary>
        private static bool DepthFirstSearch<TKey>(
            TKey node,
            IReadOnlyDigraph<TKey> graph,
            Dictionary<TKey, int> state,
            Stack<TKey> stack,
            HashSet<TKey> stackSet,
            out IReadOnlyList<TKey>? cycle)
            where TKey : notnull
        {
            state[node] = 1;
            stack.Push(node);
            stackSet.Add(node);

            foreach (var child in graph.GetOutgoing(node))
            {
                if (!state.TryGetValue(child, out var childState))
                {
                    childState = 0;
                }

                if (childState == 0)
                {
                    if (DepthFirstSearch(child, graph, state, stack, stackSet, out cycle))
                    {
                        return true;
                    }
                }
                else if (childState == 1 && stackSet.Contains(child))
                {
                    var path = new List<TKey>();
                    foreach (var v in stack)
                    {
                        path.Add(v);
                        if (EqualityComparer<TKey>.Default.Equals(v, child))
                        {
                            break;
                        }
                    }

                    path.Reverse();
                    path.Add(child);
                    cycle = path;
                    return true;
                }
            }

            stack.Pop();
            stackSet.Remove(node);
            state[node] = 2;
            cycle = null;
            return false;
        }
    }
}
