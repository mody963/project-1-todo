public class MyArrayList<T> : IMyCollection<T>
{
    private T[] _items;
    private int _count;
    private const int DefaultCapacity = 4;

    public int Count => _count;
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
        if (_count == _items.Length) EnsureCapacity();
        _items[_count++] = item;
        Dirty = true;
    }

    //basically makes a copy of the array and resizes it to double the size so it still has empty places.
    private void EnsureCapacity()
    {
        if (_count < _items.Length)
            return;

        int newSize = _items.Length * 2;
        T[] newArray = new T[newSize];

        for (int i = 0; i < _count; i++)
            newArray[i] = _items[i];

        _items = newArray;
    }

    public void Remove(T item)
    {
        for (int i = 0; i < _count; i++)
        {

            if (Equals(_items[i], item))
            {
                // Shift all elements after 'i' one position to the left
                // This overwrites the removed item and closes the gap
                for (int j = i; j < _count - 1; j++)
                {
                    _items[j] = _items[j + 1];
                }
                // Clear the last element (which is now duplicated after shifting)
                // and decrement the count
                _count--;
                _items[_count] = default!;
                Dirty = true;
                return;
            }
        }
    }

    public T FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        for (int i = 0; i < _count; i++)
        {
            if (comparer(_items[i], key)) return _items[i];
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

    public IMyIterator<T> GetIterator()
    {
        return new MyArrayIterator<T>(_items, _count);
    }
}
