namespace GraphLABS.Models;
public partial class Graph
{
    public class DisjointSet
    {
        private readonly Dictionary<Vertex, Vertex> parent = new();

        public void MakeSet(Vertex v)
        {
            parent[v] = v;
        }

        public Vertex Find(Vertex v)
        {
            if (v != parent[v])
            {
                parent[v] = Find(parent[v]); 
            }
            return parent[v];
        }

        public void Union(Vertex a, Vertex b)
        {
            a = Find(a);
            b = Find(b);
            if (a != b)
            {
                parent[a] = b; 
            }
        }
    }

    public static class KruskalAlgorythm
    {
        public static List<Edge> FindMST(Graph graph)
        {
            var edges = new List<Edge>();
            foreach (var vertex in graph.edges)
            {
                foreach (var (adjacentVertex, weight) in vertex.Value)
                {
                    edges.Add(new Edge(vertex.Key, adjacentVertex, weight));
                }
            }

            edges.Sort((a, b) => a.Weigth.CompareTo(b.Weigth));

            var disjointSet = new DisjointSet();
            foreach (var vertex in graph.Vertexes)
            {
                disjointSet.MakeSet(vertex);
            }

            var result = new List<Edge>();
            foreach (var edge in edges)
            {
                var root1 = disjointSet.Find(edge.Source);
                var root2 = disjointSet.Find(edge.Destination);
                if (root1 != root2)
                {
                    result.Add(edge);
                    disjointSet.Union(root1, root2);
                }
            }

            return result;
        }
    }


}
