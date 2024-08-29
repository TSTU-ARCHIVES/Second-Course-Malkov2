namespace GraphLABS.Models;
public partial class Graph
{
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

            for(int i = 0; i < g.VertexesCount - 1 ; i++)
            {
                foreach(var kv in g.edges)
                {
                    foreach (var (vertex, weight) in kv.Value)
                    {
                        res[vertex] = Math.Min(res[vertex], res[kv.Key] + g[ kv.Key, vertex]);
                    }
                }
            }

            return res;
        }
    }

    
}
