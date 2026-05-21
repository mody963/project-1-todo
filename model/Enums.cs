// Aimee
using System.Text.Json;
using System.Text.Json.Serialization;


// priority omzetters
public class TaskPriorityConverter : JsonConverter<TaskPriority> // o
{
    // Read: leest een string uit JSON (reader.GetString()) en mapt "must have"|"should have"|"could have" naar de overeenkomstige TaskPriority-enumwaarde; bij onbekende waarden gooit hij een JsonException.. 
    public override TaskPriority Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) // utf8 lezer leest de 
    {
        string? value = reader.GetString(); // leest het af als een string en zet het om naar de enum waarde.
        return value switch
        {
            "must have" => TaskPriority.MustHave,
            "should have" => TaskPriority.ShouldHave,
            "could have" => TaskPriority.CouldHave,
            _ => throw new JsonException($"Invalid TaskPriority value: {value}")
        };
    }
// Write: zet een TaskPriority om naar dezelfde leesbare string en schrijft die naar JSON.
    public override void Write(Utf8JsonWriter writer, TaskPriority value, JsonSerializerOptions options)
    {
        string str = value switch // zet het om naar strings. 
        {
            TaskPriority.MustHave => "must have",
            TaskPriority.ShouldHave => "should have",
            TaskPriority.CouldHave => "could have",
            _ => throw new JsonException($"Invalid TaskPriority value: {value}")
        };
        writer.WriteStringValue(str);
    }
}


// sttatus omzetter zefde als bij de gene hierboven maar dan stat. 
public class TaskStatusConverter : JsonConverter<TaskStatus>
{
    public override TaskStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "to do" => TaskStatus.todo,
            "in progress" => TaskStatus.InProgress,
            "completed" => TaskStatus.Completed,
            _ => throw new JsonException($"Invalid TaskStatus value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, TaskStatus value, JsonSerializerOptions options)
    {
        string str = value switch
        {
            TaskStatus.todo => "to do",
            TaskStatus.InProgress => "in progress",
            TaskStatus.Completed => "completed",
            _ => throw new JsonException($"Invalid TaskStatus value: {value}")
        };
        writer.WriteStringValue(str);
    }
}

[JsonConverter(typeof(TaskPriorityConverter))]
public enum TaskPriority
{
    MustHave,
    ShouldHave,
    CouldHave,
}

[JsonConverter(typeof(TaskStatusConverter))]
public enum TaskStatus
{
    todo,
    InProgress,
    Completed
}

// public class CollectionConverter<T> : JsonConverter<IMyCollection<T>> where T : IEquatable<T>, IComparable<T>
// {
//     public override IMyCollection<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         // Parse into JsonDocument to inspect
//         using var doc = JsonDocument.ParseValue(ref reader);
//         var root = doc.RootElement;

//         // Type discriminator
//         if (!root.TryGetProperty("Type", out var typeProp))
//             throw new JsonException("Missing Type discriminator.");

//         var typeName = typeProp.GetString();

//         return typeName switch
//         {
//             nameof(MyArrayList<T>) when typeof(T) == typeof(int) =>
//                 (IMyCollection<T>)JsonSerializer.Deserialize<MyArrayList<T>>(root.GetRawText(), options)!,

//             nameof(MyLinkedList<T>) when typeof(T) == typeof(string) =>
//                 (IMyCollection<T>)JsonSerializer.Deserialize<MyLinkedList<T>>(root.GetRawText(), options)!,

//             _ => throw new JsonException($"Unknown type: {typeName}")
//         };
//     }

//     public override void Write(Utf8JsonWriter writer, IMyCollection<T> value, JsonSerializerOptions options)
//     {
//         var typeName = value.GetType().Name;

//         // Serialize with type discriminator
//         var json = JsonSerializer.Serialize(value, value.GetType(), options);
//         using var doc = JsonDocument.Parse(json);

//         writer.WriteStartObject();
//         writer.WriteString("Type", typeName);

//         foreach (var prop in doc.RootElement.EnumerateObject())
//             prop.WriteTo(writer);

//         writer.WriteEndObject();
//     }
// }

public class CollectionConverter<T> : JsonConverter<IMyCollection<T>> where T : IEquatable<T>, IComparable<T>
{
    public override IMyCollection<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        string? typeName = null;
        if (root.TryGetProperty("Type", out var typeProp))
            typeName = typeProp.GetString();

        // If no Type property, infer from T
        if (string.IsNullOrWhiteSpace(typeName))
        {
            if (typeof(T) == typeof(int))
                return (IMyCollection<T>)JsonSerializer.Deserialize<MyArrayList<T>>(root.GetRawText(), options)!;
            if (typeof(T) == typeof(Person))
                return (IMyCollection<T>)JsonSerializer.Deserialize<MyLinkedList<T>>(root.GetRawText(), options)!;
            if (typeof(T) == typeof(TaskItem))
                return (IMyCollection<T>)JsonSerializer.Deserialize<MyBinaryTree<T>>(root.GetRawText(), options)!;
            if (typeof(T) == typeof(Task_Allocation))
                return (IMyCollection<T>)JsonSerializer.Deserialize<AllocationHashMapCollection>(root.GetRawText(), options)!;

            throw new JsonException($"Cannot infer type for generic parameter {typeof(T).Name}");
        }

        // If Type property exists, use it
        var targetType = Type.GetType(typeName, throwOnError: true);
        return (IMyCollection<T>)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), targetType, options);
    }

    public override void Write(Utf8JsonWriter writer, IMyCollection<T> value, JsonSerializerOptions options)
    {
        var typeName = value.GetType().AssemblyQualifiedName;
        var json = JsonSerializer.Serialize(value, value.GetType(), options);
        using var doc = JsonDocument.Parse(json);

        writer.WriteStartObject();
        writer.WriteString("Type", typeName);

        foreach (var prop in doc.RootElement.EnumerateObject())
            prop.WriteTo(writer);

        writer.WriteEndObject();
    }
}