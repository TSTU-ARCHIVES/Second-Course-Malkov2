namespace ClassLib;
/// <summary>
/// Хэш-таблица, использующая в качестве хэш-функции универсальную хэш-функцию
/// </summary>
/// <typeparam name="K">Тип данных ключей</typeparam>
/// <typeparam name="V">Тип данных значений</typeparam>
public class UniHashTable<K, V>
{
    /// <summary>
    /// Начальный размер таблицы
    /// </summary>
    private const int START_SIZE = 10;
    /// <summary>
    /// Коэфициент заполнения, при котором таблица перестраивается
    /// </summary>
    private const double MAX_FILL_COEFFICIENT = 0.7;
    /// <summary>
    /// Используемая универсальная хэш-функция
    /// </summary>
    private readonly UniHashFunction _hashFunction;
    /// <summary>
    /// Фактический размер табицы
    /// </summary>
    private int _size = START_SIZE;
    /// <summary>
    /// Массив связных списков пар ключ-значение, в котором хранятся данные
    /// </summary>

    private LinkedList<MyKeyValuePair<K, V>>[] _values;
    /// <summary>
    /// Количество пар ключ-значение в табице
    /// </summary>
    public int Count { get; protected set; }
    /// <summary>
    /// Индексатор, возвращающий значение по ключу
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <returns>Значение</returns>
    public V this[K key]
    {
        get => Get(key);
        set => Add(key, value);
    }
    /// <summary>
    /// Конструктор, создающий пустую новую хэш-таблицу
    /// </summary>
    public UniHashTable()
    {
        _values = InitValues();
        _hashFunction = InitHashFunction();
        Count = 0;
    }

    /// <summary>
    /// Конструктор, создающий пустую новую хэш-таблицу
    /// </summary>
    public UniHashTable(int capacity)
    {
        _values = InitValues(capacity);
        _hashFunction = InitHashFunction();
        _size = capacity;
        Count = 0;
    }
    /// <summary>
    /// Конструктор, создающий пустую новую хэш-таблицу 
    /// с заданной универсальной функцией хэширования
    /// </summary>
    /// <param name="hashFunction">Универсальная функция хэширования</param>
    internal UniHashTable(UniHashFunction hashFunction)
    {
        _values = InitValues();
        _hashFunction = hashFunction;
        Count = 0;
    }
    /// <summary>
    /// Инициализирует значения
    /// </summary>
    /// <returns>Массив связных списков пар ключ-значение, в котором будут хранится данные</returns>
    private static LinkedList<MyKeyValuePair<K, V>>[] InitValues(int capacity = START_SIZE)
    {
        var values = new LinkedList<MyKeyValuePair<K, V>>[capacity];
        for (int i = 0; i < capacity; i++)
        {
            values[i] = [];
        }
        return values;
    }
    /// <summary>
    /// Создает для данной таблицы случайную универсальную хэш-функцию
    /// </summary>
    /// <returns>Случайная универсальная хэш-функция</returns>
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
    /// Добавляет пару ключ-значение в массив значений
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <param name="items">Массив значений</param>
    private void AddPair(K key, V value, LinkedList<MyKeyValuePair<K, V>>[] items)
    {
        var index = _hashFunction.GetHashCode(key) % items.Length;
        items[index].AddLast(new MyKeyValuePair<K,V>(key, value));
    }
    /// <summary>
    /// Осуществляет вставку в хэш-таблицу
    /// </summary>
    /// <param name="key">Вставляемый ключ</param>
    /// <param name="value">Ассоциированный с ключом элемент</param>
    public void Add(K key, V value)
    {
        if ((double)Count/_size > MAX_FILL_COEFFICIENT)
        {
            Regroup(_size * 2);
        }
        AddPair(key, value, _values);
        Count++;
        
    }
    /// <summary>
    /// Перегруппирует хэш-таблицу согласно новому размеру
    /// </summary>
    /// <param name="newSize">Новый размер</param>
    private void Regroup(int newSize)
    {
        var newValues = new LinkedList<MyKeyValuePair<K, V>>[newSize];
        for(int i = 0; i < newSize; i++)
        {
            newValues[i] = [];
        }
        foreach (var item in _values)
        {
            foreach (var kv in item)
            {
                AddPair(kv.Key, kv.Value, newValues);
            }
        }

        _size = newSize;
        _values = newValues;
    }
    /// <summary>
    /// Пробует получить значение по данному ключу
    /// </summary>
    /// <param name="key">Данный ключ</param>
    /// <param name="value">Выходное значение, если ключ есть в таблице, иначе default</param>
    /// <returns>
    /// True: ключ присутсивует в таблице
    /// False: ключ отсутсвтует в таблице
    /// </returns>
    public bool TryGetValue(K key, out V? value)
    {
        var index = _hashFunction.GetHashCode(key) % _size;
        if (_values[index] == null)
        {
            value = default;
            return false;
        }
        foreach (var kv in _values[index])
        {
            if (kv != null && kv.Key.Equals(key))
            {
                value = kv.Value;
                return true;
            }
        }
        value = default;
        return false;
    }
    /// <summary>
    /// Осуществляет получение значения по данному ключу
    /// </summary>
    /// <param name="key">Данный ключ</param>
    /// <returns>Значение в таблице по данному ключу</returns>
    /// <exception cref="ArgumentException">Исключение, если ключ не найден</exception>
    public V Get(K key)
    {
        var index = _hashFunction.GetHashCode(key) % _size;
        foreach(var kv in _values[index])
        {
            if (kv.Key.Equals(key))
                return kv.Value;
        }
        throw new ArgumentException("Key not found");
    }
    /// <summary>
    /// Проверяет, содержится ли ключ в хэш-таблице
    /// </summary>
    /// <param name="key">Проверяемый ключ</param>
    /// <returns>
    /// True: ключ присутсивует в таблице
    /// False: ключ отсутсвтует в таблице
    /// </returns>
    public bool ContainsKey(K key)
    {
        var index = _hashFunction.GetHashCode(key) % _size;

        return _values[index].Where(kv => kv.Key.Equals(key)).Any();
    }
    /// <summary>
    /// Перечисляет все пары ключ-начение в текущей хэш-таблице
    /// </summary>
    /// <returns>Все пары ключ-начение в текущей хэш-таблице</returns>
    public IEnumerable<MyKeyValuePair<K, V>> GetKeyValuePairs() //
    {
        foreach(var kvs in _values)
        {
            if (kvs.Count > 0)
                foreach (var kv in kvs)
                    yield return kv;
        }
        yield break;
    }
    /// <summary>
    /// Удаляет значение по данному ключу вместе с ключом,
    /// либо ничего если ключ отсутсвует
    /// </summary>
    /// <param name="key">Удаляемый ключ</param>
    public void DeleteKey(K key)
    {
        var index = _hashFunction.GetHashCode(key) % _size;
        foreach(var kv in _values[index])
        {
            if (kv.Key.Equals(key))
            {
                _values[index].Remove(kv);
                break;
            }
        }
    }

}
