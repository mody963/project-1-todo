// mo
public interface ICollectionFactory<T> where T : IEquatable<T>, IComparable<T>
{
    IMyCollection<T> Create();
}
public class MyArrayListFactory<T> : ICollectionFactory<T> where T : IEquatable<T>, IComparable<T>
{
    public IMyCollection<T> Create() => new MyArrayList<T>();
}

public class MyLinkedListFactory<T> : ICollectionFactory<T> where T : IEquatable<T>, IComparable<T>
{
    public IMyCollection<T> Create() => new MyLinkedList<T>();
}

public class MyBinaryTreeFactory<T> : ICollectionFactory<T> where T : IEquatable<T>, IComparable<T>
{
    public IMyCollection<T> Create() => new MyBinaryTree<T>();
}
// public class MyHashMapFactory<T> : ICollectionFactory<T>
// {
//     public IMyCollection<T> Create()
//     {
//         if (typeof(T) == typeof(TaskItem))
//             return (IMyCollection<T>)new TaskHashMapCollection();

//         if (typeof(T) == typeof(Person))
//             return (IMyCollection<T>)new PersonHashMapCollection();

//         if (typeof(T) == typeof(Task_Allocation))
//             return (IMyCollection<T>)new AllocationHashMapCollection();

//         throw new NotSupportedException($"No HashMap wrapper for type {typeof(T)}");
//     }
// }