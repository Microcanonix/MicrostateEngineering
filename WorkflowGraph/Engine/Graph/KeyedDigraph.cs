namespace Engine.Graph
{
    public sealed class KeyedDigraph<TKey> : IDigraph<TKey>
    where TKey : notnull
    {
        private readonly Dictionary<TKey, HashSet<TKey>> _outgoing;
        private readonly Dictionary<TKey, HashSet<TKey>> _incoming;

        /// <summary>
        /// Creates an empty directed graph keyed by <typeparamref name="TKey"/>.
        /// </summary>
        public KeyedDigraph(IEqualityComparer<TKey>? comparer = null)
        {
            comparer ??= EqualityComparer<TKey>.Default;
            _outgoing = new Dictionary<TKey, HashSet<TKey>>(comparer);
            _incoming = new Dictionary<TKey, HashSet<TKey>>(comparer);
        }

        /// <summary>
        /// Gets all vertices in insertion map order.
        /// </summary>
        public IReadOnlyCollection<TKey> Vertices => _outgoing.Keys;

        /// <summary>
        /// Adds a vertex when it does not already exist.
        /// </summary>
        public bool AddVertex(TKey key)
        {
            if (_outgoing.ContainsKey(key))
            {
                return false;
            }

            _outgoing[key] = new HashSet<TKey>(_outgoing.Comparer);
            _incoming[key] = new HashSet<TKey>(_incoming.Comparer);
            return true;
        }

        /// <summary>
        /// Adds a directed edge from <paramref name="from"/> to <paramref name="to"/>.
        /// </summary>
        public bool AddEdge(TKey from, TKey to)
        {
            EnsureVertex(from);
            EnsureVertex(to);

            var added = _outgoing[from].Add(to);
            if (added)
            {
                _incoming[to].Add(from);
            }

            return added;
        }

        /// <summary>
        /// Removes a directed edge from <paramref name="from"/> to <paramref name="to"/>.
        /// </summary>
        public bool RemoveEdge(TKey from, TKey to)
        {
            if (!_outgoing.TryGetValue(from, out var outgoing))
            {
                return false;
            }

            var removed = outgoing.Remove(to);
            if (removed && _incoming.TryGetValue(to, out var incoming))
            {
                incoming.Remove(from);
            }

            return removed;
        }

        /// <summary>
        /// Removes a vertex and all incoming/outgoing edges connected to it.
        /// </summary>
        public bool RemoveVertex(TKey key)
        {
            if (!_outgoing.ContainsKey(key))
            {
                return false;
            }

            foreach (var parent in _incoming[key].ToArray())
            {
                _outgoing[parent].Remove(key);
            }

            foreach (var child in _outgoing[key].ToArray())
            {
                _incoming[child].Remove(key);
            }

            _incoming.Remove(key);
            _outgoing.Remove(key);
            return true;
        }

        /// <summary>
        /// Determines whether a vertex exists in the graph.
        /// </summary>
        public bool ContainsVertex(TKey key) => _outgoing.ContainsKey(key);

        /// <summary>
        /// Determines whether a directed edge exists from <paramref name="from"/> to <paramref name="to"/>.
        /// </summary>
        public bool ContainsEdge(TKey from, TKey to)
        {
            return _outgoing.TryGetValue(from, out var edges) && edges.Contains(to);
        }

        /// <summary>
        /// Gets vertices reachable by a single outgoing edge from <paramref name="from"/>.
        /// </summary>
        public IEnumerable<TKey> GetOutgoing(TKey from)
        {
            EnsureVertex(from);
            return _outgoing[from];
        }

        /// <summary>
        /// Gets vertices that have an outgoing edge into <paramref name="to"/>.
        /// </summary>
        public IEnumerable<TKey> GetIncoming(TKey to)
        {
            EnsureVertex(to);
            return _incoming[to];
        }

        /// <summary>
        /// Validates that a vertex exists before edge traversal operations.
        /// </summary>
        private void EnsureVertex(TKey key)
        {
            if (!_outgoing.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Vertex '{key}' is not defined.");
            }
        }
    }
}
