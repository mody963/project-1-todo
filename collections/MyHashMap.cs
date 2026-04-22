// mo

public class MyHashMap<TKey, TValue> : IMyCollection<KeyValuePair<TKey, TValue>>
{
    private MyLinkedList<KeyValuePair<TKey, TValue>>[] _buckets;
    // total buckets
    private int _capacity;
    
    public KeyValuePair<TKey, TValue>[] array
    {
        get{ return ToArray();}
        set{foreach( var item in value)
            {
                Add(item);
            }
        }
    }
    // total key-value pairs (items) in all buckets
    private int _count;

    public int Count => _count;
    public bool Dirty { get; private set; }
    private const double LOAD_FACTOR = 0.75; // real hashmaps use 0.72

    // decides capacity and makes a bucket array
    public MyHashMap(int capacity = 8)
    {
        _capacity = capacity < 4 ? 4 : capacity;
        _buckets = new MyLinkedList<KeyValuePair<TKey, TValue>>[_capacity];

        for (int i = 0; i < _capacity; i++)
            _buckets[i] = new MyLinkedList<KeyValuePair<TKey, TValue>>();
    }
    private void EnsureCapacity()
    {
        // if amount / capacity is smaller or equal than loadfactor dont do anything otherwise resize needed
        if ((double)Count / _capacity <= LOAD_FACTOR)
            return;

        Resize();
    }
    private void Resize()
    {
        // just double the size
        int newCapacity = _capacity * 2;
        var newBuckets = new MyLinkedList<KeyValuePair<TKey, TValue>>[newCapacity];

        // each position in the array gets a linkedlist bucket
        for (int i = 0; i < newCapacity; i++)
            newBuckets[i] = new MyLinkedList<KeyValuePair<TKey, TValue>>();

        // Rehash items
        for (int i = 0; i < _capacity; i++)
        {
            // iterator of old bucket
            var it = _buckets[i].GetIterator();

            // loop through all items
            while (it.HasNext())
            {
                var item = it.Next();

                // decide bucket index with new capacity
                int hash = item.Key == null ? 0 : item.Key.GetHashCode();
                hash &= 0x7fffffff;
                int index = hash % newCapacity;

                newBuckets[index].Add(item);
            }
        }

        _buckets = newBuckets;
        _capacity = newCapacity;
    }


    //takes key and returns index of it in the bucket array
    private int GetIndex(TKey key)
    {
        // if key null use 0, otherwise take the hashvalue
        int hash = key == null ? 0 : key.GetHashCode();
        // makes the hash value positive if its negative
        hash &= 0x7fffffff;
        // uses modulo to give the hash a index based on capacity
        return hash % _capacity;
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        int index = GetIndex(item.Key);
        var bucket = _buckets[index];

        var it = bucket.GetIterator();
        while (it.HasNext())
        {
            var current = it.Next();
            if (Equals(current.Key, item.Key))
                throw new ArgumentException("Duplicate key");
        }

        bucket.Add(item);
        _count++;
        Dirty = true;

        EnsureCapacity();
    }

    public void Remove(KeyValuePair<TKey, TValue> item)
    {
        int index = GetIndex(item.Key);
        var bucket = _buckets[index];

        var it = bucket.GetIterator();
        while (it.HasNext())
        {
            var current = it.Next();
            if (Equals(current.Key, item.Key))
            {
                bucket.Remove(current);
                _count--;
                Dirty = true;
                return;
            }
        }
    }

    public bool TryFindBy<K>(K key, Func<KeyValuePair<TKey, TValue>, K, int> comparer, out KeyValuePair<TKey, TValue> result)
    {
        foreach (var bucket in _buckets)
        {
            var it = bucket.GetIterator();
            while (it.HasNext())
            {
                var item = it.Next();
                if (comparer(item, key) == 0)
                {
                    result = item;
                    return true;
                }
            }
        }

        result = default;
        return false;
    }

    public IMyCollection<KeyValuePair<TKey, TValue>> Filter(Func<KeyValuePair<TKey, TValue>, bool> predicate)
    {
        var result = new MyHashMap<TKey, TValue>(_capacity);

        foreach (var bucket in _buckets)
        {
            var it = bucket.GetIterator();
            while (it.HasNext())
            {
                var item = it.Next();
                if (predicate(item))
                    result.Add(item);
            }
        }

        return result;
    }

    public void Sort(Comparison<KeyValuePair<TKey, TValue>> comparison)
    {
        // hashmaps dont have a basic sort
    }

    public KeyValuePair<TKey, TValue> Reduce(Func<KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>> accumulator)
    {
        var it = GetIterator();
        if (!it.HasNext())
            throw new InvalidOperationException("Empty collection");

        var result = it.Next();

        while (it.HasNext())
            result = accumulator(result, it.Next());

        return result;
    }

    public R Reduce<R>(R initial, Func<R, KeyValuePair<TKey, TValue>, R> accumulator)
    {
        var result = initial;
        var it = GetIterator();

        while (it.HasNext())
            result = accumulator(result, it.Next());

        return result;
    }

    public RResult Reduce<R, RResult>(R initial, Func<R, KeyValuePair<TKey, TValue>, R> accumulator, Func<R, RResult> resultSelector)
    {
        var result = Reduce(initial, accumulator);
        return resultSelector(result);
    }

    public IMyIterator<KeyValuePair<TKey, TValue>> GetIterator()
    {
        return new HashMapIterator(this);
    }

    public KeyValuePair<TKey, TValue>[] ToArray()
    {
        var arr = new KeyValuePair<TKey, TValue>[_count];
        int i = 0;

        var it = GetIterator();
        while (it.HasNext())
            arr[i++] = it.Next();

        return arr;
    }

    public void ResetDirty()
    {
        Dirty = false;
    }

    private class HashMapIterator : IMyIterator<KeyValuePair<TKey, TValue>>
    {
        private readonly MyHashMap<TKey, TValue> _map;
        // what index we are at currently
        private int _bucketIndex;
        //// second iterator for the linked list within the bucket
        private IMyIterator<KeyValuePair<TKey, TValue>>? _bucketIterator;

        // Constructor begins on -1 cause loop didnt start yet
        // bucket iterator is null because we havent chosen a bucket
        public HashMapIterator(MyHashMap<TKey, TValue> map)
        {
            _map = map;
            _bucketIndex = -1;
        }

        public bool HasNext()
        {
            if (_bucketIterator != null && _bucketIterator.HasNext())
                return true;

            // index for next bucket
            int i = _bucketIndex + 1;

            while (i < _map._capacity)
            {
                var it = _map._buckets[i].GetIterator();
                if (it.HasNext())
                {
                    _bucketIndex = i;
                    _bucketIterator = it;
                    return true;
                }
                i++;
            }

            return false;
        }

        public KeyValuePair<TKey, TValue> Next()
        {
            if (!HasNext())
                throw new InvalidOperationException();

            return _bucketIterator!.Next();
        }

        public void Reset()
        {
            _bucketIndex = -1;
            _bucketIterator = null;
        }
    }
}