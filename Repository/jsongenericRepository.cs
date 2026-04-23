using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


// mo
class JsonRepository<T> : IJsonRepository<T> where T : IEquatable<T>, IComparable<T>
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
        options.Converters.Add(new CollectionConverter<T>());

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
        options.Converters.Add(new CollectionConverter<T>());
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

// Generic converter for interface serialization/deserialization
// public class InterfaceConverter<TInterface, TImplementation> : JsonConverter<TInterface>
//     where TImplementation : TInterface, new()
// {
//     public override TInterface Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         // Deserialize JSON into the concrete type
//         return JsonSerializer.Deserialize<TImplementation>(ref reader, options);
//     }

//     public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
//     {
//         // Serialize using the concrete type
//         JsonSerializer.Serialize(writer, (TImplementation)value, options);
//     }
// }
/*.   */