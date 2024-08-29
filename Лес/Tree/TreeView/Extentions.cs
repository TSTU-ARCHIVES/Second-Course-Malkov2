namespace TreeView;

public static class Extentions
{
    public static void DrawArrow(this Graphics g, Point from, Point to, Pen pen)
    {
        g.DrawLine(pen, from, to);
        double angle = Math.Atan2(to.Y - from.Y, to.X - from.X);
        double arrowLength = 10;
        var arrowEnd1 = new Point((int)(to.X - arrowLength * Math.Cos(angle - Math.PI / 6)),
                                    (int)(to.Y - arrowLength * Math.Sin(angle - Math.PI / 6)));
        var arrowEnd2 = new Point((int)(to.X - arrowLength * Math.Cos(angle + Math.PI / 6)),
                                    (int)(to.Y - arrowLength * Math.Sin(angle + Math.PI / 6)));

        g.DrawLine(pen, to, arrowEnd1);
        g.DrawLine(pen, to, arrowEnd2);
    }
}
