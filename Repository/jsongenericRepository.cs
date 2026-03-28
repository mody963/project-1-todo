using System.Text.Json;
class JsonRepository<T> where T : IEquatable<T>
{
    private readonly string _filePath;
    private readonly ICollectionFactory<T> _factory;

    public JsonRepository(string filePath, ICollectionFactory<T> factory)
    {
        _filePath = filePath;
        _factory = factory;
    }

    public IMyCollection<T> Load()
    {
        var collection = _factory.Create();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

        T[] items = JsonSerializer.Deserialize<T[]>(json) ?? new T[0];

        for (int i = 0; i < items.Length; i++)
            collection.Add(items[i]);

        return collection;
    }

    public void Save(IMyCollection<T> collection)
    {
        T[] arr = collection.ToArray();
        string json = JsonSerializer.Serialize(arr, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void SaveIfDirty(IMyCollection<T> collection)
    {
        if (!collection.Dirty)
            return;

        Save(collection);
        collection.ResetDirty();

       
    }
}