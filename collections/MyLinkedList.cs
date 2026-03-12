// find should find, delete should only delete so find is an helper method. 
// not an empath sorry gang 

class LinkedList<T>: IMyCollection<T>
{

    // data en adress
    private T FirstNode = default(T);
    private T LastNode = default(T);


    // je moet ook de keuzen krijgen om een legen linked list aan te maken. 
    public LinkedList()
    {
    }
    

    // uiteindelijk ook de keuzen om een gevulde linked list aan te maken. 
    // public LinkedList(hoeveel items en de items zelf)
    // kijken of hoeveelheid items null is zo ja dan argument exception.
    // via de add de items toevoegen. 

    public LinkedList(IEnumerable<T> CollectionYouWantToAdd)
    {
        
    }


    public void Add(T item) // add wordt gebruikt voor dingen aan het einde toevoegen 
    {
        return;
    }

    public void Insert(T item, int index)// bij insert gaan we vooral dingen toevoegen op basis van index dus eerst loopt het door de ding heen en daarna voegt het het toe en daarna zie uitleg blaadje. 
    {
        
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
    
    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

}

