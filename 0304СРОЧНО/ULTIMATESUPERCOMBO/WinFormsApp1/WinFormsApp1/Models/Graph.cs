using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Models
{
    public class Graph
    {
        public List<Vertex> Vertexes;
        public Dictionary<Vertex, List<Vertex>> Edges;

        public Graph()
        {
            Vertexes = new List<Vertex>();
            Edges = new Dictionary<Vertex, List<Vertex>>();
        }

        public void AddVertex(Vertex vertex)
        {
            if (!Vertexes.Contains(vertex))
            {
                Vertexes.Add(vertex);
                Edges[vertex] = new List<Vertex>();
            }
        }

        public void AddEdge(Vertex source, Vertex destination)
        {
            if (Vertexes.Contains(source) && Vertexes.Contains(destination))
            {
                if (!Edges[source].Contains(destination))
                {
                    Edges[source].Add(destination);
                }
            }
        }


        public void AddEdge(int source, int destination)
        {
            if (Vertexes.Select(x => x.Number).Contains(source) && Vertexes.Select(x => x.Number).Contains(destination))
            {
                if (!Edges.Keys.Select(x => x.Number).Contains(destination))
                {
                    Edges[source].Add(destination);
                }
            }
        }

        public List<Vertex> GetAdjacentVertices(Vertex vertex)
        {
            if (Vertexes.Contains(vertex))
            {
                return Edges[vertex];
            }
            return new List<Vertex>();
        }
    }
}
