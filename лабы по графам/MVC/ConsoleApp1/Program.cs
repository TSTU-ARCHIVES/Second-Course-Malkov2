
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
namespace P;
public class Graph
{
    public double this[Vertex a, Vertex b]
    {
        get
        {
            foreach (var (vertex, weight) in edges[a])
            {
                if (vertex.Equals(b))
                    return weight;
            }
            return double.PositiveInfinity;
        }
    }
    internal Dictionary<Vertex, HashSet<(Vertex vertex, int weight)>> edges;
    public List<Vertex> Vertexes { get => edges.Keys.ToList(); }
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
            foreach (var (vertex, weight) in value)
            {
                if (destination.Equals(vertex))
                    return false;
            }
            value.Add((destination, weigth));
            return true;
        }
    }
    public void TraverseInDepth(Vertex v)
        => DFS(v, Enumerable.Range(0, VertexesCount).Select(x => Color.White).ToArray());
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

    public List<Edge> Kruscal(Vertex start)
        => KruskalAlgorythm.FindMST(this, start);

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
    void CycleDFS(Vertex v) => OnCycleDFS?.Invoke(this, new GraphEventArgs(v));

    private bool IsAdjastent(Vertex v, Vertex i) =>
        edges[v].Select(x => x.vertex).Contains(i);

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

    internal static class RobertsFloresAlgrythm
    {
        private static List<Vertex> path = new();
        private static Dictionary<Vertex, bool> visited = new();
        private static List<Vertex> hamiltonianCycle = new();


        internal static void FindHamiltonianCycles(Graph graph, Vertex v, int count)
        {
            visited[v] = true;
            path.Add(v);

            if (count == graph.VertexesCount)
            {
                if (graph.edges[path.Last()].Select(x => x.vertex).Contains(path.First()))
                {
                    hamiltonianCycle = new List<Vertex>(path);
                }
            }
            else
            {
                foreach (var vertex in graph.edges[v])
                {
                    if (!visited.ContainsKey(vertex.vertex) || !visited[vertex.vertex])
                    {
                        FindHamiltonianCycles(graph, vertex.vertex, count + 1);
                    }
                }
            }

            visited[v] = false;
            path.RemoveAt(path.Count - 1);
        }

        internal static List<Vertex> FindHamiltonianCycles(Graph graph, Vertex v)
        {
            FindHamiltonianCycles(graph, v, 1);
            return hamiltonianCycle;
        }
    }

    internal static class PrimAlgorythm
    {
        internal static List<Edge> FindMST(Graph g, Vertex start)
        {
            var res = new List<Edge>();
            var ourComponent = new List<Vertex>
            {
                start
            };

            while (!g.Vertexes.TrueForAll(elem => ourComponent.Contains(elem)))
            {
                foreach (var current in g.edges.Keys)
                {
                    var sorted = g.edges[current].OrderBy<(Vertex, int), int>(v => v.Item2);
                    foreach (var (vertex, weight) in sorted)
                    {
                        if (!ourComponent.Contains(vertex))
                        {
                            ourComponent.Add(vertex);
                            res.Add(new(current, vertex, weight));
                        }
                    }
                }
            }
            return res;
        }
    }

    internal static class KruskalAlgorythm
    {
        internal static List<Edge> FindMST(Graph g, Vertex start)
        {
            var list = new List<Edge>();
            var res = new List<Edge>();
            var ourComponent = new List<Vertex>();
            foreach (var v in g.edges.Keys)
            {
                foreach (var (vertex, weight) in g.edges[v])
                    list.Add(new(v, vertex, weight));
            }
            list.Sort((e, e2) => e.Weigth - e2.Weigth);

            res.Add(list[0]);
            ourComponent.AddRange([list[0].Source, list[0].Destination]);
            while (!g.Vertexes.TrueForAll(elem => ourComponent.Contains(elem)))
            {
                for (int i = 1; i < list.Count; i++)
                {
                    var edge = list[i];
                    var inOurComponent = (ourComponent.Contains(edge.Source)
                        && !ourComponent.Contains(edge.Destination));
                    var inGraphComponent = (!ourComponent.Contains(edge.Source)
                        && ourComponent.Contains(edge.Destination));

                    if (inOurComponent || inGraphComponent)
                    {
                        res.Add(edge);
                        ourComponent.AddRange([edge.Source, edge.Destination]);
                    }
                }
            }
            return res;
        }
    }

    internal static class FleuryAlgorythm
    {
        internal static List<Vertex> FindEulerianCycle(Graph graph, Vertex start)
        {
            var tempGraph = new Dictionary<Vertex, List<Vertex>>();
            foreach (var kv in graph.edges)
            {
                tempGraph.Add(kv.Key, kv.Value.Select(x => x.vertex).ToList());
            }
            var stack = new Stack<Vertex>();
            var cycle = new List<Vertex>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                var currentVertex = stack.Peek();

                if (tempGraph[currentVertex].Count == 0)
                {
                    cycle.Add(stack.Pop());
                }
                else
                {
                    var nextVertex = tempGraph[currentVertex][0];
                    stack.Push(nextVertex);
                    tempGraph[currentVertex].Remove(nextVertex);
                    tempGraph[nextVertex].Remove(currentVertex);
                }
            }

            cycle.Reverse();

            return cycle;
        }
    }

    internal static class DijkstraAlgorythm
    {
        internal static Dictionary<Vertex, double> FindShortestPaths(Graph g, Vertex from)
        {
            var res = new Dictionary<Vertex, double>();
            var visited = new Dictionary<Vertex, bool>();
            foreach (var kv in g.edges)
            {
                res.Add(kv.Key, double.PositiveInfinity);
                visited.Add(kv.Key, false);
            }

            res[from] = 0;
            while (!visited.All(x => x.Value == true))
            {
                var min = res
                    .Where(vertex => !visited[vertex.Key])
                    .MinBy(x => x.Value).Key;
                foreach (var (vertex, weight) in g.edges[min])
                {
                    if (visited[vertex])
                        continue;
                    if (res[vertex] > weight + res[min])
                    {
                        res[vertex] = weight + res[min];
                    }
                }
                visited[min] = true;
            }

            return res;
        }

        internal static Dictionary<Vertex, List<Edge>> FindShortestPath(Graph g, Vertex from, Vertex to)
        {
            var res = new Dictionary<Vertex, List<Edge>>();
            var distances = new Dictionary<Vertex, double>();
            var visited = new Dictionary<Vertex, bool>();
            foreach (var kv in g.edges)
            {
                distances.Add(kv.Key, double.PositiveInfinity);
                visited.Add(kv.Key, false);
                res.Add(kv.Key, []);
            }

            distances[from] = 0;
            while (!visited.All(x => x.Value == true))
            {
                var min = distances
                    .Where(vertex => !visited[vertex.Key])
                    .MinBy(x => x.Value).Key;
                foreach (var (vertex, weight) in g.edges[min])
                {
                    if (visited[vertex])
                        continue;
                    if (distances[vertex] > weight + distances[min])
                    {
                        distances[vertex] = weight + distances[min];
                        res[vertex].AddRange([.. res[min], new(min, vertex, weight)]);
                    }
                }
                visited[min] = true;
            }

            return res;
        }
    }

    internal static class FloydWarshallAlgorythm
    {
        internal static Dictionary<Vertex, Dictionary<Vertex, double>> FindShortestPaths(Graph g)
        {
            var res = new Dictionary<Vertex, Dictionary<Vertex, double>>();
            foreach (var kv in g.edges)
            {
                var dict = new Dictionary<Vertex, double>();
                foreach (var nkv in g.edges)
                {
                    dict.Add(nkv.Key, double.PositiveInfinity);
                }
                res.Add(kv.Key, dict);
                foreach (var (vertex, weight) in g.edges[kv.Key])
                {
                    res[kv.Key][vertex] = weight;
                }
                res[kv.Key][kv.Key] = 0;
            }

            foreach (var k in g.edges.Keys)
                foreach (var i in g.edges.Keys)
                    foreach (var j in g.edges.Keys)
                        res[i][j] = Math.Min(res[i][j], res[i][k] + res[k][j]);

            return res;
        }
    }

    internal static class BellmanFordAlgorythm
    {
        internal static Dictionary<Vertex, double> FindShortestPaths(Graph g, Vertex from)
        {
            var res = new Dictionary<Vertex, double>();
            foreach (var kv in g.edges)
            {
                res.Add(kv.Key, double.PositiveInfinity);
            }

            res[from] = 0;

            for (int i = 0; i < g.VertexesCount - 1; i++)
            {
                foreach (var kv in g.edges)
                {
                    foreach (var (vertex, weight) in kv.Value)
                    {
                        res[vertex] = Math.Min(res[vertex], res[kv.Key] + g[kv.Key, vertex]);
                    }
                }
            }

            return res;
        }
    }


}


public class GraphEventArgs(Vertex vertex) : EventArgs
{
    public Vertex Vertex { get; set; } = vertex;
}

public record struct Vertex(int Number)
{
    public override readonly string ToString() => Convert.ToChar('A' + this.Number).ToString();
}

public record struct Edge(Vertex Source, Vertex Destination, int Weigth);
public class Program
{
    public static void Main()
    {
        Graph graph = new();
    }
}