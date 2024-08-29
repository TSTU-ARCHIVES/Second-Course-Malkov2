using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GraphLABS.Models;
using View;

namespace GraphLABS.Controllers;
public class GraphController(Graph graph, ViewForm view)
{
    private readonly Dictionary<Point, Vertex> locations  = [];
    private readonly Dictionary<Vertex, Point> circles    = [];
    private readonly Graphics                  g          = view.CreateGraphics();
    private readonly Pen                       defaultPen = new(Color.SkyBlue, 2);
    private readonly Pen                       grayPen    = new(Color.Gray, 2);
    private readonly Pen                       whitePen   = new(Color.SkyBlue, 2);
    private readonly Pen                       blackPen   = new(Color.Black, 2);
    private readonly Font                      font       = new("Times New Roman", 16);
    private readonly Brush                     brush      = new SolidBrush(Color.Red);
    private const    int                       radius     = 25;

    public void AddVertex(Point location)
    {
        if (locations.ContainsKey(location))
            return;

        var newVertex = new Vertex(locations.Count);
        locations.Add(location, newVertex);
        circles.Add(newVertex, location);

        graph.AddVertex(newVertex);

        g.DrawEllipse(defaultPen, location.X - radius,
                           location.Y - radius,
                           2 * radius, 2 * radius);
        g.DrawString(newVertex.ToString(), font, brush,
            location.X - radius + font.Height / 2,
            location.Y - radius + font.Height / 2);
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

        if (graph.AddEdge((Vertex)srcVertex, (Vertex)destVertex))
        {
            g.DrawArrow(from, to, defaultPen);
        }


    }


    private static bool InRadius(Point center, Point point) =>
        Distance(center, point) <= radius;

    private static double Distance(Point a, Point b) =>
        Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));

    public void DFS()
    {
        InitEvents();
        graph.TraverseInDepth(locations.First().Value);
        Redraw();
    }


    public void BFS()
    {
        InitEvents();
        graph.TraverseInBreadth(locations.First().Value);
        Redraw();
    }

    public string GraphString() => graph.ToString();

    void InitEvents()
    {
        graph.OnBlackVisit +=  (o, e) =>
        {
            var center = circles[e.Vertex];
            g.DrawEllipse(blackPen, center.X - radius,
                           center.Y - radius,
                           2 * radius, 2 * radius);
            Thread.Sleep(1000);
        };
        graph.OnWhiteVisit += (o, e) =>
        {
            var center = circles[e.Vertex];
            g.DrawEllipse(whitePen, center.X - radius,
                           center.Y - radius,
                           2 * radius, 2 * radius);
            Thread.Sleep(1000);

        };
        graph.OnGreyVisit += (o, e) =>
        {
            var center = circles[e.Vertex];
            g.DrawEllipse(grayPen, center.X - radius,
                           center.Y - radius,
                           2 * radius, 2 * radius);
            Thread.Sleep(1000);
        };
    }

    void Redraw()
    {
        foreach(var pair in locations)
        {
            var point = pair.Key;
            g.DrawEllipse(defaultPen, point.X - radius,
                           point.Y - radius,
                           2 * radius, 2 * radius);
        }
        GC.Collect(0);
    }

}
