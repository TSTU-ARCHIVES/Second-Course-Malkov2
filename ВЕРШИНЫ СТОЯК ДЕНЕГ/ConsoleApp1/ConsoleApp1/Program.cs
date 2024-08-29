using System.Collections.Generic;
using System.Formats.Asn1;

namespace P;
public class GraphOnMatrixADJ
{

    protected int[,] MatrixADJ { get; set; }
    public int AmountOfVertexes { get; protected set; }

    public event EventHandler<GraphEventArgs> OnEnterVertex;
    public event EventHandler<GraphEventArgs> OnExitVertex;
    public GraphOnMatrixADJ(int[,] matrixADJ)
    {
        this.MatrixADJ = matrixADJ;
        AmountOfVertexes = matrixADJ.GetLength(0);
    }
    public GraphOnMatrixADJ()
    {
        MatrixADJ = new int[1, 1];
        AmountOfVertexes = 1;
    }
    public GraphOnMatrixADJ(int N)
    {
        MatrixADJ = new int[N, N];
        AmountOfVertexes = N;
    }

    public void TraverseInDepth(int start)
        => DFS(start);

    void EnterVertex(int v) => OnEnterVertex?.Invoke(this, new GraphEventArgs(v));
    void ExitVertex(int v) => OnExitVertex?.Invoke(this, new GraphEventArgs(v));

    public List<int> Path(int targetCost, int[] costs)
    {
        for (var v = 0; v < AmountOfVertexes; v++)
        {
            var sum = 0;
            var currentVertex = v;
            var vertexes = new Stack<int>();
            var seen = new bool[AmountOfVertexes];
            seen[currentVertex] = true;
            vertexes.Push(currentVertex);

            while (vertexes.Count > 0)
            {
                currentVertex = vertexes.Peek();
                sum += costs[currentVertex];
                if (sum == targetCost)
                {
                    return [.. vertexes.Reverse()];
                }
                var checkableVertex = FindAdjastentNotSeen(currentVertex, seen);
                if (checkableVertex != -1)
                {
                    vertexes.Push(checkableVertex);
                    seen[checkableVertex] = true;
                }
                else
                {
                    currentVertex = vertexes.Pop();
                    sum -= costs[currentVertex];
                }
            }
        }
        return [];
    }

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
            {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            {1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            {0, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0 },
            {0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0 },
            {0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0 },
            {0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
            {0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0 },
            {0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1 },
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1 }
        };

        var graph = new GraphOnMatrixADJ(labyrinth);
        var path = graph.Path(5, [1, 2, 3, 4, 4, 6, 1, 1, 1, 1, 1]);
        Console.WriteLine(path.Aggregate("", (cur, v) => cur + " " + v));
    }
}