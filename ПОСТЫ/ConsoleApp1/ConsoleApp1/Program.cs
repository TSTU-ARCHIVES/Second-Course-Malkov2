using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Formats.Asn1;

namespace P;
public class Graph
{

    protected int[,] MatrixADJ { get; set; }
    public int AmountOfVertexes { get; protected set; }

    public event EventHandler<GraphEventArgs> OnEnterVertex;
    public event EventHandler<GraphEventArgs> OnExitVertex;
    public Graph(int[,] matrixADJ)
    {
        this.MatrixADJ = matrixADJ;
        AmountOfVertexes = matrixADJ.GetLength(0);
    }
    public Graph()
    {
        MatrixADJ = new int[1, 1];
        AmountOfVertexes = 1;
    }
    public Graph(int N)
    {
        MatrixADJ = new int[N, N];
        AmountOfVertexes = N;
    }

    public void TraverseInDepth(int start)
        => DFS(start);

    void EnterVertex(int v) => OnEnterVertex?.Invoke(this, new GraphEventArgs(v));
    void ExitVertex(int v) => OnExitVertex?.Invoke(this, new GraphEventArgs(v));


    private void DFS(int v)
    {
        var currentVertex = v;
        var vertexes = new Stack<int>();
        var seen = new bool[AmountOfVertexes];
        seen[currentVertex] = true;
        vertexes.Push(currentVertex);

        while (vertexes.Count > 0)
        {
            currentVertex = vertexes.Peek(); 
            EnterVertex(currentVertex);
            var checkableVertex = FindAdjastentNotSeen(currentVertex, seen);
            if (checkableVertex != -1)
            {
                vertexes.Push(checkableVertex);
                seen[checkableVertex] = true;
            }
            else
            {
                ExitVertex(currentVertex);
                currentVertex = vertexes.Pop();
            }
        }
    }

    private int FindAdjastentNotSeen(int current, bool[] seen)
    {
        foreach(var v in AdjastentVertexes(current))
        {
            if (!seen[v])
                return v;
        }
        return -1;
    }

    public IEnumerable<int> AdjastentVertexes(int v)
    {
        for(var i = 0; i < AmountOfVertexes; i++)
        {
            if (Adjastent(v, i))
                yield return i;
        }
        yield break;
    }

    public bool Adjastent(int v, int i) => MatrixADJ[v, i] != 0;

    public List<int> Posts(int[] postsCost, out int value)
    {
        var globalSeen = new bool[AmountOfVertexes];
        var vertexes = new Stack<int>();
        var components = new List<List<int>>();
        value = 0;
        var strongComponents = new TarjanAlgorithm().FindStronglyConnectedComponents(this);
        for (var v = 0; v < AmountOfVertexes; v++)
        {
            if (globalSeen[v])
                continue;

            vertexes.Clear();
            var seen = new bool[AmountOfVertexes];
            var component = new List<int>();
            var currentVertex = v;
            vertexes.Push(currentVertex);

            while (vertexes.Count > 0)
            {
                currentVertex = vertexes.Peek();
                component.Add(currentVertex);
                seen[currentVertex] = true;
                globalSeen[currentVertex] = true;
                var checkableVertex = FindAdjastentNotSeen(currentVertex, seen);
                if (checkableVertex != -1)
                {
                    vertexes.Push(checkableVertex);
                }
                else
                {
                    currentVertex = vertexes.Pop();
                }
            }

            components.Add(component);
        }
        var res = new List<int>();
        //есть компоненты
        //в них есть вершины
        //мы берем самую дешевую вершину, находящуюся в  той же
        //сильно связной компоненте, что и нулевая (ну либо же ее саму)
        foreach (var component in components)
        {
            var minComponent = strongComponents
                .FirstOrDefault(x => x.Contains(component[0]));
            var minVertex = minComponent
                .MinBy(x => postsCost[x]);
            value += postsCost[minVertex];
            res.Add(minVertex);
        }
        return res;
    }

    class TarjanAlgorithm
    {
        private int index = 0;
        private Stack<int> stack = new();
        private List<List<int>> strongComponents = [];
        private bool[] onStack;
        private int[] indexArr, lowlink;

        public List<List<int>> FindStronglyConnectedComponents(Graph graph)
        {
            onStack = new bool[graph.AmountOfVertexes];
            indexArr = new int[graph.AmountOfVertexes];
            lowlink = new int[graph.AmountOfVertexes];

            for (int v = 0; v < graph.AmountOfVertexes; v++)
            {
                if (indexArr[v] == 0)
                {
                    StrongConnect(graph, v);
                }
            }

            return strongComponents;
        }

        private void StrongConnect(Graph graph, int v)
        {
            indexArr[v] = index;
            lowlink[v] = index;
            index++;
            stack.Push(v);
            onStack[v] = true;

            for (int w = 0; w < graph.AmountOfVertexes; w++)
            {
                if (graph.Adjastent(v, w))
                {
                    if (indexArr[w] == 0)
                    {
                        StrongConnect(graph, w);
                        lowlink[v] = Math.Min(lowlink[v], lowlink[w]);
                    }
                    else if (onStack[w])
                    {
                        lowlink[v] = Math.Min(lowlink[v], indexArr[w]);
                    }
                }
            }

            if (lowlink[v] == indexArr[v])
            {
                List<int> component = [];
                int w;
                do
                {
                    w = stack.Pop();
                    onStack[w] = false;
                    component.Add(w);
                } 
                while (w != v);

                strongComponents.Add(component);
            }
        }
    }
}


public class GraphEventArgs : EventArgs
{
    public int Vertex { get; set;}
    public GraphEventArgs(int vertex) { Vertex = vertex; }
}

class Program
{
    public static void Main(string[] args)
    {
        //int[,] uncycledGraph =
        //{
        //    {0, 1, 0, 0, 0},
        //    {0, 0, 1, 1, 0},
        //    {0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 1},
        //    {0, 0, 0, 0, 0}
        //};

        //var graph = new GraphOnMatrixADJ(uncycledGraph);
        //graph.GreyVisit += (o, e) => Console.WriteLine("цикл в графе 1 начинается в вершине " + e.Vertex);

        //Console.WriteLine("1 граф имеет цикл? " + graph.IsCycled());

        //int[,] cycledGraph =
        //{
        //    {0, 1, 0, 0, 0},
        //    {0, 0, 1, 1, 0},
        //    {1, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 1},
        //    {0, 0, 0, 0, 0}
        //};

        //var cgraph = new GraphOnMatrixADJ(cycledGraph);
        //cgraph.GreyVisit += (o, e) => Console.WriteLine("цикл в графе 2 начинается в вершине " + e.Vertex);

        //Console.WriteLine("2 граф имеет цикл? " + cgraph.IsCycled());

        int[,] labyrinth =
        {
            {0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            {0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }
        };

        var graph = new Graph(labyrinth);
        var path = graph.Posts([10, 10 , 10, 10, 5, 10, 10, 10, 10, 10, 10], out int val);//graph.Posts([1, 2, 3, 4, 4, 6, 1, 1, 1, 1, 1]);

        Console.WriteLine(path.Aggregate("", (cur, v) => cur + " " + v));
        Console.WriteLine(val);
    }
}