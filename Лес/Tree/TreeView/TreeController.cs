using System.Windows.Forms;
using System.Xml.Linq;
using Tree;
namespace TreeView;
public class TreeController(Form1 view)
{
    private int _nodeCount = 0;

    private Dictionary<Point, TreeNode<int>> locations = [];
    private Dictionary<TreeNode<int>, Point> points = [];

    private readonly Graphics g = view.CreateGraphics();
    private readonly Pen defaultPen = new(Color.SkyBlue, 2);
    private readonly Pen grayPen = new(Color.Gray, 2);
    private readonly Pen whitePen = new(Color.SkyBlue, 2);
    private readonly Pen blackPen = new(Color.Black, 2);
    private readonly Pen selectionPen = new(Color.DarkRed, 2);
    private readonly Pen selectionPen2 = new(Color.Coral, 2);
    private readonly Font font = new("Times New Roman", 16);
    private readonly Brush brush = new SolidBrush(Color.Red);
    private const int RADIUS = 25;
    private const int DELAY = 500;

    private NotBinaryTree<int> tree;

    public void AddNode(Point srcCoords)
    {
        var newNode = new TreeNode<int>(_nodeCount);
        locations.Add(srcCoords, newNode);
        points.Add(newNode, srcCoords);
        tree ??= new(newNode);

        Draw(newNode, defaultPen);

        g.DrawString(((char)(_nodeCount + 'A')).ToString(), font, brush,
            srcCoords.X - RADIUS + font.Height / 2,
            srcCoords.Y - RADIUS + font.Height / 2);

        _nodeCount++;
    }

    public void AddEdge(Point src, Point dest)
    {
        TreeNode<int>? srcVertex = null, destVertex = null;
        foreach (var point in locations.Keys)
        {
            if (InRadius(src, point))
            {
                srcVertex = locations[point];
                break;
            }
        }
        foreach (var point in locations.Keys)
        {
            if (InRadius(dest, point))
            {
                destVertex = locations[point];
                break;
            }
        }

        if (srcVertex == null || destVertex == null)
            return;

        srcVertex.AddChild(destVertex);
        g.DrawArrow(src, dest, defaultPen);
    }


    private static bool InRadius(Point center, Point point) =>
        Distance(center, point) <= RADIUS;

    private static double Distance(Point a, Point b) =>
        Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

    public void InOrder()
    {
        tree.InStraightOrderTraverse(TraverseHandler);
    }
    public void InOrderStack()
    {
        tree.InStraightOrderTraverseStack(TraverseHandler);
    }

    public void InIverseOrder()
    {
        tree.InInverseOrderTraverse(TraverseHandler);
    }

    public void InIverseOrderStack()
    {
        tree.InInverseOrderTraverseStack(TraverseHandler);
    }

    public void InSymmetricOrder()
    {
        tree.InSymmetricOrderTraverse(TraverseHandler);
    }

    public void InSymmetricOrderStack()
    {
        tree.InSymmetricOrderTraverseStack(TraverseHandler);
    }

    void Draw(TreeNode<int> node, Pen pen)
    {
        g.DrawEllipse(pen, points[node].X - RADIUS,
                           points[node].Y - RADIUS,
                           2 * RADIUS, 2 * RADIUS);
    }

    void TraverseHandler(TreeNode<int> node)
    {
        Draw(node, selectionPen);
        Thread.Sleep(DELAY);
    }

    public void Redraw()
    {
        foreach(var kv in locations)
        {
            Draw(kv.Value, defaultPen);
        }
    }
}
