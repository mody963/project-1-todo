interface IJsonRepository<T> where T : IEquatable<T>, IComparable<T>
{
    IMyCollection<T> Load();
    void Save(IMyCollection<T> collection);
}