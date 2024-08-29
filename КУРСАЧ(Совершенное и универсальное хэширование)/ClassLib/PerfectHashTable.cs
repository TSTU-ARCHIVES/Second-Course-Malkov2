namespace ClassLib;
/// <summary>
/// Представляет совершенную хэш-таблицу.
/// </summary>
/// <typeparam name="K">Тип ключа.</typeparam>
/// <typeparam name="V">Тип значения.</typeparam>
public class PerfectHashTable<K, V>
{
    /// <summary>
    /// Хэш-таблица, содержащая внутренние хэш-таблицы.
    /// </summary>
    private readonly UniHashTable<int, UniHashTable<K, V>> _hashTable;
    /// <summary>
    /// Хэш-функция для хэширования ключей.
    /// </summary>
    private UniHashFunction _hashFunction;
    /// <summary>
    /// Количество элементов в таблице.
    /// </summary>
    public int Count { get; private set; }
    /// <summary>
    /// Инициализирует новую совершенную хэш-таблицу на основе переданных пар ключ-значение.
    /// </summary>
    /// <param name="keyValues">Коллекция пар ключ-значение.</param>
    public PerfectHashTable(IEnumerable<MyKeyValuePair<K, V>> keyValues)
    {
        _hashFunction = InitHashFunction();
        Count = keyValues.Count();
        _hashTable = InitValues(keyValues);
    }
    /// <summary>
    /// Инициализирует новую хэш-функцию.
    /// </summary>
    /// <returns>Новая хэш-функция.</returns>
    private static UniHashFunction InitHashFunction()
    {
        var rand = new Random();
        return new()
        {
            A = rand.Next(1, UniHashFunction.P - 1),
            B = rand.Next(0, UniHashFunction.P - 1)
        };
    }
    /// <summary>
    /// Инициализирует значения внутренних хэш-таблиц на основе переданных пар ключ-значение.
    /// </summary>
    /// <param name="keyValues">Коллекция пар ключ-значение.</param>
    /// <returns>Хэш-таблица с внутренними хэш-таблицами.</returns>
    private UniHashTable<int, UniHashTable<K, V>> InitValues(IEnumerable<MyKeyValuePair<K, V>> keyValues)
    {
        var table = new UniHashTable<int, UniHashTable<K, V>>(_hashFunction);
        var bucketsCapacity = new Dictionary<int, int>();
        foreach(var (key, _) in keyValues)
        {
            var hash = _hashFunction.GetHashCode(key) % Count;
            if (bucketsCapacity.TryGetValue(hash, out int amount))
                bucketsCapacity[hash] = ++amount;
            else 
                bucketsCapacity[hash] = 1;
        }
        foreach (var (key, value) in keyValues)
        {
            var hash = _hashFunction.GetHashCode(key) % Count;
            if (!table.ContainsKey(hash))
            {
                var innerTable = new UniHashTable<K, V>(bucketsCapacity[hash] * bucketsCapacity[hash]);
                innerTable.Add(key, value);
                table.Add(hash, innerTable);
            }
            else
            {
                table[hash].Add(key, value);
            }
        }
        return table;
    }
    /// <summary>
    /// Получает значение по заданному ключу.
    /// </summary>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <returns>Значение, соответствующее ключу.</returns>
    public V Get(K key)
    {
        int hash = _hashFunction.GetHashCode(key) % Count;
        return _hashTable[hash][key];
    }
    /// <summary>
    /// Пытается получить значение по заданному ключу.
    /// </summary>
    /// <param name="key">Ключ для поиска значения.</param>
    /// <param name="value">Найденное значение (если найдено).</param>
    /// <returns>True, если значение найдено; иначе - false.</returns>
    public bool TryGetValue(K key, out V? value)
    {
        int hash = _hashFunction.GetHashCode(key) % Count;
        if (_hashTable.TryGetValue(hash, out var table))
        {
            if (table.TryGetValue(key, out var result))
            {
                value = result;
                return true;
            }
        }
        value = default;
        return false;
    }
    /// <summary>
    /// Получает все пары ключ-значение из совершенной хэш-таблицы.
    /// </summary>
    /// <returns>Перечисление всех пар ключ-значение.</returns>
    public IEnumerable<MyKeyValuePair<K, V>> GetKeyValuePairs()
    {
        foreach (var kvs in _hashTable.GetKeyValuePairs())
        {
            foreach (var kv in kvs.Value.GetKeyValuePairs())
                yield return kv;
        }
        yield break;
    }
    /// <summary>
    /// Удаляет элемент с заданным ключом из совершенной хэш-таблицы.
    /// </summary>
    /// <param name="key">Ключ элемента для удаления.</param>
    public void DeleteKey(K key)
    {
        int hash = _hashFunction.GetHashCode(key) % Count;
        _hashTable.DeleteKey(hash);
    }
    /// <summary>
    /// Добавляет новый элемент с заданным ключом и значением в совершенную хэш-таблицу.
    /// </summary>
    /// <param name="key">Ключ нового элемента.</param>
    /// <param name="value">Значение нового элемента.</param>
    public void Add(K key, V value)
    {
        var hash = _hashFunction.GetHashCode(key) % Count;
        if (!_hashTable.ContainsKey(hash))
        {
            var innerTable = new UniHashTable<K, V>();
            innerTable.Add(key, value);
            _hashTable.Add(hash, innerTable);
        }
        else
        {
            _hashTable[hash].Add(key, value);
        }
    }
}