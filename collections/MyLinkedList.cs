// find should find, delete should only delete so find is an helper method. 
// Aimee



class MyLinkedList<T>: IMyCollection<T>
{
    // could be both singly or double depends what we like to do. 


    // internal betekent dat het alleen binnen dezelfde map gebruikt kan worden. geen internal meer nodig toestemming docent. 
    public class Node 
    {
        // data
        public T Data;
        // adress
        public Node? Next;

        public Node? Previous; // voor doubly referencie naar de vorrige. 

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }


    // data
    private Node? _head;
    //adress 
    private Node? _tail;
    private int _count;
    public int Count => _count;

    public bool Dirty { get; private set; }

    public void ResetDirty()
    {
        Dirty = false;
    }
    // what does dirty do?
    // dirty is een boolean die aangeeft of er iets veranderd is aan de linked list sinds de laatste keer dat het opgeslagen is.
    // deserialization is wnr je de list afleest en het dan in een json file gaat zetten. 

    

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

        Node newNode = new Node(item);

        if (_head == null || _tail == null)
        {
            _head = newNode;
            _tail = newNode; // dit moet zo zijn omdat je dan aangeeft dat dit zowel heet begin als het einde is van de linked list.
        }
        else
        {
            _tail.Next = newNode; // de oude tail wordt de nieuwe node met info. de head verander je niet. 
            newNode.Previous = _tail; // neiuwe node naar oude tail. 
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
        Node newNode = new Node(item);
        newNode.Next = _head; // hierbij verwijst de nieuwe node naar de oude head. en dus komt het ervoor. 
        if (_head != null)
            _head.Previous = newNode;
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

        Node newNode = new Node(item); // nu is de next ofc nog null
        Node? current = _head;

        
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

        

        for (int i = 0; current!=null && i < index - 1 ; i++, current = current.Next) ;// -1 zodat het op de plek van de index uitkomt en niet erna. 
        // {

        //     current = current.Next;
        // }


        if (current == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        newNode.Next = current.Next; // de nieuwe node wijst nu naar het adress waar de vorige naar wees. 
        newNode.Previous = current; // de pev die wordt dus de current want dat was de oude. 

        if (current.Next != null)
            current.Next.Previous = newNode; // doubly linked list update

        current.Next = newNode; // het adress van de current node wijst nu naar de nieuwe. 

        if (newNode.Next == null)
            _tail = newNode;

        _count++;
        Dirty = true;
    }


    public void Remove(T item) // hierbij geeft het gewoon de naam van de item mee en dan pakt hij het meteen en delete hij het. 
    {
        if (_head == null ||_head.Data == null || item == null)
        return;

        if (_head.Data.Equals(item))
        {
            _head = _head.Next;
            if (_head != null)
                _head.Previous = null; // a;s head nie null is dan moey je hey wel leeg maken. 

            if (_head == null)
                _tail = null;

            _count--;
            return;
        }

        Node? current = _head;

        while (current.Next != null && current.Next.Data != null)
        {
            if (current.Next.Data.Equals(item)) // mag niet dezelfde data hebben vandaar de equals. 
            {
                Node? nodeToRemove = current.Next;
                current.Next = nodeToRemove.Next;

                if (nodeToRemove.Next != null)
                    nodeToRemove.Next.Previous = current;

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

        Node? current = _head;



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

        Node? current = _head;


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
    public void Sort(Comparison<T> comparison)  // > than 0 first is larger, 0= the same < 0 first is smaller.
    {
        if (comparison == null)
        throw new ArgumentNullException(nameof(comparison));

        if (_head == null || _head.Next == null) // check if already sorted or empty.
            return;



        bool swapped; 
        do
        {
            swapped = false;
            Node current = _head;

            while (current.Next != null)
            {
                if (comparison(current.Data, current.Next.Data) > 0) // if current groter dan next dan en dus niet gelijk aan of kleiner dan dn sort. 
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
    

    public T Reduce(Func<T, T, T> accumulator)  // je wil hierbij bijvb de getallen of dingen opellen. 
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));

        if (_head == null)
            throw new InvalidOperationException("Collection is empty");

        T result = _head.Data;
        Node? current = _head.Next;

        while (current != null)
        {
            result = accumulator(result, current.Data); // info 1 en info 2 door de gegeven functie halen.
            current = current.Next;
        }

        return result;
    }
    
    
    public R Reduce<R>(R initial, Func<R, T, R> accumulator)
    {
        if (accumulator == null)
        throw new ArgumentNullException(nameof(accumulator));

        R result = initial;
        Node? current = _head;

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
        Node? current = _head;

        while (current != null)
        {
            result = accumulator(result, current.Data);
            current = current.Next;
        }

        return resultSelector(result);
    }
    

    public IMyIterator<T> GetIterator()
    {
        return new MyLinkedListIterator<T>(_head); // je hoeft alleen maar de head mee te geven because it references the rest. 
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
    
    // public IEnumerator<T> GetEnumerator()
    // {
    //     throw new NotImplementedException();
    // }

    public bool TryFindBy<K>(K key, Func<T, K, int> comparer, out T? result) // als je de variabele niet mee geeft maar aanmaakt in de class dan is het out. 
    {
        if (comparer == null)
        throw new ArgumentNullException(nameof(comparer));

        Node? current = _head;

        while (current != null)
        {

            // rede  waarom is het 0 is omdat het de comparer is dus met die 0, -1 en 1. 
            // als het 0 is dan zijn ze gelijk, als het groter is dan 0 dan is current groter en als het kleiner 
            // is dan 0 dan is current kleiner.
            // de reden dat je het dan ook in result opslaat is.          
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




