using System.Collections;

namespace ClassLib;
/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class MyList<T> : IList<T>, ICloneable
{
    /// <summary>
    /// 
    /// </summary>
    private T[] _items;
    /// <summary>
    /// 
    /// </summary>
    private int _count;
    public int Count
    {
        get => _count;
    }

    public bool IsReadOnly => ((ICollection<T>)_items).IsReadOnly;

    public T this[int index] { get => ((IList<T>)_items)[index]; set => ((IList<T>)_items)[index] = value; }

    public MyList()
    {
        _items = new T[4];
        _count = 0;
    }

    public MyList(int startCapacity)
    {
        _items = new T[startCapacity];
        _count = 0;
    }

    public void Add(T item)
    {
        if (_count == _items.Length)
        {
            Array.Resize(ref _items, _items.Length * 2);
        }
        _items[_count] = item;
        _count++;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= _count)
        {
            throw new IndexOutOfRangeException();
        }
        return _items[index];
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
            yield return _items[i];
        yield break;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return ((IList<T>)_items).IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        if (index < _count)
            ((IList<T>)_items).Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        if (index < _count)
            ((IList<T>)_items).RemoveAt(index);
    }

    public void Clear()
    {
        ((ICollection<T>)_items).Clear();
        _count = 0;
    }

    public bool Contains(T item)
    {
        return ((ICollection<T>)_items).Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        ((ICollection<T>)_items).CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return ((ICollection<T>)_items).Remove(item);
    }

    public object Clone()
    {
        var clone = new MyList<T>
        {
            _count = _count,
            _items = (T[])_items.Clone()
        };
        return clone;
    }
}
