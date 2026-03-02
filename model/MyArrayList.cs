public class MyArrayList<T> : IMyCollection<T> where T : IEquatable<T>
{
    private T[] _items;
    private int _count;
    private const int DefaultCapacity = 4;

    public int Count => _count;

    // public T this[int index]
    // {
    //     get
    //     {
    //         if (index < 0 || index >= _count)
    //             throw new IndexOutOfRangeException();

    //         return _items[index];
    //     }
    // }
    public bool Dirty { get; set; }

    public MyArrayList(int capacity = DefaultCapacity)
    {
        if (capacity <= 0)
            capacity = DefaultCapacity;

        _items = new T[capacity];
        _count = 0;
    }

    public void Add(T item)
    {
        EnsureCapacity();
        // count is the number of elements so its the last element if u add 1 to it its the next empty spot.
        _items[_count++] = item;
        Dirty = true;
    }

    //basically makes a copy of the array and resizes it to double the size so it still has empty places.
    private void EnsureCapacity()
    {
        if (_count < _items.Length)
            return;

        int newSize = _items.Length == 0 ? DefaultCapacity : _items.Length * 2;
        T[] newArray = new T[newSize];

        for (int i = 0; i < _count; i++)
            newArray[i] = _items[i];

        _items = newArray;
    }

    public void Remove(T item)
    {
        if (item != null)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Equals(_items[i], item))
                {
                    ShiftLeft(i);
                    Dirty = true;
                    return;
                }
            }
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        if (key != null)
        {
            for (int i = 0; i < _count; i++)
            {
                if (comparer(_items[i], key)) return _items[i];
            }
            
        }
        return default!;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        var result = new MyArrayList<T>();
        for (int i = 0; i < _count; i++)
        {
            if (predicate(_items[i])) result.Add(_items[i]);
        }
        return result;
    }

    public void Sort(Comparison<T> comparison)
    {
        for (int i = 0; i < _count - 1; i++)
        {
            //_count - i - 1  last index for inner loop (ignores already sorted elements)
            for (int j = 0; j < _count - i - 1; j++)
            {
                if (comparison(_items[j], _items[j + 1]) > 0)
                {
                    T temp = _items[j];
                    _items[j] = _items[j + 1];
                    _items[j + 1] = temp;
                }
            }
        }
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R current = initial;
        for (int i = 0; i < _count; i++)
        {
            current = accumulator(current, _items[i]);
        }
        return current;
    }
    public T Reduce(Func<T, T, T> accumulator)
    {
        if (_count == 0)
            throw new InvalidOperationException("Cannot reduce empty collection.");

        T current = default!;
        for (int i = 1; i < _count; i++)
        {
            current = accumulator(current, _items[i]);
        }

        return current;
    }
    public RResult Reduce<R, RResult>(R initial, Func<R, T, R> accumulator, Func<R, RResult> resultSelector)
    {
        R current = initial;
        for (int i = 0; i < _count; i++)
        {
            current = accumulator(current, _items[i]);
        }
        return resultSelector(current);
    }


    public IMyIterator<T> GetIterator()
    {
        return new MyArrayIterator<T>(_items, _count);
    }
    private void ShiftLeft(int startIndex)
    {
        // _count - 1 is the index of the last valid element in the list.
        for (int i = startIndex; i < _count - 1; i++)
        {
            _items[i] = _items[i + 1];
        }

        _count--;
        // reset final item
        _items[_count] = default!;
    }
    private void ShiftRight(int startIndex)
    {
        EnsureCapacity();
        for (int i = _count; i > startIndex; i--)
        {
            _items[i] = _items[i - 1];
        }

        _count++;
    }
    public T[] ToArray()
    {
        T[] arr = new T[_count];
        for (int i = 0; i < _count; i++)
            arr[i] = _items[i];
        return arr;
    }
}
