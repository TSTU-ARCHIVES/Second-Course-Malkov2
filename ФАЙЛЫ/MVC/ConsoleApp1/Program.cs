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


    public List<Edge> Kruscal(Vertex start)
        => KruskalAlgorythm.FindMST(this, start);


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



}


public class GraphEventArgs(Vertex vertex) : EventArgs
{
    public Vertex Vertex { get; set; } = vertex;
}

public record struct Vertex(int Number)
{
    public override readonly string ToString() => Convert.ToChar('A' + this.Number).ToString();

    public override int GetHashCode()
    {
        return Number;
    }
}

public record struct Edge(Vertex Source, Vertex Destination, int Weigth);
public class Program
{
    public static int Difference(byte[] first, byte[] second, int len)
    {
        int res = 0;
        for(int i = 0; i < len; i ++)
        {
            res += (int)Math.Pow(2, i) * (first[i] - second[i]);
        }
        return Math.Abs(res);
    }

    public static int Weigth(byte[] file, int len)
    {
        int res = 0;

        for (int i = 0; i < len; i++)
        {
            res += (int)Math.Pow(2, i) * file[i];
        }
        return res;
    }

    static void RebuildGraph(Graph graph, List<byte[]> files, int len)
    {
        for (int i = 0; i < files.Count; i++)
        {
            var vertex = new Vertex(i);
            graph.AddVertex(vertex);
            for (int j = 0; j < files.Count; j++)
            {
                if (i != j)
                {
                    var newVertex = new Vertex(j);
                    var minWeigth = Math.Min(Difference(files[i], files[j], len), Weigth(files[j], len));
                    graph.AddEdge(vertex, newVertex, minWeigth);
                }
            }
        }



    }

    static int Lightest(int v, Graph graph, List<byte[]> files, int len, out List<Edge> pathRes, out int start)
    {
        int startWeigth = Weigth(files[v], len);
        start = startWeigth;
        var path = graph.Kruscal(new(v));
        pathRes = path;
        foreach(var edge in path)
        {
            startWeigth += edge.Weigth;
        }
        return startWeigth;
    }

    public static void Main()
    {
        var files = new List<byte[]>();
        files.Add([0, 1, 1, 1, 0, 1]);
        files.Add([1, 1, 0, 0, 1, 1]);
        files.Add([1, 1, 1, 0, 1, 1]);


        int len = 6;
        var graph = new Graph();
        var min = int.MaxValue;
        RebuildGraph(graph, files, len);
        List<Edge> path = new();
        List<Edge> resPath = new();
        int start = 0;
        int resStart = 0;
        int v = -1;
        for (int i = 0; i < files.Count; i++)
        {
            var potential = Lightest(i, graph, files, len, out path, out start);
            if (potential < min)
            {
                v = i;
                resStart = start;
                min = potential;
                resPath = path;
            }
        }

        Console.WriteLine("Start file: " + v + ", weigth: " + resStart);
        Console.WriteLine(resPath.Aggregate("", (a, b)=> a.ToString() + " " + b) );
        Console.WriteLine(min);
    }
}