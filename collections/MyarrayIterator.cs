public class MyArrayIterator<T> : IMyIterator<T>
{
    private readonly T[] _items;
    private readonly int _count;
    private int _index;

    public MyArrayIterator(T[] items, int count)
    {
        _items = items;
        _count = count;
        _index = 0;
    }
    public bool HasNext()
    {
        if (_index < _count) return true;
        return false;
    }

    public T Next()
    {
        if (HasNext() == false)
        {
            throw new InvalidOperationException("No more elements in the collection.");
        }
        return _items[_index++];
    }
    public void Reset()
    {
        _index = 0;
    }
}