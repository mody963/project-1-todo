interface IJsonRepository<T>
{
    IMyCollection<T> Load();
    void Save(IMyCollection<T> collection);
}