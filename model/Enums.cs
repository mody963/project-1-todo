// Aimee
using System.Text.Json;
using System.Text.Json.Serialization;

public class TaskPriorityConverter : JsonConverter<TaskPriority>
{
    public override TaskPriority Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();
        return value switch
        {
            "must have" => TaskPriority.MustHave,
            "should have" => TaskPriority.ShouldHave,
            "could have" => TaskPriority.CouldHave,
            _ => throw new JsonException($"Invalid TaskPriority value: {value}")
        };
    }

    public override void Write(Utf8JsonWriter writer, TaskPriority value, JsonSerializerOptions options)
    {
        string str = value switch
        {
            TaskPriority.MustHave => "must have",
            TaskPriority.ShouldHave => "should have",
            TaskPriority.CouldHave => "could have",
            _ => throw new JsonException($"Invalid TaskPriority value: {value}")
        };
        writer.WriteStringValue(str);
    }
}

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

