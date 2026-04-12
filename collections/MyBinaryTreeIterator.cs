// Aimee


using Spectre.Console;

class MyBinaryTreeIterator<T> : IMyIterator<T> where T : IComparable<T>
{
    private MyBinaryTree<T>.Node? _root;
    private MyBinaryTree<T>.Node? _current;

    public MyBinaryTreeIterator(MyBinaryTree<T>.Node? root) // gebruik maken van de node in andere class alleen deze wil je natuurlijk niet opnieuw hoeven maken. 
    {
        _root = root; // allee head nodig want die refereerd naar de rest sws toe. 
        _current = null; // je begint altijd bij null omdat je eerst kijkt of de head wel meer heeft en daarna ga je pas bij next kijken. 
    }

    public bool HasNext()
    {
        if (_current == null)
            return _root != null;
        MyBinaryTree<T>.Node value = FindSuccessor(_root, _current);
        if(value == null)
        {
            return false;
        }
        if(value == _current)
        {
            return false;
        }
        
        return true;
    }

    public T Next()
    {
        if (!HasNext())
            throw new InvalidOperationException("No more elements");

        if(_current == null)
        {
            _current = _root;
            while (_current.Left != null)
            {
                _current = _current.Left;
            }
            return _current.Value;
        }
        _current = FindSuccessor(_root, _current);
        if (_current == null || _current.Value == null)
            throw new InvalidOperationException("Current node is null");
        return _current.Value;
    }

    private MyBinaryTree<T>.Node FindSuccessor(MyBinaryTree<T>.Node root, MyBinaryTree<T>.Node target)
    {
        if (target == null) return null;

        // Case 1: Right subtree exists → leftmost node in right subtree
        if (target.Right != null)
        {
            MyBinaryTree<T>.Node curr = target.Right;
            while (curr.Left != null)
                curr = curr.Left;
            return curr;
        }

        // Case 2: No right subtree → search from root
        MyBinaryTree<T>.Node successor = null;
        MyBinaryTree<T>.Node ancestor = root;

        while (ancestor != null)
        {
            if (target.Value.CompareTo(ancestor.Value) < 0)
            {
                successor = ancestor; // potential successor
                ancestor = ancestor.Left;
            }
            else if (target.Value.CompareTo(ancestor.Value) > 0)
            {
                ancestor = ancestor.Right;
            }
            else
            {
                break; // found target
            }
        }

        return successor;
    }

    public void Reset()
    {
        _current = null;
    }
}