using System.Numerics;

namespace GraphLABS.Models
{
    public record struct Edge(Vertex Source, Vertex Destination, int Weigth);
}
