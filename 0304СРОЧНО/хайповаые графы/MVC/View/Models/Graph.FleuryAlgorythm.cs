namespace GraphLABS.Models;
public partial class Graph
{
    internal static class FleuryAlgorythm
    {
        internal static List<Vertex> FindEulerianCycle(Graph graph, Vertex start)
        {
            var tempGraph = new Dictionary<Vertex, List<Vertex>>();
            foreach(var kv in graph.edges)
            {
                tempGraph.Add(kv.Key, kv.Value.Select(x => x.vertex).ToList());
            }
            var stack = new Stack<Vertex>();
            var cycle = new List<Vertex>();

            stack.Push(start);

            while (stack.Count > 0)
            {
                var currentVertex = stack.Peek();

                if (tempGraph[currentVertex].Count == 0)
                {
                    cycle.Add(stack.Pop());
                }
                else
                {
                    var nextVertex = tempGraph[currentVertex][0];
                    stack.Push(nextVertex);
                    tempGraph[currentVertex].Remove(nextVertex);
                    tempGraph[nextVertex].Remove(currentVertex);
                }
            }

            cycle.Reverse();

            return cycle;
        }
    }

    
}
