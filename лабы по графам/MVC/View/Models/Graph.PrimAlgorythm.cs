namespace GraphLABS.Models;
public partial class Graph
{
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
                var list = new List<Edge>();
                foreach (var current in ourComponent)
                {
                    list.AddRange(g.edges[current].Select(x => new Edge(current, x.vertex, x.weight)));
                }

                var sorted = list.OrderBy(e => e.Weigth);
                foreach (var (source, dest, weight) in sorted)
                {
                    if (!ourComponent.Contains(dest))
                    {
                        ourComponent.Add(dest);
                        res.Add(new(source, dest, weight));
                        break;
                    }
                }
                list.Clear();
            }
            return res;
        }
    }

    
}
