namespace GraphLABS.Models;
public partial class Graph
{
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
