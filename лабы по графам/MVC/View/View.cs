namespace View;
using GraphLABS.Controllers;

public partial class ViewForm : Form
{
    GraphController controller;

    private Point? startPoint;
    private Point? endPoint;
    public ViewForm()
    {
        InitializeComponent();
        controller = new(new(), this);
    }

    private void DFS_button_Click(object sender, EventArgs e)
    {
        controller.DFS();
    }

    private void BFS_button_Click(object sender, EventArgs e)
    {
        controller.BFS();
    }
    private void ViewForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            controller.AddVertex(e.Location);
            graph_string.Text = controller.GraphString();
        }
        startPoint = e.Location;
    }

    private void ViewForm_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            endPoint = e.Location;
            if (endPoint.Equals(startPoint))
            {
                controller.SelectVertex1(e.Location);
                startPoint = null;
                endPoint = null;
                return;
            }
            controller.AddEdge((Point)startPoint, (Point)endPoint);
            startPoint = null;
            endPoint = null;
            graph_string.Text = controller.GraphString();
        }
        if (e.Button == MouseButtons.Middle)
        {
            endPoint = e.Location;
            if (endPoint.Equals(startPoint))
            {
                controller.SelectVertex2(e.Location);
                startPoint = null;
                endPoint = null;
                return;
            }
            controller.AddEdge((Point)startPoint, (Point)endPoint);
            controller.AddEdge((Point)endPoint, (Point)startPoint);
            startPoint = null;
            endPoint = null;
            graph_string.Text = controller.GraphString();
        }
    }

    private void DFS_cycle_button_Click(object sender, EventArgs e)
    {
        var cycle = controller.FindCycle();
        MessageBox.Show(cycle);
    }

    private void RobertsFlores_button_Click(object sender, EventArgs e)
    {
        var found = controller.RobertsFlores(out var stringPath);
        if (!found)
            MessageBox.Show("Path not found");
        else
            MessageBox.Show("path: " + stringPath);

        controller.Redraw();
    }

    private void Clear_button_Click(object sender, EventArgs e)
    {
        controller.Clear();
        graph_string.Text = "";
    }

    private void button1_Click(object sender, EventArgs e)
    {
        var found = controller.Kruscal(out var stringPath);
        if (!found)
            MessageBox.Show("Path not found");
        else
            MessageBox.Show("path: " + stringPath);

        controller.Redraw();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        var found = controller.Prim(out var stringPath);
        if (!found)
            MessageBox.Show("Path not found");
        else
            MessageBox.Show("path: " + stringPath);

        controller.Redraw();

    }

    private void button3_Click(object sender, EventArgs e)
    {
        var found = controller.Fleury(out var stringPath);
        if (!found)
            MessageBox.Show("Path not found");
        else
            MessageBox.Show("path: " + stringPath);

        controller.Redraw();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        MessageBox.Show("distances: \n" + controller.Dijkstra());
        startPoint = null;
        endPoint = null;
        controller.Redraw();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        MessageBox.Show("distances matrix: \n" + controller.FloydWarshall());
        startPoint = null;
        endPoint = null;
        controller.Redraw();
    }

    private void button6_Click(object sender, EventArgs e)
    {
        MessageBox.Show("distances: \n" + controller.Ford());
        startPoint = null;
        endPoint = null;
        controller.Redraw();
    }

    private void button7_Click(object sender, EventArgs e)
    {
        MessageBox.Show(controller.IsTree());
    }
}
