public interface IMyCollection<T> 
{
    void Add(T item);
    void Remove(T item);
    T? FindBy<K>(K key, Func<T, K, bool> comparer);
    IMyCollection<T> Filter(Func<T, bool> predicate);
    void Sort(Comparison<T> comparison);
    int Count { get; }
    bool Dirty {get; set;}
    //R Reduce<R>(Func<R, T, R> accumulator);
    // OR
    R Reduce<R>(R initial, Func<R, T, R> accumulator);
    IMyIterator<T> GetIterator(); // Custom Iterator - Since we
    //are not using System.Collections.Generic
   // IEnumerator<T> GetEnumerator(); // Extra foreach lookup.     IS THIS ALLOWED SIR? ITS A BUILT IN THINGY SHOULD WE MAKE IMYENUMERATOR?
}