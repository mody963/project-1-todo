using Spectre.Console;

public class MyBinaryTree<T>: IMyCollection<T> where T : IComparable<T>
{
    
    public class Node
    {
        public T Value;
        public Node? Left;
        public Node? Right;

        public Node(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }
    public Node? Root;

    private int _count;
    public int Count => _count;

    public bool Dirty { get; private set; }

    public void ResetDirty()
    {
        Dirty = false;
    }

    public MyBinaryTree()
    {
    }
    
    public MyBinaryTree(IMyCollection<T>? CollectionYouWantToAdd)
    {
        ArgumentNullException.ThrowIfNull(CollectionYouWantToAdd);
        var iterator = CollectionYouWantToAdd.GetIterator();

        while (iterator.HasNext())
        {
            Add(iterator.Next());
        }

    }
    
    // Insert a value into the MyBinaryTree
    public void Add(T value)
    {
        if(value == null)
        {
            throw new ArgumentNullException(nameof(value), "Cannot insert null value.");
        }
        Root = InsertRec(Root, value);
        _count++;
        Dirty = true;
    }

    private Node InsertRec(Node root, T value)
    {
        if (root == null)
            return new Node(value);

        if (root.Value.CompareTo(value) > 0)
            root.Left = InsertRec(root.Left, value);
        else if (root.Value.CompareTo(value) < 0)
            root.Right = InsertRec(root.Right, value);

        return root;
    }

    // Step 1: Create backbone (vine)
    private int CreateBackbone()
    {
        Node grandParent = null;
        Node parent = Root;
        int count = 0;

        while (parent != null)
        {
            if (parent.Left != null)
            {
                Node leftChild = parent.Left;
                parent.Left = leftChild.Right;
                leftChild.Right = parent;

                if (grandParent == null)
                    Root = leftChild;
                else
                    grandParent.Right = leftChild;

                parent = leftChild;
            }
            else
            {
                count++;
                grandParent = parent;
                parent = parent.Right;
            }
        }
        return count;
    }

    // Step 2: Perform rotations to balance
    private void PerformRotations(int count)
    {
        Node grandParent = null;
        Node parent = Root;

        for (int i = 0; i < count; i++)
        {
            Node rightChild = parent.Right;
            parent.Right = rightChild.Left;
            rightChild.Left = parent;

            if (grandParent == null)
                Root = rightChild;
            else
                grandParent.Right = rightChild;

            grandParent = rightChild;
            parent = rightChild.Right;
        }
    }

    // Main DSW balancing method
    public void Balance()
    {
        int nodeCount = CreateBackbone();

        // Calculate m = 2^floor(log2(n+1)) - 1
        int m = (int)Math.Pow(2, Math.Floor(Math.Log(nodeCount + 1, 2))) - 1;

        PerformRotations(nodeCount - m);

        while (m > 1)
        {
            m /= 2;
            PerformRotations(m);
        }
    }

    // Remove a specific value from the MyBinaryTree
    public void Remove(T value)
    {
        Root = RemoveRecursive(Root, value);
        Balance(); // Rebalance after deletion
    }

    private Node RemoveRecursive(Node root, T value)
    {
        if (root == null) return null;

        if (root.Value.CompareTo(value) > 0)
            root.Left = RemoveRecursive(root.Left, value);
        else if (root.Value.CompareTo(value) < 0)
            root.Right = RemoveRecursive(root.Right, value);
        else
        {
            // Node found
            _count--;
            if (root.Left == null) return root.Right;
            if (root.Right == null) return root.Left;

            // Node with two children: replace with inorder successor
            Node successor = FindMin(root.Right);
            root.Value = successor.Value;
            root.Right = RemoveRecursive(root.Right, successor.Value);
        }
        return root;
    }

    private Node FindMin(Node root)
    {
        while (root.Left != null)
            root = root.Left;
        return root;
    }
    //T? FindBy<K>(K key, Func<T, K, int> comparer);
    public bool TryFindBy<K>(K key, Func<T, K, int> comparer, out T? result)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        Node? current = Root;



        // comparisons looop. 
        if (current != null)
        {
            
            (bool, T) value;
            if (comparer(current.Value, key) == 0)
            {
                result = current.Value;
                return true;
            }
            if(current.Right != null)
            {
                value = TryFindBy<K>(key, comparer, out result, current.Right);
                if(value.Item1 == true)
                {
                    result = value.Item2;
                    return true;
                }
            }
            if(current.Left != null)
            {
                value = TryFindBy<K>(key, comparer, out result, current.Left);
                if(value.Item1 == true)
                {
                    result = value.Item2;
                    return true;
                }
            }
        }
        result = default;
        return false;
    }

    private (bool, T) TryFindBy<K>(K key, Func<T, K, int> comparer, out T? result, Node current)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        // comparisons looop. 
        if (current != null)
        {
            
            (bool, T) value;
            if (comparer(current.Value, key) == 0)
            {
                result = current.Value;
                return (true, result);
            }
            if(current.Right != null)
            {
                value = TryFindBy<K>(key, comparer, out result, current.Right);
                if(value.Item1 == true)
                {
                    result = value.Item2;
                    return (true, result);
                }
            }
            if(current.Left != null)
            {
                value = TryFindBy<K>(key, comparer, out result, current.Left);
                if(value.Item1 == true)
                {
                    result = value.Item2;
                    return (true, result);
                }
            }
        }
        result = default;
        return (false, result);
    }
    
    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        if (predicate == null)
        throw new ArgumentNullException(nameof(predicate));

        MyBinaryTree<T> result = new MyBinaryTree<T>();

        Node? current = Root;


        // zo goed als zelfde als findby. maar dan met predicate. 
        if (current != null)
        {
            if (predicate(current.Value))
                result.Add(current.Value);

            if(current.Right != null)
            {
                result = Filter(predicate, current.Right, result);
            }
            if(current.Left != null)
            {
                result = Filter(predicate, current.Left, result);
            }
        }

        return result;
    }

    private MyBinaryTree<T> Filter(Func<T, bool> predicate, Node current, MyBinaryTree<T> result)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        // comparisons looop. 
        if (current != null)
        {
            
            if (predicate(current.Value))
                result.Add(current.Value);

            if(current.Right != null)
            {
                result = Filter(predicate, current.Right, result);
            }
            if(current.Left != null)
            {
                result = Filter(predicate, current.Left, result);
            }
        }
        return result;
    }

    
    private void InOrder(Node node, Action<Node> action)
    {
        if (node == null) return;
        InOrder(node.Left, action);
        action(node);
        InOrder(node.Right, action);
    }

    // Bubble sort directly on the tree nodes
    public void Sort(Comparison<T> comparison)
    {
        if (Root == null) return;

        bool swapped;
        do
        {
            swapped = false;
            Node prev = null;
            Node current = null;

            // Traverse in-order and compare adjacent nodes
            InOrder(Root, node =>
            {
                if (current == null)
                {
                    current = node;
                }
                else
                {
                    prev = current;
                    current = node;

                    if (comparison(prev.Value, current.Value) > 0)
                    {
                        // Swap values
                        T temp = prev.Value;
                        prev.Value = current.Value;
                        current.Value = temp;
                        swapped = true;
                    }
                }
            });

        } while (swapped);
    }

    private Node FindSuccessor(Node root, Node target)
    {
        if (target == null) return null;

        // Case 1: Right subtree exists → leftmost node in right subtree
        if (target.Right != null)
        {
            Node curr = target.Right;
            while (curr.Left != null)
                curr = curr.Left;
            return curr;
        }

        // Case 2: No right subtree → search from root
        Node successor = null;
        Node ancestor = root;

        while (ancestor != null)
        {
            if (root.Value.CompareTo(target.Value) > 0)
            {
                successor = ancestor; // potential successor
                ancestor = ancestor.Left;
            }
            else if (root.Value.CompareTo(target.Value) < 0)
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

    public T Reduce(Func<T, T, T> accumulator)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));

        if (Root == null)
            throw new InvalidOperationException("Collection is empty");

        T result = Root.Value;
        Node? current = Root.Left;
        if(current != null)
        {
            while (current.Left != null)
            {
                current = current.Left;
            }
        }

        while (current != null)
        {
            result = accumulator(result, current.Value); // info 1 en info 2 door de gegeven functie halen.
            current = FindSuccessor(Root, current);
        }

        return result;
    }
    // or
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));

        R result = initial;
        Node? current = Root;

        while (current != null)
        {
            result = accumulator(result, current.Value);
            current = FindSuccessor(Root, current);
        }

        return result;
    }
    //or
    public RResult Reduce<R, RResult>(R initial, Func<R, T, R> accumulator, Func<R, RResult> resultSelector)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));
        if (resultSelector == null)
            throw new ArgumentNullException(nameof(resultSelector));

        R result = initial;
        Node? current = Root;

        while (current != null)
        {
            result = accumulator(result, current.Value);
            current = FindSuccessor(Root, current);
        }

        return resultSelector(result);
    }
    

    public IMyIterator<T> GetIterator()
    {
        return new MyBinaryTreeIterator<T>(Root); // je hoeft alleen maar de head mee te geven because it references the rest. 
    }

    
    public T[] ToArray()
    {
        T[] arr = new T[_count];
        int i = 0;

        var iterator = GetIterator();
        while (iterator.HasNext())
        {
            arr[i++] = iterator.Next();
        }

        return arr;
    }
}