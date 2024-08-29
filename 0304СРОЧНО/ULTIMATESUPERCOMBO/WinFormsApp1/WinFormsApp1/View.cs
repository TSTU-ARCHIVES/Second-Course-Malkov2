using WinFormsApp1.Controllers;

namespace WinFormsApp1
{
    public partial class GraphForm : Form
    {
        private GraphController graphController;
        private bool isDragging = false;
        private VertexControl draggingVertex;

        public GraphForm()
        {
            graphController = new GraphController(this);
            this.MouseDown += GraphForm_MouseDown;
        }

        private void GraphForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                graphController.AddVertexAtPosition(e.Location);
                UpdateGraphInfo();
            }
        }

        private void Vertex_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draggingVertex = (VertexControl)sender;
                isDragging = true;
            }
        }

        private void Vertex_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggingVertex != null)
            {
                draggingVertex.Location = this.PointToClient(Cursor.Position);
            }
        }

        private void Vertex_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging && draggingVertex != null)
            {
                isDragging = false;
                int source = draggingVertex.VertexNumber;

                // Найти вершину под курсором мыши
                VertexControl destinationVertex = FindVertexUnderCursor();
                if (destinationVertex != null)
                {
                    int destination = destinationVertex.VertexNumber;
                    graphController.AddEdge(source, destination);
                    UpdateGraphInfo();
                }
            }
        }

        private VertexControl FindVertexUnderCursor()
        {
            foreach (Control control in this.Controls)
            {
                if (control is VertexControl vertex && vertex.Bounds.Contains(this.PointToClient(Cursor.Position)))
                {
                    return vertex;
                }
            }
            return null;
        }

        private void UpdateGraphInfo()
        {
            // Обновить информацию о графе на форме
        }
    }
}
