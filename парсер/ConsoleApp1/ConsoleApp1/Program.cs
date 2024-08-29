using ClassLibrary1;
using System.Xml.Linq;

namespace ConsoleApp1;

class Program
{
    static void PrintParsingTree(Node<Token> node, string indent = "", bool last = true)
    {
        if (node == null) return;
        
        Console.Write(indent);
        if (last)
        {
            Console.Write("└─");
            indent += "  ";
        }
        else 
        {
            Console.Write("├─");
            indent += "| ";
        }

        Console.WriteLine(node.Value.TokenString);

        PrintParsingTree(node.Left, indent, node.Right == null);
        PrintParsingTree(node.Right, indent, true);
    }
    public static void Main(string[] args)
    {
        var expr = new Expression("sin( cos( x + 5) - exp(pi) * 8) / 4");
        var variables = new Dictionary<char, double>()
        {
            {'x', 1 },
            {'y', 1 },
            {'λ', 2 }
        };
        var value = expr.CalculateAt(variables, out var node);
        Console.WriteLine(value);
        PrintParsingTree(node);
    }
}