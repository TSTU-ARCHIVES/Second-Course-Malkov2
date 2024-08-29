namespace GraphLABS.Models;
public partial class Graph
{
    internal static class FloydWarshallAlgorythm
    {
        internal static Dictionary<Vertex, Dictionary<Vertex, double>> FindShortestPaths(Graph g)
        {
            var res = new Dictionary<Vertex, Dictionary<Vertex, double>>();
            foreach (var kv in g.edges)
            {
                var dict = new Dictionary<Vertex, double>();
                foreach(var nkv in g.edges)
                {
                    dict.Add(nkv.Key, double.PositiveInfinity);
                }
                res.Add(kv.Key, dict);
                foreach(var (vertex, weight) in g.edges[kv.Key])
                {
                    res[kv.Key][vertex] = weight;
                }
                res[kv.Key][kv.Key] = 0;
            }

            foreach(var k in g.edges.Keys)
                foreach(var i in g.edges.Keys)
                    foreach(var j in g.edges.Keys)
                        res[i][j] = Math.Min(res[i][j], res[i][k] + res[k][j]);

            return res;
        }
    }

    
}
