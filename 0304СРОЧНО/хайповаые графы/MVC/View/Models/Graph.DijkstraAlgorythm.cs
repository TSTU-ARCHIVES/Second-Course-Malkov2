namespace GraphLABS.Models;
public partial class Graph
{
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

    
}
