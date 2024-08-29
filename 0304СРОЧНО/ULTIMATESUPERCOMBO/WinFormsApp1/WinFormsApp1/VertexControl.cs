namespace WinFormsApp1
{
    public class VertexControl : Control
    {
        public int VertexNumber { get; private set; }

        public VertexControl(int vertexNumber, Point location)
        {
            VertexNumber = vertexNumber;
            this.Location = location;
            this.Size = new Size(30, 30);
            this.BackColor = Color.Blue;
            this.MouseDown += VertexControl_MouseDown;
            this.MouseMove += VertexControl_MouseMove;
            this.MouseUp += VertexControl_MouseUp;
        }

        private void VertexControl_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move);
        }

        private void VertexControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.DoDragDrop(this, DragDropEffects.Move);
            }
        }

        private void VertexControl_MouseUp(object sender, MouseEventArgs e)
        {
            // Обработка события MouseUp
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillEllipse(Brushes.Blue, 0, 0, this.Width - 1, this.Height - 1);
            e.Graphics.DrawString(VertexNumber.ToString(), SystemFonts.DefaultFont, Brushes.White, 10, 10);
        }
    }
}
