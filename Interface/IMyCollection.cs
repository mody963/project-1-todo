public interface IMyCollection<T> 
{
    void Add(T item);
    void Remove(T item);
    T? FindBy<K>(K key, Func<T, K, bool> comparer);
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    bool Dirty {get; set;}
    R Reduce<R>(Func<R, T, R> accumulator);
    // or
    R Reduce<R>(R initial, Func<R, T, R> accumulator);
    //or
    RResult Reduce<R, RResult>(R initial, Func<R, T, R> accumulator, Func<R, RResult> resultSelector);
    

    IMyIterator<T> GetIterator();
    T[] ToArray();
    
    //IEnumerator<T> GetEnumerator();
    //IMyEnumerator<T> GetMyEnumerator();
}