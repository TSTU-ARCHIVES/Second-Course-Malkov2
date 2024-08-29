using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace GraphLABS.Models;
public partial class Graph
{
    public double this[Vertex a, Vertex b]
    {
        get
        {
            foreach(var (vertex, weight) in edges[a])
            {
                if (vertex.Equals(b))
                    return weight;
            }
            return double.PositiveInfinity;
        }
    }
    internal Dictionary<Vertex, HashSet<(Vertex vertex, int weight)>> edges;
    public List<Vertex> Vertexes { get => edges.Keys.ToList();  }
    public int VertexesCount { get => edges.Keys.Count; }

    public event EventHandler<GraphEventArgs> OnGreyVisit;

    public event EventHandler<GraphEventArgs> OnWhiteVisit;

    public event EventHandler<GraphEventArgs> OnBlackVisit;

    public event EventHandler<GraphEventArgs> OnCycleDFS;

    public Graph() => edges = [];

    public void AddVertex(Vertex vertex)
    {
        if (!edges.ContainsKey(vertex))
            edges.Add(vertex, []);
    }

    public bool AddEdge(Vertex source, Vertex destination, int weigth)
    {
        if (!edges.TryGetValue(source, out var value))
        {
            edges.Add(source, [(destination, weigth)]);
            return true;
        }
        else
        {
            foreach(var (vertex, weight) in value)
            {
                if (destination.Equals(vertex))
                    return false;
            }
            value.Add((destination, weigth));
            return true;
        }
    }
    public void TraverseInDepth()
    { 
        foreach (var v in edges.Keys)
            DFS(v, Enumerable.Range(0, VertexesCount).Select(x => Color.White).ToArray()); 
    
    }
    public void TraverseInBreadth(Vertex v) => BFS(v);

    private void DFS(Vertex vertex, Color[] vertexes)
    {
        vertexes[vertex.Number] = Color.Gray;
        GreyVisit(vertex);
        foreach (var adjVertex in edges[vertex])
        {
            if (vertexes[adjVertex.vertex.Number] == Color.White)
            {
                WhiteVisit(adjVertex.vertex);
                DFS(adjVertex.vertex, vertexes);
            }
            if (vertexes[adjVertex.vertex.Number] == Color.Gray)
            {
                CycleDFS(adjVertex.vertex);
                GreyVisit(adjVertex.vertex);
            }
        }
        vertexes[vertex.Number] = Color.Black;
        BlackVisit(vertex);
    }



    private void BFS(Vertex vertex)
    {
        var queue = new Queue<Vertex>();
        Color[] vertexes = new Color[VertexesCount];
        GreyVisit(vertex);
        vertexes[vertex.Number] = Color.Gray;
        BFS(vertex, queue, vertexes);
    }
    private void BFS(Vertex vertex, Queue<Vertex> queue, Color[] vertexes)
    {
        vertexes[vertex.Number] = Color.Gray;
        foreach (var adj in edges[vertex])
        {
            if (vertexes[adj.vertex.Number] == Color.Gray)
                continue;
            queue.Enqueue(adj.vertex);
            GreyVisit(adj.vertex);
        }
        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            BFS(v, queue, vertexes);
        }
        vertexes[vertex.Number] = Color.Black;
        BlackVisit(vertex);
    }

    public List<Vertex> HamiltonianPath(Vertex startVertex) 
        => RobertsFloresAlgrythm.FindHamiltonianCycles(this, startVertex);

    public List<Vertex> EulerPath(Vertex startVertex)
        => FleuryAlgorythm.FindEulerianCycle(this, startVertex);

    public List<Edge> Kruscal()
        => KruskalAlgorythm.FindMST(this);

    public List<Edge> Prim(Vertex start)
        => PrimAlgorythm.FindMST(this, start);

    public Dictionary<Vertex, double> Dijkstra(Vertex from) =>
        DijkstraAlgorythm.FindShortestPaths(this, from);
    public Dictionary<Vertex, List<Edge>> Dijkstra(Vertex from, Vertex to) =>
        DijkstraAlgorythm.FindShortestPath(this, from, to);
    public Dictionary<Vertex, double> Ford(Vertex from) =>
        BellmanFordAlgorythm.FindShortestPaths(this, from);

    public Dictionary<Vertex, Dictionary<Vertex, double>> Floyd() =>
        FloydWarshallAlgorythm.FindShortestPaths(this);

    public void Clear() => edges.Clear();

    void GreyVisit(Vertex v) => OnGreyVisit?.Invoke(this, new GraphEventArgs(v));
    void WhiteVisit(Vertex v) => OnWhiteVisit?.Invoke(this, new GraphEventArgs(v));
    void BlackVisit(Vertex v) => OnBlackVisit?.Invoke(this, new GraphEventArgs(v));

    bool hasCycle = false;
    void CycleDFS(Vertex v) 
    {
        hasCycle = true;
        OnCycleDFS?.Invoke(this, new GraphEventArgs(v)); 
    }

    private bool IsAdjastent(Vertex v, Vertex i) =>
        edges[v].Select(x => x.vertex).Contains(i);

    public bool IsTree()
    {
        var roots = edges.Keys.Where(v => !GetParents(v).Any());
        if (roots.Count() != 1)
            return false;

        foreach(var v in edges.Keys.Except(roots))
        {
            if (GetParents(v).Count() > 1)
                return false;
        }

        TraverseInDepth();

        return IsDirected() && !HasLoops() && !hasCycle;
    }


    public bool HasLoops()
        => edges.Keys.Any(vertex => IsAdjastent(vertex, vertex));

    public bool IsDirected()
    {
        foreach(var a in edges.Keys)
        {
            foreach(var b in edges.Keys)
            {
                if (!(IsAdjastent(a, b) && IsAdjastent(b, a)) )
                    return true;
            }
        }
        return false;
    }

    public IEnumerable<Vertex> GetChildren(Vertex vertex) =>
        edges[vertex].Select(pair => pair.vertex);

    public IEnumerable<Vertex> GetParents(Vertex vertex) =>
        edges.
        Where(kv => kv.Value.Select(x => x.vertex).Contains(vertex)).
        Select(v => v.Key);

    public override string ToString()
    {
        var res = "";
        foreach (var pair in edges)
        {
            res += $"Vertex {pair.Key}, adjastent: ";
            foreach (var edges in pair.Value)
            {
                res += $"{edges} ";
            }
            res += "\n";
        }
        return res;
    }

    
}


public class GraphEventArgs(Vertex vertex) : EventArgs
{
    public Vertex Vertex { get; set; } = vertex;
}