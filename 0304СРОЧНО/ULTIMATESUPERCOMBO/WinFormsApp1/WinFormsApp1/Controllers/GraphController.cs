using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Models;

namespace WinFormsApp1.Controllers
{
    public class GraphController
    {
        private Graph graph;
        private GraphForm graphForm;

        public GraphController(GraphForm form)
        {
            graph = new Graph();
            graphForm = form;
        }

        public void AddEdge(int source, int destination)
        {
            graph.AddEdge(source, destination);
            Console.WriteLine($"Edge between {source} and {destination} added successfully");
        }


        public void AddVertexAtPosition(Point position)
        {
            int vertexNumber = graph.Vertexes.Count + 1;
            Vertex newVertex = new Vertex(vertexNumber, position);
            graph.AddVertex(newVertex);

            VertexControl vertexControl = new VertexControl(vertexNumber, position);
            graphForm.Controls.Add(vertexControl);
        }

    }
}
