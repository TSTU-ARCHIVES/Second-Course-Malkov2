namespace WinFormsApp1.Models
{
    public class Vertex
    {
        public int Number { get; }
        public Point Position { get; }

        public Vertex(int number, Point position)
        {
            Number = number;
            Position = position;
        }

        // Дополнительные свойства и методы, если необходимо
    }
}
