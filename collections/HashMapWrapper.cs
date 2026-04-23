// this file contains all 3 wrappers not just the taskitem
// mo
// taskitem wrapper
public class TaskHashMapCollection : IMyCollection<TaskItem> 
{
    private readonly MyHashMap<int, TaskItem> _map = new();

    public TaskItem[] array
    {
        get{ return ToArray();}
        set{foreach( var item in value)
            {
                Add(item);
            }
        }
    }
    public int Count => _map.Count;
    public bool Dirty => _map.Dirty;

    public void Add(TaskItem item)
    {
        _map.Add(new KeyValuePair<int, TaskItem>(item.Id, item));
    }

    public void Remove(TaskItem item)
    {
        _map.Remove(new KeyValuePair<int, TaskItem>(item.Id, item));
    }

    public bool TryFindBy<K>(K key, Func<TaskItem, K, int> comparer, out TaskItem? result)
    {
        bool found = _map.TryFindBy(key, (pair, k) => comparer(pair.Value, k), out var kvp);

        result = found ? kvp.Value : default;
        return found;
    }

    public IMyCollection<TaskItem> Filter(Func<TaskItem, bool> predicate)
    {
        var result = new TaskHashMapCollection();
        var it = _map.GetIterator();

        while (it.HasNext())
        {
            var item = it.Next().Value;
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public void Sort(Comparison<TaskItem> comparison)
    {
        // nothing
    }

    public TaskItem Reduce(Func<TaskItem, TaskItem, TaskItem> accumulator)
    {
        var result = _map.Reduce((a, b) =>
        {
            var reduced = accumulator(a.Value, b.Value);
            return new KeyValuePair<int, TaskItem>(a.Key, reduced);
        });

        return result.Value;
    }

    public R Reduce<R>(R initial, Func<R, TaskItem, R> accumulator)
    {
        return _map.Reduce(initial, (acc, kvp) => accumulator(acc, kvp.Value));
    }

    public RResult Reduce<R, RResult>(R initial, Func<R, TaskItem, R> accumulator, Func<R, RResult> resultSelector)
    {
        return _map.Reduce(initial, (acc, kvp) => accumulator(acc, kvp.Value), resultSelector);
    }

    public IMyIterator<TaskItem> GetIterator()
    {
        return new TaskIterator(_map);
    }

    public TaskItem[] ToArray()
    {
        var arr = new TaskItem[Count];
        int i = 0;

        var it = GetIterator();
        while (it.HasNext())
            arr[i++] = it.Next();

        return arr;
    }

    public void ResetDirty()
    {
        _map.ResetDirty();
    }

    private class TaskIterator : IMyIterator<TaskItem>
    {
        private readonly IMyIterator<KeyValuePair<int, TaskItem>> _it;

        public TaskIterator(MyHashMap<int, TaskItem> map)
        {
            _it = map.GetIterator();
        }

        public bool HasNext() => _it.HasNext();

        public TaskItem Next() => _it.Next().Value;

        public void Reset() => _it.Reset();
    }
}

// person wrapper
public class PersonHashMapCollection : IMyCollection<Person>
{
    private readonly MyHashMap<int, Person> _map = new();

    public Person[] array
    {
        get{ return ToArray();}
        set{foreach( var item in value)
            {
                Add(item);
            }
        }
    }
    public int Count => _map.Count;
    public bool Dirty => _map.Dirty;

    public void Add(Person item)
    {
        _map.Add(new KeyValuePair<int, Person>(item.Id, item));
    }

    public void Remove(Person item)
    {
        _map.Remove(new KeyValuePair<int, Person>(item.Id, item));
    }

    public bool TryFindBy<K>(K key, Func<Person, K, int> comparer, out Person? result)
    {
        bool found = _map.TryFindBy(key, (pair, k) => comparer(pair.Value, k), out var kvp);
        result = found ? kvp.Value : default;
        return found;
    }

    public IMyCollection<Person> Filter(Func<Person, bool> predicate)
    {
        var result = new PersonHashMapCollection();
        var it = _map.GetIterator();

        while (it.HasNext())
        {
            var item = it.Next().Value;
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public void Sort(Comparison<Person> comparison) { }

    public Person Reduce(Func<Person, Person, Person> accumulator)
    {
        var result = _map.Reduce((a, b) =>
            new KeyValuePair<int, Person>(a.Key, accumulator(a.Value, b.Value)));

        return result.Value;
    }

    public R Reduce<R>(R initial, Func<R, Person, R> accumulator)
    {
        return _map.Reduce(initial, (acc, kvp) => accumulator(acc, kvp.Value));
    }

    public RResult Reduce<R, RResult>(R initial, Func<R, Person, R> accumulator, Func<R, RResult> resultSelector)
    {
        return _map.Reduce(initial, (acc, kvp) => accumulator(acc, kvp.Value), resultSelector);
    }

    public IMyIterator<Person> GetIterator() => new Iterator(_map);

    public Person[] ToArray()
    {
        var arr = new Person[Count];
        int i = 0;
        var it = GetIterator();

        while (it.HasNext())
            arr[i++] = it.Next();

        return arr;
    }

    public void ResetDirty() => _map.ResetDirty();

    private class Iterator : IMyIterator<Person>
    {
        private readonly IMyIterator<KeyValuePair<int, Person>> _it;

        public Iterator(MyHashMap<int, Person> map) => _it = map.GetIterator();
        public bool HasNext() => _it.HasNext();
        public Person Next() => _it.Next().Value;
        public void Reset() => _it.Reset();
    }
}
// allocation wrapper

// File: AllocationHashMapCollection.cs
public class AllocationHashMapCollection : IMyCollection<Task_Allocation>
{
    private readonly MyHashMap<string, Task_Allocation> _map = new();

    public Task_Allocation[] array
    {
        get{ return ToArray();}
        set{foreach( var item in value)
            {
                Add(item);
            }
        }
    }
    public int Count => _map.Count;
    public bool Dirty => _map.Dirty;

    private string GetKey(Task_Allocation a)
        => $"{a.Task.Id}-{a.Person.Id}"; // shared key

    public void Add(Task_Allocation item)
    {
        _map.Add(new KeyValuePair<string, Task_Allocation>(GetKey(item), item));
    }

    public void Remove(Task_Allocation item)
    {
        _map.Remove(new KeyValuePair<string, Task_Allocation>(GetKey(item), item));
    }

    public bool TryFindBy<K>(K key, Func<Task_Allocation, K, int> comparer, out Task_Allocation? result)
    {
        bool found = _map.TryFindBy(key, (pair, k) => comparer(pair.Value, k), out var kvp);
        result = found ? kvp.Value : default;
        return found;
    }

    public IMyCollection<Task_Allocation> Filter(Func<Task_Allocation, bool> predicate)
    {
        var result = new AllocationHashMapCollection();
        var it = _map.GetIterator();

        while (it.HasNext())
        {
            var item = it.Next().Value;
            if (predicate(item))
                result.Add(item);
        }

        return result;
    }

    public void Sort(Comparison<Task_Allocation> comparison) { }

    public Task_Allocation Reduce(Func<Task_Allocation, Task_Allocation, Task_Allocation> accumulator)
    {
        var result = _map.Reduce((a, b) =>
            new KeyValuePair<string, Task_Allocation>(a.Key, accumulator(a.Value, b.Value)));

        return result.Value;
    }

    public R Reduce<R>(R initial, Func<R, Task_Allocation, R> accumulator)
    {
        return _map.Reduce(initial, (acc, kvp) => accumulator(acc, kvp.Value));
    }

    public RResult Reduce<R, RResult>(R initial, Func<R, Task_Allocation, R> accumulator, Func<R, RResult> resultSelector)
    {
        return _map.Reduce(initial, (acc, kvp) => accumulator(acc, kvp.Value), resultSelector);
    }

    public IMyIterator<Task_Allocation> GetIterator() => new Iterator(_map);

    public Task_Allocation[] ToArray()
    {
        var arr = new Task_Allocation[Count];
        int i = 0;
        var it = GetIterator();

        while (it.HasNext())
            arr[i++] = it.Next();

        return arr;
    }

    public void ResetDirty() => _map.ResetDirty();

    private class Iterator : IMyIterator<Task_Allocation>
    {
        private readonly IMyIterator<KeyValuePair<string, Task_Allocation>> _it;

        public Iterator(MyHashMap<string, Task_Allocation> map) => _it = map.GetIterator();
        public bool HasNext() => _it.HasNext();
        public Task_Allocation Next() => _it.Next().Value;
        public void Reset() => _it.Reset();
    }
}