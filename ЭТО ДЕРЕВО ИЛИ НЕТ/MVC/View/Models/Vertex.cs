using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphLABS.Models
{
    public record struct Vertex(int Number)
    {
        public override readonly string ToString() => Convert.ToChar('A' + this.Number).ToString();
    }
}
