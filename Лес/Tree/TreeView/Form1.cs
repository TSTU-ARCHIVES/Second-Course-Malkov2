namespace TreeView;

public partial class Form1 : Form
{
    TreeController tc;
    Point? start = null, end = null;
    public Form1()
    {
        InitializeComponent();
        tc = new(this);
    }

    private void Form1_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            tc.AddNode(e.Location);
            return;
        }
        if (e.Button == MouseButtons.Left)
            start = e.Location;
    }

    private void Form1_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            end = e.Location;
            if (!end.Equals(start))
            {
                tc.AddEdge((Point)start, (Point)end);
            }
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        tc.InOrder();
        tc.Redraw();
    }

    private void button3_Click(object sender, EventArgs e)
    {

        tc.InIverseOrder();
        tc.Redraw();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        tc.InSymmetricOrder();
        tc.Redraw();
    }

    private void button6_Click(object sender, EventArgs e)
    {
        tc.InOrderStack();
        tc.Redraw();
    }

    private void button5_Click(object sender, EventArgs e)
    {
        tc.InSymmetricOrderStack();
        tc.Redraw();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        tc.InIverseOrderStack();
        tc.Redraw();
    }
}
