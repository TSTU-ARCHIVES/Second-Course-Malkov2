namespace P;

enum Colors
{
    WHITE, GRAY, BLACK
}
public class GraphOnMatrixADJ
{

    protected int[,] MatrixADJ { get; private set; }
    protected readonly int Vertexes;

    public event EventHandler<GraphEventArgs> GreyVisit;
    public GraphOnMatrixADJ(int[,] matrixADJ)
    {
        this.MatrixADJ = matrixADJ;
        Vertexes = matrixADJ.GetLength(0);
    }
    public GraphOnMatrixADJ()
    {
        MatrixADJ = new int[1, 1];
        Vertexes = 1;
    }
    public GraphOnMatrixADJ(int N)
    {
        MatrixADJ = new int[N, N];
        Vertexes = N;
    }
    public bool IsCycled()
    {
        Colors[] vertexes = new Colors[Vertexes];
        for (int i = 0; i < Vertexes; i++)
        {

            if (DFS(i, vertexes))
                return true;
        }
        return false;
    }

    void OnGreyVisit(int v) => GreyVisit.Invoke(this, new GraphEventArgs(v));

    private bool DFS(int v, Colors[] vertexes)
    {
        vertexes[v] = Colors.GRAY;
        foreach(var adjVertex in AdjastentVertexes(v))
        {
            if (vertexes[adjVertex] == Colors.WHITE)
                return DFS(adjVertex, vertexes);
            if (vertexes[adjVertex] == Colors.GRAY)
            {
                OnGreyVisit(adjVertex);
                return true;
            }
        }
        vertexes[v] = Colors.BLACK;
        return false;
    }

    private IEnumerable<int> AdjastentVertexes(int v)
    {
        for(int i = 0; i < Vertexes; i++)
        {
            if (Adjastent(v, i))
                yield return i;
        }
        yield break;
    }

    private bool Adjastent(int v, int i) => MatrixADJ[v, i] != 0;

    
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
        int[,] uncycledGraph =
        {
            {0, 1, 0, 0, 0},
            {0, 0, 1, 1, 0},
            {0, 0, 0, 0, 0},
            {0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0}
        };

        var graph = new GraphOnMatrixADJ(uncycledGraph);
        graph.GreyVisit += (o, e) => Console.WriteLine("цикл в графе 1 начинается в вершине " + e.Vertex);

        Console.WriteLine("1 граф имеет цикл? " + graph.IsCycled());

        int[,] cycledGraph =
        {
            {0, 1, 0, 0, 0},
            {0, 0, 1, 1, 0},
            {1, 0, 0, 0, 0},
            {0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0}
        };

        var cgraph = new GraphOnMatrixADJ(cycledGraph);
        cgraph.GreyVisit += (o, e) => Console.WriteLine("цикл в графе 2 начинается в вершине " + e.Vertex);

        Console.WriteLine("2 граф имеет цикл? " + cgraph.IsCycled());
    }
}