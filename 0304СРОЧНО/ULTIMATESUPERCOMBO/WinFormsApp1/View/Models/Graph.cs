using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLABS.Models;
public class Graph
{
    private Dictionary<Vertex, HashSet<Vertex>> edges;
    public List<Vertex> Vertexes { get => edges.Keys.ToList();  }
    public int VertexesCount { get => edges.Keys.Count; }

    public event EventHandler<GraphEventArgs> OnGreyVisit;

    public event EventHandler<GraphEventArgs> OnWhiteVisit;

    public event EventHandler<GraphEventArgs> OnBlackVisit;

    public Graph() => edges = [];

    public void AddVertex(Vertex vertex)
    {
        if (!edges.ContainsKey(vertex))
            edges.Add(vertex, []);
    }

    public bool AddEdge(Vertex source, Vertex destination)
    {
        if (!edges.TryGetValue(source, out HashSet<Vertex>? value))
        {
            edges.Add(source, [destination]);
            return true;
        }
        else
        {
            value.Add(destination);
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
            if (vertexes[adjVertex.Number] == Color.White)
            {
                WhiteVisit(adjVertex);
                DFS(adjVertex, vertexes);
            }
            if (vertexes[adjVertex.Number] == Color.Gray)
            {
                GreyVisit(adjVertex);
            }
        }
        vertexes[vertex.Number] = Color.Black;
        BlackVisit(vertex);
    }

    private void BFS(Vertex vertex)
    {
        var queue = new Queue<Vertex>();
        GreyVisit(vertex);
        BFS(vertex, queue);
    }
    private void BFS(Vertex vertex, Queue<Vertex> queue)
    {
        foreach (var adj in edges[vertex])
        {
            queue.Enqueue(adj);
            GreyVisit(adj);
        }
        while (queue.Count > 0)
        {
            var v = queue.Dequeue();
            BFS(v);
        }
        BlackVisit(vertex);
    }

    void GreyVisit(Vertex v) => OnGreyVisit?.Invoke(this, new GraphEventArgs(v));
    void WhiteVisit(Vertex v) => OnWhiteVisit?.Invoke(this, new GraphEventArgs(v));
    void BlackVisit(Vertex v) => OnBlackVisit?.Invoke(this, new GraphEventArgs(v));

    private bool IsAdjastent(Vertex v, Vertex i) =>
        edges[v].Contains(i);

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