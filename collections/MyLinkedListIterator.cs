// Aimee


public class MyLinkedListIterator<T> : IMyIterator<T>
{
    private MyLinkedList<T>.Node? _head;
    private MyLinkedList<T>.Node? _current;

    public MyLinkedListIterator(MyLinkedList<T>.Node? head) // gebruik maken van de node in andere class alleen deze wil je natuurlijk niet opnieuw hoeven maken. 
    {
        _head = head; // allee head nodig want die refereerd naar de rest sws toe. 
        _current = null; // je begint altijd bij null omdat je eerst kijkt of de head wel meer heeft en daarna ga je pas bij next kijken. 
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
            throw new InvalidOperationException("No more elements");

        _current = _current == null ? _head : _current.Next;
        if (_current == null || _current.Data == null)
            throw new InvalidOperationException("Current node is null");
        return _current.Data;
    }

    public void Reset()
    {
        _current = null;
    }
}