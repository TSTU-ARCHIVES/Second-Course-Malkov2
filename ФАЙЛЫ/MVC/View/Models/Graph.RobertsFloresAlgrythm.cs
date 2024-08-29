namespace GraphLABS.Models;
public partial class Graph
{
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

    
}
