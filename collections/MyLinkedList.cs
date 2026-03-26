// find should find, delete should only delete so find is an helper method. 

class MyLinkedList<T>: IMyCollection<T>
{
    // could be both singly or double depends what we like to do. 


    // internal betekent dat het alleen binnen dezelfde map gebruikt kan worden. geen internal meer nodig toestemming docent. 
    public class Node<T> // reden wrm is omdat my linked list iterator het nodig had. 
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


    // data
    private Node<T>? _head;
    //adress 
    private Node<T>? _tail;
    private int _count;
    // public int Count => _count;
    // what does dirty do?
    // deserialization is wnr je de list afleest en het dan in een json file gaat zetten. 

    public bool Dirty { get; private set; }

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
        ArgumentNullException.ThrowIfNull(CollectionYouWantToAdd);
        var iterator = CollectionYouWantToAdd.GetIterator();

        while (iterator.HasNext())
        {
            Add(iterator.Next());
        }

    }


    // add wordt gebruikt voor dingen aan het einde toevoegen 
    // noemen het gwn add wegens de interface.
    public void Add(T item) 
    {
        if (item == null && default(T) == null)
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
        Dirty = true;
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
        Dirty = true;
    }

    
    // bij insert gaan we vooral dingen toevoegen op basis van index dus eerst loopt het door de ding heen 
    // en daarna voegt het het toe en daarna zie uitleg blaadje.
    public void Insert(T item, int index) 
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (index == _count)
        {
            Add(item);
            return;
        }
        if (index < 0 || index > _count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        
        if (index == 0) // als je aan het begin wil zetten automatisch add fist. 
        {
            AddFirst(item);
            return;
        }

        Node<T> newNode = new Node<T>(item); // nu is de next ofc nog null
        Node<T>? current = _head;

        for (int i = 0; i < index - 1; i++) // -1 zodat het op de plek van de index uitkomt en niet erna. 
        {
            current = current.Next;
        }

        newNode.Next = current.Next; // de nieuwe node wijst nu naar het adress waar de vorige naar wees. 
        current.Next = newNode; // het adress van de current node wijst nu naar de nieuwe. 

        if (newNode.Next == null)
            _tail = newNode;

        _count++;
        Dirty = true;
    }


    public void Remove(T item) // hierbij geeft het gewoon de naam van de item mee en dan pakt hij het meteen en delete hij het. 
    {
        if (_head == null)
        return;

        if (_head.Data.Equals(item))
        {
            _head = _head.Next;

            if (_head == null)
                _tail = null;

            _count--;
            return;
        }

        Node<T>? current = _head;

        while (current.Next != null)
        {
            if (current.Next.Data.Equals(item)) // mag niet dezelfde data hebben vandaar de equals. 
            {
                current.Next = current.Next.Next;

                if (current.Next == null)
                    _tail = current;

                _count--;
                return;
            }

            current = current.Next;
        }
        Dirty = true;
    }


    // list.FindBy("John", (student, name) => student.Name == name); vb van input. 
    public T? FindBy<K>(K key, Func<T, K, bool> comparer)
    {
        if (comparer == null)
        {
            throw new ArgumentNullException(nameof(comparer));
        }

        Node<T>? current = _head;



        // comparisons looop. 
        while (current != null)
            {
                if (comparer(current.Data, key))
                    return current.Data;

                current = current.Next;
            }

            return default;
    }


    public IMyCollection<T> Filter(Func<T, bool> predicate)
    {
        if (predicate == null)
        throw new ArgumentNullException(nameof(predicate));

        MyLinkedList<T> result = new MyLinkedList<T>();

        Node<T>? current = _head;


        // zo goed als zelfde als findby. maar dan met predicate. 
        while (current != null)
        {
            if (predicate(current.Data))
                result.Add(current.Data);

            current = current.Next;
        }

        return result;
    }




    // gebruik gemaakt van bubblesort. 
    public void Sort(Comparison<T> comparison)
    {
        if (comparison == null)
        throw new ArgumentNullException(nameof(comparison));

        if (_head == null || _head.Next == null)
            return;

        bool swapped; 

        do
        {
            swapped = false;
            Node<T>? current = _head;

            while (current.Next != null)
            {
                if (comparison(current.Data, current.Next.Data) > 0)
                {
                    // swap data
                    T temp = current.Data;
                    current.Data = current.Next.Data;
                    current.Next.Data = temp;

                    swapped = true;
                }

                current = current.Next;
            }

        } while (swapped);
    }


    public int Count { get; }
    

    public T Reduce(Func<T, T, T> accumulator)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));

        if (_head == null)
            throw new InvalidOperationException("Collection is empty");

        T result = _head.Data;
        Node<T>? current = _head.Next;

        while (current != null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }

        return result;
    }
    
    
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));

        R result = initial;
        Node<T>? current = _head;

        while (current != null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }

        return result;
    }
    
    
    public RResult Reduce<R, RResult>(R initial, Func<R, T, R> accumulator, Func<R, RResult> resultSelector)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));
        if (resultSelector == null)
            throw new ArgumentNullException(nameof(resultSelector));

        R result = initial;
        Node<T>? current = _head;

        while (current != null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }

        return resultSelector(result);
    }
    

    public IMyIterator<T> GetIterator()
    {
        return new MyLinkedListIterator<T>(_head);
    }

    
    public T[] ToArray()
    {
        throw new NotImplementedException();
    }
    
    // public IEnumerator<T> GetEnumerator()
    // {
    //     throw new NotImplementedException();
    // }

    public bool TryFindBy<K>(K key, Func<T, K, int> comparer, out T? result)
    {
        if (comparer == null)
        throw new ArgumentNullException(nameof(comparer));

        Node<T>? current = _head;

        while (current != null)
        {
            if (comparer(current.Data, key) == 0)
            {
                result = current.Data;
                return true;
            }

            current = current.Next;
        }

        result = default;
        return false;
    }
}




