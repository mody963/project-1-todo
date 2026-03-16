// find should find, delete should only delete so find is an helper method. 

class MyLinkedList<T>: IMyCollection<T>
{
    // data
    private Node<T>? _head;
    //adress 
    private Node<T>? _tail;
    private int _count;

    // what does dirty do?




    // je moet ook de keuzen krijgen om een legen linked list aan te maken. 
    public MyLinkedList()
    {
    }
    

    // uiteindelijk ook de keuzen om een gevulde linked list aan te maken. 
    // public LinkedList(hoeveel items en de items zelf)
    // kijken of hoeveelheid items null is zo ja dan argument exception.
    // via de add de items toevoegen. 
    public MyLinkedList(IMyCollection<T>? CollectionYouWantToAdd)
    {
        // willen we het accepteren als het leeg is en dan vullen met default waardes of dat nie?
        if (CollectionYouWantToAdd == null)
        {
            ArgumentNullException.ThrowIfNull(CollectionYouWantToAdd);
        }

        var iterator = CollectionYouWantToAdd.GetIterator();

        while (iterator.HasNext())
        {
            Add(iterator.Next());
        }

    }


    public void Add(T item) // add wordt gebruikt voor dingen aan het einde toevoegen 
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        Node<T> newNode = new Node<T>(item);

        if (_head == null)
        {
            _head = newNode;
            _tail = newNode; // dit moet zo zijn omdat je dan aangeeft dat dit zowel heet begin als het einde is van de linked list.
        }
        else
        {
            _tail.Next = newNode; // de oude tail wordt de nieuwe node met info. de head verander je niet. 
            _tail = newNode;
        }

    _count++;
}

    public void AddBefore(T item, int index) // bij add before gaan we vooral dingen toevoegen op basis van index dus eerst loopt het door de ding heen en daarna voegt het het toe en daarna zie uitleg blaadje. 
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (index < 0 || index > _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        
        // if (index == 0)
        // {
        //     linkedlist.Add(item);
        // }
        // else if (index == _count)
        // {
        //     linkedlist.Add(item);
        // }
        // else
        // {
        //     var currentNode = linkedlist.First;
        //     for (int i = 0; i < index - 1; i++)
        //     {
        //         currentNode = currentNode.Next;
        //     }
        //     linkedlist.AddBefore(currentNode, item);
        // }
        
        // _count++;
    }

    public void AddAfter(T item, int index) // bij add after gaan we vooral dingen toevoegen op basis van index dus eerst loopt het door de ding heen en daarna voegt het het toe en daarna zie uitleg blaadje. 
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (index < 0 || index > _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        
        
    }

    public void AddFirst(T item) // add first wordt gebruikt voor dingen aan het begin toevoegen 
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        Node<T> newNode = new Node<T>(item);
        newNode.Next = _head; // hierbij verwijst de nieuwe node naar de oude head. en dus komt het ervoor. 
        _head = newNode; // hierna vervangt de nieuwe node de oude head.

        if (_tail == null)
        _tail = newNode;

        _count++;

    }

    public void Insert(T item, int index)// bij insert gaan we vooral dingen toevoegen op basis van index dus eerst loopt het door de ding heen en daarna voegt het het toe en daarna zie uitleg blaadje. 
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (index < 0 || index > _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        
        // if (index == 0)
        // {
        //     linkedlist.Add(item);
        // }
        // else if (index == _count)
        // {
        //     linkedlist.AddLast(item);
        // }
        // else
        // {
        //     var currentNode = linkedlist.First;
        //     for (int i = 0; i < index - 1; i++)
        //     {
        //         currentNode = currentNode.Next;
        //     }
        //     linkedlist.AddAfter(currentNode, item);
        // }
        
        // _count++;
        
    }


    public void Remove(T item) // hierbij geeft het gewoon de naam van de item mee en dan pakt hij het meteen en delete hij het. 
    {
        return;
    }
    

    public T? FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }
        
        // for (int i = 0; i < _count; i++)
        // {
        //     if (comparer(_items[i], key)) return _items[i];
        // }
            
        return default(T);
    }


    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        throw new NotImplementedException();
    }


    public void Sort(Comparison<T> comparison)
    {
        throw new NotImplementedException();
    }


    public int Count { get; }
    
    
    public bool Dirty {get;} // was get set maar ik heb alleen get van gemaakt, omdat set private moet zijn
    
    
    public T Reduce(Func<T, T, T> accumulator)
    {
        throw new NotImplementedException();
    }
    // or
    
    
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        throw new NotImplementedException();
    }
    //or
    
    
    public RResult Reduce<R, RResult>(R initial, Func<R, T, R> accumulator, Func<R, RResult> resultSelector)
    {
        throw new NotImplementedException();
    }
    

    public IMyIterator<T> GetIterator()
    {
        throw new NotImplementedException();
    }
    public T[] ToArray()
    {
        throw new NotImplementedException();
    }
    
    // public IEnumerator<T> GetEnumerator()
    // {
    //     throw new NotImplementedException();
    // }

    public bool TryFindBy<K>(K key, Func<T, K, bool> comparer, out T? result)
    {
        throw new NotImplementedException();
    }
}


class Node<T>
{
    // data
    public T Data;
    // adress
    public Node<T>? Next;

    public Node(T data)
    {
        Data = data;
        Next = null;
    }
}

