using GraphLABS.Controllers;

namespace View;

public partial class ViewForm : Form
{
    GraphController gc;

    private Point? startPoint;
    private Point? endPoint;
    public ViewForm()
    {
        InitializeComponent();
        gc = new(new(), this);
    }

    private void ViewForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
            gc.AddVertex(e.Location);
        else if (e.Button == MouseButtons.Left)
            startPoint = e.Location;
    }

    private void ViewForm_MouseMove(object sender, MouseEventArgs e)
    {

    }

    private void ViewForm_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            endPoint = e.Location;
            gc.AddEdge((Point)startPoint, (Point)endPoint);
            startPoint = null;
            endPoint = null;
        }
    }

    private void ViewForm_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

    private void DFS_button_Click(object sender, EventArgs e)
    {
        gc.DFS();
    }

    private void BFS_button_Click(object sender, EventArgs e)
    {
        gc.BFS();
    }
}
