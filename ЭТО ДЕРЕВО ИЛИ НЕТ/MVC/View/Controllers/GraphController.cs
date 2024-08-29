using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using GraphLABS.Models;
using View;

namespace GraphLABS.Controllers;
public class GraphController(Graph graph, ViewForm view)
{
    private readonly Dictionary<Point, Vertex> locations  = [];
    private readonly Dictionary<Vertex, Point> circles    = [];
    private readonly Dictionary<Edge, (Point from, Point to)> arrows = [];
    private readonly Graphics                  g          = view.CreateGraphics();
    private readonly Pen                       defaultPen = new(Color.SkyBlue, 2);
    private readonly Pen                       grayPen    = new(Color.Gray, 2);
    private readonly Pen                       whitePen   = new(Color.SkyBlue, 2);
    private readonly Pen                       blackPen   = new(Color.Black, 2);
    private readonly Pen                       selectionPen = new(Color.DarkRed, 2);
    private readonly Pen                       selectionPen2 = new(Color.Coral, 2);
    private readonly Font                      font       = new("Times New Roman", 16);
    private readonly Brush                     brush      = new SolidBrush(Color.Red);
    private const    int                       RADIUS     = 25;
    private const    int                       DELAY      = 500;

    private Vertex? selectedVertex1;
    private Vertex? selectedVertex2;

    public void SelectVertex1(Point location)
    {
        foreach (var point in locations.Keys)
        {
            if (InRadius(location, point))
            {
                Point circle;
                if (selectedVertex1 != null)
                {
                    circle = circles[(Vertex)selectedVertex1];
                    g.DrawEllipse(defaultPen, circle.X - RADIUS,
                               circle.Y - RADIUS,
                               2 * RADIUS, 2 * RADIUS);
                }
                selectedVertex1 = locations[point];
                circle = circles[(Vertex)selectedVertex1];
                g.DrawEllipse(selectionPen, circle.X - RADIUS,
                           circle.Y - RADIUS,
                           2 * RADIUS, 2 * RADIUS);
                break;
            }
        }
    }

    public void SelectVertex2(Point location)
    {
        foreach (var point in locations.Keys)
        {
            if (InRadius(location, point))
            {
                Point circle;
                if (selectedVertex2 != null)
                {
                    circle = circles[(Vertex)selectedVertex1];
                    g.DrawEllipse(defaultPen, circle.X - RADIUS,
                               circle.Y - RADIUS,
                               2 * RADIUS, 2 * RADIUS);
                }
                selectedVertex2 = locations[point];
                circle = circles[(Vertex)selectedVertex2];
                g.DrawEllipse(selectionPen2, circle.X - RADIUS,
                           circle.Y - RADIUS,
                           2 * RADIUS, 2 * RADIUS);
                break;
            }
        }
    }

    public void AddVertex(Point location)
    {
        if (locations.ContainsKey(location))
            return;

        var newVertex = new Vertex(locations.Count);
        locations.Add(location, newVertex);
        circles.Add(newVertex, location);

        graph.AddVertex(newVertex);

        g.DrawEllipse(defaultPen, location.X - RADIUS,
                           location.Y - RADIUS,
                           2 * RADIUS, 2 * RADIUS);
        g.DrawString(newVertex.ToString(), font, brush,
            location.X - RADIUS + font.Height / 2,
            location.Y - RADIUS + font.Height / 2);
    }

    public void AddEdge(Point from, Point to)
    {

        Vertex? srcVertex = null, destVertex = null;
        foreach(var point in locations.Keys)
        {
            if (InRadius(from, point))
            {
                srcVertex = locations[point];
                break;
            }
        }
        foreach (var point in locations.Keys)
        {
            if (InRadius(to, point))
            {
                destVertex = locations[point];
                break;
            }
        }
        if (srcVertex == null || destVertex == null)
            return;

        if (graph.AddEdge((Vertex)srcVertex, (Vertex)destVertex, (int)Distance(to, from)))
        {
            g.DrawArrow(from, to, defaultPen);
            this.arrows.TryAdd(new Edge((Vertex)srcVertex, (Vertex)destVertex, (int)Distance(to, from)), (from, to));
        }


    }


    private static bool InRadius(Point center, Point point) =>
        Distance(center, point) <= RADIUS;

    private static double Distance(Point a, Point b) =>
        Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

    public void DFS()
    {
        if (locations.Count <= 0)
        {
            return;
        }
        InitEvents();
        graph.TraverseInDepth();
        Redraw();
        DeInitEvents();
    }


    public void BFS()
    {
        if (locations.Count <= 0)
        {
            return;
        }
        InitEvents();
        graph.TraverseInBreadth(locations.First().Value);
        Redraw();
        DeInitEvents();
    }

    public string GraphString() => graph.ToString();

    string cycle = "cycle not found";
    public string FindCycle()
    {
        if (locations.Count <= 0)
        {
            return "no vertexes";
        }
        graph.OnCycleDFS += CycleCheck;
        graph.TraverseInDepth();
        graph.OnCycleDFS -= CycleCheck;
        return cycle;
    }

    public bool RobertsFlores(out string? path)
    {
        if (locations.Count <= 0)
        {
            path = "no vertexes";
            return false;
        }
        var foundPath = graph.HamiltonianPath(locations.First().Value);

        if (foundPath.Count == 0)
        {
            path = default;
            return false;
        }

        for (int i = 0; i < foundPath.Count - 1; i++)
        {
            foreach(var edge in arrows.Keys)
            {
                if (edge.Source.Equals(foundPath[i]) && edge.Destination.Equals(foundPath[i + 1]))
                {
                    var (from, to) = arrows[edge];
                    g.DrawArrow(from, to, blackPen);
                }
            }
        }
        foreach (var edge in arrows.Keys)
        {
            if (edge.Source.Equals(foundPath[^1]) && edge.Destination.Equals(foundPath[0]))
            {
                var (from, to) = arrows[edge];
                g.DrawArrow(from, to, blackPen);
            }
        }
        path = foundPath
        .Aggregate(" ", (a, b) => a + " " + b)
        .ToString();
        selectedVertex1 = null;
        selectedVertex2 = null;
        return true;
    }

    public bool Fleury(out string? path)
    {
        if (locations.Count <= 0)
        {
            path = "no vertexes";
            return false;
        }
        var foundPath = graph.EulerPath(locations.First().Value);
        if (foundPath.Count == 0)
        {
            path = default;
            return false;
        }

        for (int i = 0; i < foundPath.Count - 1; i++)
        {
            foreach (var edge in arrows.Keys)
            {
                if (edge.Source.Equals(foundPath[i]) && edge.Destination.Equals(foundPath[i + 1]))
                {
                    var (from, to) = arrows[edge];
                    g.DrawArrow(from, to, blackPen);
                }
            }
        }
        foreach (var edge in arrows.Keys)
        {
            if (edge.Source.Equals(foundPath[^1]) && edge.Destination.Equals(foundPath[0]))
            {
                var (from, to) = arrows[edge];
                g.DrawArrow(from, to, blackPen);
            }
        }
        path = foundPath
        .Aggregate(" ", (a, b) => a + " " + b)
        .ToString();
        selectedVertex1 = null;
        selectedVertex2 = null;
        return true;
    }

    public bool Kruscal(out string? vertexes)
    {
        if (locations.Count <= 0)
        {
            vertexes = "no vertexes";
            return false;
        }
        var foundPath = graph.Kruscal(locations.First().Value);
        if (foundPath.Count == 0)
        {
            vertexes = default;
            return false;
        }
        foreach (var edge in foundPath)
        {
            var (from, to) = arrows[edge];
            g.DrawArrow(from, to, blackPen);
        }
        vertexes = foundPath
        .Aggregate(" ", (a, b) => a + " " + b + "\n")
        .ToString();
        selectedVertex1 = null;
        selectedVertex2 = null;
        return true;
    }

    public bool Prim(out string? vertexes)
    {
        if (locations.Count <= 0)
        {
            vertexes = "no vertexes";
            return false;
        }
        var foundPath = graph.Prim(locations.First().Value);
        if (foundPath.Count == 0)
        {
            vertexes = default;
            return false;
        }
        foreach (var edge in foundPath)
        {
            var (from, to) = arrows[edge];
            g.DrawArrow(from, to, blackPen);
        }
        vertexes = foundPath
        .Aggregate(" ", (a, b) => a + " " + b + "\n")
        .ToString();
        selectedVertex1 = null;
        selectedVertex2 = null;
        return true;
    }

    public string Dijkstra()
    {
        if (locations.Count <= 0)
        {
            return "no vertexes";
        }
        if (selectedVertex1 == null)
            return "no vertex selected";
        string foundPath;
        if (selectedVertex2 == null)
        {
            foundPath = graph.Dijkstra((Vertex)selectedVertex1)
                .Aggregate("", (kv1, kv2) => kv1.ToString() + " " + kv2.ToString() + '\n');
        }
        else
        {
            var pathes = graph.Dijkstra((Vertex)selectedVertex1, (Vertex)selectedVertex2);
            foundPath = pathes
                .Aggregate("", (kv1, kv2) => 
                kv1 + " " + 
                kv2.Value.Aggregate($"to {kv2.Key}: ", (v1, v2) => v1 + " " + v2)
                + '\n');
            foreach (var edge in pathes[(Vertex)selectedVertex2])
            {
                var (from, to) = arrows[edge];
                g.DrawArrow(from, to, blackPen);
            }

        }
        selectedVertex1 = null;
        selectedVertex2 = null;
        return foundPath.ToString();

    }

    public string FloydWarshall()
    {
        if (locations.Count <= 0)
        {
            return "no vertexes";
        }
        var foundPath = graph.Floyd()
            .Aggregate("", 
                (row1, row2) => row1 + " " +
                row2.Value
                    .Aggregate($"from {row2.Key} to:", (kv1, kv2) => kv1.ToString() + " " + kv2.ToString())
            + '\n');
        selectedVertex1 = null;
        selectedVertex2 = null;
        return foundPath.ToString();
    }

    public string Ford()
    {
        if (locations.Count <= 0)
        {
            return "no vertexes";
        }
        if (selectedVertex1 == null)
            return "no vertex selected";
        var foundPath = graph.Ford((Vertex)selectedVertex1)
            .Aggregate("", (kv1, kv2) => kv1.ToString() + " " + kv2.ToString() + '\n');
        selectedVertex1 = null;
        selectedVertex2 = null;
        return foundPath.ToString();
    }

    public string IsTree() =>
        graph.IsTree() ? "graph is a tree" : "graph isn't tree";


    public void Clear()
    {
        locations.Clear();
        circles.Clear();
        arrows.Clear();
        graph.Clear();
        view.Refresh();
        Redraw();
    }

    
    void InitEvents()
    {
        graph.OnBlackVisit += BlackVisit;
        graph.OnWhiteVisit += WhiteVisit;
        graph.OnGreyVisit += GreyVisit;
    }

    void DeInitEvents()
    {
        graph.OnBlackVisit -= BlackVisit;
        graph.OnWhiteVisit -= WhiteVisit;
        graph.OnGreyVisit -= GreyVisit;
    }

    void GreyVisit(object sender, GraphEventArgs e)
    {
        var center = circles[e.Vertex];
        g.DrawEllipse(grayPen, center.X - RADIUS,
                       center.Y - RADIUS,
                       2 * RADIUS, 2 * RADIUS);
        Thread.Sleep(DELAY);
    }


    void WhiteVisit(object sender, GraphEventArgs e)
    {
        var center = circles[e.Vertex];
        g.DrawEllipse(whitePen, center.X - RADIUS,
                       center.Y - RADIUS,
                       2 * RADIUS, 2 * RADIUS);
        Thread.Sleep(DELAY);
    }

    void CycleCheck(object sender, GraphEventArgs e)
    {
        cycle = "cycle starts on vertex " + e.Vertex;
    }

    void BlackVisit(object sender, GraphEventArgs e)
    {
        var center = circles[e.Vertex];
        g.DrawEllipse(blackPen, center.X - RADIUS,
                       center.Y - RADIUS,
                       2 * RADIUS, 2 * RADIUS);
        Thread.Sleep(DELAY);
    }

    public void Redraw()
    {
        DrawCircles(defaultPen);
        DrawArrows(defaultPen);
        GC.Collect(0);
    }

    void DrawCircles(Pen pen)
    {
        foreach (var pair in locations)
        {
            var point = pair.Key;
            g.DrawEllipse(pen, point.X - RADIUS,
                           point.Y - RADIUS,
                           2 * RADIUS, 2 * RADIUS);
        }
    }

    void DrawArrows(Pen pen)
    {
        foreach (var pair in arrows)
        {
            var (from, to) = pair.Value;
            g.DrawArrow(from, to, pen);
        }
    }

}
