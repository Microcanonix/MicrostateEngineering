using Engine.Graph;

namespace Engine.Workflow
{
    public sealed class Workflow<TKey>
        where TKey : notnull
    {
        private readonly Dictionary<TKey, WorkflowNode<TKey>> _nodes;

        /// <summary>
        /// Creates an empty workflow and underlying dependency graph.
        /// </summary>
        public Workflow(IEqualityComparer<TKey>? comparer = null)
        {
            Graph = new KeyedDigraph<TKey>(comparer);
            _nodes = new Dictionary<TKey, WorkflowNode<TKey>>(comparer ?? EqualityComparer<TKey>.Default);
        }

        /// <summary>
        /// Gets the directed dependency graph for workflow nodes.
        /// </summary>
        public KeyedDigraph<TKey> Graph { get; }

        /// <summary>
        /// Gets all nodes currently registered in the workflow.
        /// </summary>
        public IReadOnlyCollection<WorkflowNode<TKey>> Nodes => _nodes.Values;

        /// <summary>
        /// Adds a node to the workflow and to the graph vertex set.
        /// </summary>
        public Workflow<TKey> AddNode(WorkflowNode<TKey> node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (!_nodes.TryAdd(node.Id, node))
            {
                throw new InvalidOperationException($"Node '{node.Id}' already exists.");
            }

            Graph.AddVertex(node.Id);
            return this;
        }

        /// <summary>
        /// Adds a node using the new execution contract.
        /// </summary>
        public Workflow<TKey> AddNode(TKey id, Func<WorkflowContext, CancellationToken, Task<WorkflowNodeResult>> runAsync)
        {
            return AddNode(new WorkflowNode<TKey>(id, runAsync));
        }

        /// <summary>
        /// Adds a node using the legacy execution contract that only reports completion by not throwing.
        /// </summary>
        public Workflow<TKey> AddNode(TKey id, Func<CancellationToken, Task> runAsync)
        {
            return AddNode(new WorkflowNode<TKey>(id, runAsync));
        }

        /// <summary>
        /// Adds a node using the legacy execution contract that returns <see cref="NodeResult"/>.
        /// </summary>
        public Workflow<TKey> AddNode(TKey id, Func<CancellationToken, Task<NodeResult>> runAsync)
        {
            return AddNode(new WorkflowNode<TKey>(id, runAsync));
        }

        /// <summary>
        /// Adds a dependency edge indicating <paramref name="dependent"/> requires <paramref name="dependency"/>.
        /// </summary>
        public Workflow<TKey> AddDependency(TKey dependency, TKey dependent)
        {
            EnsureNode(dependency);
            EnsureNode(dependent);
            Graph.AddEdge(dependency, dependent);
            return this;
        }

        /// <summary>
        /// Gets a node by id, throwing when it is not defined.
        /// </summary>
        public WorkflowNode<TKey> GetNode(TKey id)
        {
            EnsureNode(id);
            return _nodes[id];
        }

        /// <summary>
        /// Ensures the workflow contains the specified node id.
        /// </summary>
        private void EnsureNode(TKey id)
        {
            if (!_nodes.ContainsKey(id))
            {
                throw new KeyNotFoundException($"Node '{id}' is not defined.");
            }
        }
    }

}
