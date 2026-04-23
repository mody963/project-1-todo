// mo

public class MyArrayList<T> : IMyCollection<T> where T : IEquatable<T>, IComparable<T>
{
    private T[] _items;
    private int _count;
    private const int DefaultCapacity = 4;

    public T[] array
    {
        get{ return ToArray();}
        set{_items = value;
        _count = _items.Count();}
    }

    public int Count {get => _count;} // met die count kan je bij elke andere file zien wat de lengte van array is, maar je kan het niet aanpassen

    public bool Dirty { get; private set; } // maybe backingfield same as count.

    public MyArrayList(int capacity = DefaultCapacity)
    {
        if (capacity <= 0)
            capacity = DefaultCapacity;

        _items = new T[capacity];
        _count = 0;
    }
    public MyArrayList()
    {
        _items = new T[DefaultCapacity];
        _count = 0;
    }

    public void Add(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        EnsureCapacity();
        // count is the number of elements so its the last element if u add 1 to it its the next empty spot.
        // count++ you increase count after adding an item
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
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));

        }
        for (int i = 0; i < _count; i++)
        {
            if (_items[i].Equals(item))
            {
                ShiftLeft(i);
                Dirty = true;
                return;
            }
            
        }
    }
    public bool TryFindBy<K>(K key, Func<T, K, int> comparer, out T? result) //This is where the found item is returned, because of out
    {
        if (comparer == null)
            throw new ArgumentNullException(nameof(comparer));

        for (int i = 0; i < _count; i++)
        {
            if (comparer(_items[i], key) == 0)
            {
                result = _items[i];
                return true;
            }
        }

        result = default; // if not found set to default but if its an int it doesnt matter because we return false
        return false;
    }
    // public T FindBy<K>(K key, Func<T, K, int> comparer)
    // {
    //     if (comparer == null)
    //         throw new ArgumentNullException(nameof(comparer));

    //     for (int i = 0; i < _count; i++)
    //     {
    //         if (comparer(_items[i], key) == 0)
    //             return _items[i];
    //     }

    //     return default!;
    // }
    public bool TryFindBy<K>(K key1, K key2, Func<T, K, K, int> comparer, out T? result)
    {
        if (comparer == null)
            throw new ArgumentNullException(nameof(comparer));

        for (int i = 0; i < _count; i++)
        {
            if (comparer(_items[i], key1, key2) == 0)
            {
                result = _items[i];
                return true;
            }
        }

        result = default;
        return false;
    }

    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        var result = new MyArrayList<T>();
        for (int i = 0; i < _count; i++)
        {
            if (predicate(_items[i])) result.Add(_items[i]);
        }
        return result;
    }

    // public void Sort(Comparison<T> comparison) this was a bubble sort
    // {
    //     if (comparison == null) throw new ArgumentNullException(nameof(comparison));
    //     for (int i = 0; i < _count - 1; i++)
    //     {
    //         //_count - i - 1  last index for inner loop (ignores already sorted elements)
    //         for (int j = 0; j < _count - i - 1; j++)
    //         {
    //             if (comparison(_items[j], _items[j + 1]) > 0)
    //             {
    //                 T temp = _items[j];
    //                 _items[j] = _items[j + 1];
    //                 _items[j + 1] = temp;
    //             }
    //         }
    //     }
    //     Dirty = true;
    // }
    public void Sort(Comparison<T> comparison)
    {
        if (comparison == null) throw new ArgumentNullException(nameof(comparison));

        for (int i = 1; i < _count; i++) // loop from second item to last
        {
            T key = _items[i]; // je slaat item op 
            int j = i - 1; // j is 1 index voor de opgeslagen item

            // left side sorted and we keep shifting to right
            // je verlaagt j met 1 na elke loop we doen j >= 0 want als j 0 is of kleiner dan ben je uit de index range
            //compare with left element, if bigger shift right
            while (j >= 0 && comparison(_items[j], key) > 0) // comp checks if items[j] is bigger than key
            {
                _items[j + 1] = _items[j]; // je blijft item verplaatsen naar rechts 
                j--; // dan vergelijk je 1 index naar links
                // je blijft naar rechts vergelijken, maar j-- zodat je het uiteindelijk naar links toe brengt, de sorted kant dus
            }

            // Insert key at its correct position
            _items[j + 1] = key;
        }

        Dirty = true;
    }

    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        R current = initial;
        for (int i = 0; i < _count; i++)
        {
            current = accumulator(current, _items[i]); // je blijft current updaten met item[i] daarom sla je het op
        }
        return current;
    }
    public T Reduce(Func<T, T, T> accumulator)
    {
        if (_count == 0)
            throw new InvalidOperationException("Cannot reduce empty collection.");

        T current = _items[0];
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
        if (startIndex < 0 || startIndex >= _count)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex));
        }
        // _count - 1 is the index of the last valid element in the list.
        for (int i = startIndex; i < _count - 1; i++) // count - 1 is last item
        {
            _items[i] = _items[i + 1]; // item[i] gets set to the next item
        }

        _count--; // last index of the list/ we lower the count of items in the list 
        // reset final item
        _items[_count] = default!; // we set the last item to default as its gone
    }
    public T[] ToArray()
    {
        T[] arr = new T[_count];
        for (int i = 0; i < _count; i++)
            arr[i] = _items[i];
        return arr;
    }
    public void ResetDirty()
    {
        Dirty = false;
    }
  
}
