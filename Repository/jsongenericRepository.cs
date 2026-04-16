using System.Text.Json;
using System.Text.Json.Serialization;


// mo
class JsonRepository<T> : IJsonRepository<T> where T : IEquatable<T>
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

        var options = new JsonSerializerOptions();
        options.Converters.Add(new TaskPriorityConverter());
        options.Converters.Add(new TaskStatusConverter());

        T[] items = JsonSerializer.Deserialize<T[]>(json, options) ?? new T[0];

        for (int i = 0; i < items.Length; i++)
            collection.Add(items[i]);

        return collection;
    }

    public void Save(IMyCollection<T> collection)
    {
        T[] arr = collection.ToArray();
        var options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new TaskPriorityConverter());
        options.Converters.Add(new TaskStatusConverter());
        string json = JsonSerializer.Serialize(arr, options);
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


/*.   */