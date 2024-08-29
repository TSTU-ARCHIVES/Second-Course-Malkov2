using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace GraphLABS.Models;
public class Graph
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

    public List<Edge> FindExit()
    {
        var lab = new Labyrinth(this);
        var exit = lab.FindExit().Last();
        return DijkstraAlgorythm.FindShortestPath(this, edges.First().Key, exit)[exit];
    }

    internal class Labyrinth(Graph graphView)
    {
        private enum VertexState
        {
            PART_OF_PATH, EXIT, UNUSED
        }
        public Vertex[] FindExit()
        {
            var list = new LinkedList<Vertex>();
            var searchState = VertexState.PART_OF_PATH;
            graphView.OnGreyVisit += (o, e) =>
            {
                if (searchState == VertexState.PART_OF_PATH)
                {
                    searchState = VertexHandler(o, e, list);
                }
                if (searchState == VertexState.EXIT)
                {
                    list.AddLast(e.Vertex);
                    searchState = VertexState.UNUSED;
                }
            };
            graphView.TraverseInDepth(graphView.edges.First().Key);
            return [.. list];
        }

        private VertexState VertexHandler(object o, GraphEventArgs e, LinkedList<Vertex> vertexes)
        {
            if (graphView.edges[e.Vertex].Count() != 1)
            {
                vertexes.AddLast(e.Vertex);
                return VertexState.PART_OF_PATH;
            }
            else
                return VertexState.EXIT;
        }

    }

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
        internal static List<Vertex> FindHamiltonianCycles(Graph graph, Vertex v)
        {
            return default;
        }

    }

    internal static class PrimAlgorythm
    {
        internal static List<Edge> FindMST(Graph g, Vertex start)
        {
            return default;
        }
    }

    internal static class KruskalAlgorythm
    {
        internal static List<Edge> FindMST(Graph g, Vertex start)
        {
            return default;
        }
    }

    internal static class FleuryAlgorythm
    {
        internal static List<Vertex> FindEulerianCycle(Graph graph, Vertex start)
        {
            return default;
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
                foreach(var (vertex, weight) in g.edges[min])
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
            return default;
        }
    }

    internal static class BellmanFordAlgorythm
    {
        internal static Dictionary<Vertex, double> FindShortestPaths(Graph g, Vertex from)
        {
            return default;
        }
    }

    
}


public class GraphEventArgs(Vertex vertex) : EventArgs
{
    public Vertex Vertex { get; set; } = vertex;
}