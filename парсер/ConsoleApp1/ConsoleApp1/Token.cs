using System;

namespace ClassLibrary1;
public class Token
{
    /// <summary>
    /// Типы токенов
    /// </summary>
    public enum TYPE
    {
        BINARY_OPERATOR, // оператор бинарный
        UNARY_OPERATOR, // унарный оператор (унарный минус)
        INT_NUM, // целое число
        FLOAT_NUM, // число с плав запятой
        FUNCTION, // функция
        L_BRACE, //левая скобка
        R_BRACE, //правая скобка
        CONSTANT, 
        VARIABLE // переменная 
    }
    public static readonly Dictionary<string, (TYPE tokenType, int precendency)> tokenMap = new()
         {
            { "+",    (TYPE.BINARY_OPERATOR, 0) },
            { "-",    (TYPE.BINARY_OPERATOR, 0) },
            { "*",    (TYPE.BINARY_OPERATOR, 1) },
            { "/",    (TYPE.BINARY_OPERATOR, 1) },
            { "%",    (TYPE.BINARY_OPERATOR, 2) },
            { "^",    (TYPE.BINARY_OPERATOR, 2) },
            { "sin",  (TYPE.FUNCTION       , 3) },
            { "cos",  (TYPE.FUNCTION       , 3) },
            { "tg",   (TYPE.FUNCTION       , 3) },
            { "ctg",  (TYPE.FUNCTION       , 3) },
            { "exp",  (TYPE.FUNCTION       , 3) },
            { "ln",   (TYPE.FUNCTION       , 3) },
            { "sqrt", (TYPE.FUNCTION       , 3) },
            { "(",    (TYPE.L_BRACE        , -1) },
            { ")",    (TYPE.R_BRACE        , -1) },
            { "e",    (TYPE.CONSTANT       , NUMBER_PRECENDENCY)},
            { "pi",   (TYPE.CONSTANT       , NUMBER_PRECENDENCY)}
        };

    const char COMMA = ',';
    internal const int NUMBER_PRECENDENCY = 11;

    private TYPE _type;
    /// <summary>
    /// Тип текущего токена
    /// </summary>
    public TYPE Type { get => this._type; private set => _type = value; }

    private string _token;
    /// <summary>
    /// Строка, отображающая токен
    /// </summary>
    public string TokenString { get => this._token; }

    private int _precendency;

    public int Precendency { get => this._precendency; }
    /// <summary>
    /// Конструктор, создающий токен на основе строки и ее типа
    /// </summary>
    /// <param name="str"></param>
    /// <param name="type"></param>
    public Token(string str, TYPE type, int precendency)
    {
        this._token = str;
        this._type = type;
        this._precendency = precendency;
    }
    /// <summary>
    /// Функция, извлекающая из строки массив токенов
    /// </summary>
    /// <param name="str">Обрабатываемая строка</param>
    /// <returns>Массив токенов, представляющий строку</returns>
    public static Token[] Tokenize(string str)
    {
        var tokens = new List<Token>();
        var start = 0;
        while (start < str.Length)
        {
            if (Char.IsWhiteSpace(str[start]))
            {
                start++;
                continue;
            }
            start = ParseNum(tokens, str, start);
            if (start > str.Length - 1)
            {
                break;
            }
            var found = false;
            foreach (var tokenStr in tokenMap.Keys)
                if (!found && str[start..].StartsWith(tokenStr))
                {
                    var (tokenType, precendency) = tokenMap[tokenStr];
                    tokens.Add(new(tokenStr, tokenType, precendency));
                    start += tokenStr.Length;
                    found = true;
                    break;
                }
            if (!found && start < str.Length)
            {
                tokens.Add(new(str[start].ToString(), TYPE.VARIABLE, NUMBER_PRECENDENCY));
                start++;
            }
        }
        ParseUnary(tokens);
            
        return [.. tokens];
    }
    private static void ParseUnary(List<Token> tokens)
    {
        if (tokens[0].TokenString == "-")
            tokens[0].Type = TYPE.FUNCTION;

        for (var i = 1; i < tokens.Count - 1; i++)
        {
            if (tokens[i].TokenString == "-")
            {
                var prevToken = tokens[i - 1];
                var nextToken = tokens[i + 1];
                var isBinary = prevToken.IsNumber() && nextToken.IsNumber() ||
                               prevToken.Type == TYPE.R_BRACE && nextToken.Type == TYPE.L_BRACE ||
                               prevToken.IsNumber() && nextToken.Type == TYPE.L_BRACE ||
                               prevToken.Type == TYPE.R_BRACE && nextToken.IsNumber();
                if (!isBinary)
                {
                    tokens[i].Type = TYPE.FUNCTION;
                }
            }
        }
    }
    internal bool IsNumber()
        => this.Type == TYPE.FLOAT_NUM || this.Type == TYPE.INT_NUM ||
           this.Type == TYPE.CONSTANT || this.Type == TYPE.VARIABLE;

    internal bool IsBracket()
        => this.Type == TYPE.L_BRACE || this.Type == TYPE.R_BRACE;

    private static int ParseNum(List<Token> tokens, string str, int start)
    {
        var end = str.Length;
        while (end > start)
        {
            var parsableSubString = str[start..end];

            if (parsableSubString.Contains(COMMA))
            {
                if (double.TryParse(parsableSubString, out _))
                {
                    tokens.Add(new Token(str[start..end], TYPE.FLOAT_NUM, NUMBER_PRECENDENCY));
                    return end;
                }
            }
            if (int.TryParse(parsableSubString, out _))
            {
                tokens.Add(new Token(str[start..end], TYPE.INT_NUM, NUMBER_PRECENDENCY));
                return end;
            }
            end--;
        }
        return end;
    }
    /// <summary>
    /// Возвращает строковое представление токена
    /// </summary>
    /// <returns>Тип токена и строку, его представляющую</returns>
    public override string ToString()
    {
        return $"{_token} : {_type} : {_precendency}";
    }
}
