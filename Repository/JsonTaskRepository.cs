using System.Text.Json;
class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;

    public JsonTaskRepository(string filePath)
    {
        _filePath = filePath;
    }

    public List<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new List<TaskItem>();
        }

        string json = File.ReadAllText(_filePath);
        var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

        return tasks ?? new List<TaskItem>();
    }

    public void SaveTasks(List<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(
            tasks,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(_filePath, json);
    }
}
