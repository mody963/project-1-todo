class MyLinkedListIterator<T> : IMyIterator<T>
{
    private MyLinkedList<T>.Node? _head;
    private MyLinkedList<T>.Node? _current;

    public MyLinkedListIterator(MyLinkedList<T>.Node? head) // gebruik maken van de node in andere class alleen deze wil je natuurlijk niet opnieuw hoeven maken. 
    {
        _head = head; // allee head nodig want die refereerd naar de rest sws toe. 
        _current = null;
    }

    public bool HasNext()
    {
        if (_current == null)
            return _head != null;

        return _current.Next != null; // omdat je wilt weten of er nog een volgende is.
    }

    public T Next()
    {
        if (!HasNext())
            throw new InvalidOperationException();

        if (_current == null)
            _current = _head;
        else
            _current = _current.Next;

        return _current.Data;
    }

    public void Reset()
    {
        _current = null;
    }
}