namespace ClassLib;
/// <summary>
/// Структура, хранящая информацию о конкретном слове
/// </summary>
public struct WordInfo(int position)
{
    /// <summary>
    /// Позициия - номер слова в тексте
    /// </summary>
    public int Position { get; private set; } = position;
    /// <summary>
    /// Флаг, указывающий, выделено ли слово
    /// </summary>
    public bool IsHighlighted { get; set; }

    public override readonly string ToString()
    {
        return $"Position: {Position}; IsHighlighted: {IsHighlighted}";
    }
}
