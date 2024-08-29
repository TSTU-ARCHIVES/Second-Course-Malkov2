using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1;
public static class Polish
{
    
    public static Token[] ToInversePolishView(Token[] expression)
    {
        var output = new List<Token>();
        var operators = new Stack<Token>();

        foreach (var token in expression)
        {
            if (token.IsNumber())
            {
                output.Add(token);
            }
            else if (
                token.Type == Token.TYPE.BINARY_OPERATOR ||
                token.Type == Token.TYPE.FUNCTION 
                )
            {
                while (operators.Count > 0 && 
                    token.Precendency <= operators.Peek().Precendency)
                {
                    output.Add(operators.Pop());
                }
                operators.Push(token);
            }
            else if (token.Type == Token.TYPE.L_BRACE)
            {
                operators.Push(token);
            }
            else if (token.Type == Token.TYPE.R_BRACE)
            {
                while (operators.Count > 0 && operators.Peek().Type != Token.TYPE.L_BRACE)
                {
                    output.Add(operators.Pop());
                }
                operators.TryPop(out _);
            }
        }

        while (operators.Count > 0)
        {
            output.Add(operators.Pop());
        }

        return output.ToArray();

    }
}
