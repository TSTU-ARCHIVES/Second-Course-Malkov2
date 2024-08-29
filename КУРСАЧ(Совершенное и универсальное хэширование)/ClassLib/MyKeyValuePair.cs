namespace ClassLib;
/// <summary>
/// Представляет пару ключ-значение.
/// </summary>
/// <typeparam name="K">Тип ключа.</typeparam>
/// <typeparam name="V">Тип значения.</typeparam>
/// <param name="key">Ключ пары.</param>
/// <param name="value">Значение пары.</param>
public class MyKeyValuePair<K, V>(K key, V value)
{
    /// <summary>
    /// Получает ключ пары.
    /// </summary>
    public K Key { get; private set; } = key;
    /// <summary>
    /// Получает значение пары.
    /// </summary>
    public V Value { get; private set; } = value;
    /// <summary>
    /// Разбивает ключ-значение на отдельные ключ и значение.
    /// </summary>
    /// <param name="key">Извлеченный из пары ключ.</param>
    /// <param name="value">Извлеченное из пары значение.</param>
    public void Deconstruct(out K key, out V value)
    {
        key = Key;
        value = Value;
    }

    public override string ToString()
        => $"{{Ключ: {Key}, Значение: {Value}}}";
}
