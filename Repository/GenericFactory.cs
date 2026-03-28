
public interface ICollectionFactory<T>
{
    IMyCollection<T> Create();
}
public class MyArrayListFactory<T> : ICollectionFactory<T> where T : IEquatable<T>
{
    public IMyCollection<T> Create() => new MyArrayList<T>();
}

public class MyLinkedListFactory<T> : ICollectionFactory<T>
{
    public IMyCollection<T> Create() => new MyLinkedList<T>();
}