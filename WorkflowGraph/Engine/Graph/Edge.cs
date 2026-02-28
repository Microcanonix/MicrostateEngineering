namespace Engine.Graph
{
    public readonly record struct Edge<TKey>(TKey From, TKey To);
}
