using Engine.Graph;
using Engine.Graph.Extensions;

namespace GraphAndWorkflowTests;

public class TopologyTests
{
    [Fact]
    public void GetLayers_ReturnsExpectedKahnLayers()
    {
        var g = new KeyedDigraph<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var v in new[] { "A", "B", "C", "D", "E" })
        {
            g.AddVertex(v);
        }

        g.AddEdge("A", "C");
        g.AddEdge("B", "C");
        g.AddEdge("C", "D");
        g.AddEdge("C", "E");

        var layers = g.GetLayers();

        Assert.Equal(3, layers.Count);
        Assert.Equal(new[] { "A", "B" }, layers[0].OrderBy(x => x));
        Assert.Equal(new[] { "C" }, layers[1]);
        Assert.Equal(new[] { "D", "E" }, layers[2].OrderBy(x => x));
    }

    [Fact]
    public void TryGetCycle_ReturnsConcreteCyclePath()
    {
        var g = new KeyedDigraph<int>();
        g.AddVertex(1);
        g.AddVertex(2);
        g.AddVertex(3);
        g.AddEdge(1, 2);
        g.AddEdge(2, 3);
        g.AddEdge(3, 1);

        var hasCycle = g.TryGetCycle(out var cycle);

        Assert.True(hasCycle);
        Assert.NotNull(cycle);
        Assert.True(cycle!.Count >= 4);
        Assert.Equal(cycle[0], cycle[^1]);
    }
}
