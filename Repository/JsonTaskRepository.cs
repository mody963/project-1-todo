using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;

    public JsonTaskRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IMyCollection<TaskItem> LoadTasks()
    {
        var collection = new MyArrayList<TaskItem>();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

        // Deserialize into an array       IK MOET VRAGEN OF DIT MAG MET ARRAYS MAAR VGM KAN HET NIET ANDERS
        TaskItem[] tasks = JsonSerializer.Deserialize<TaskItem[]>(json) ?? new TaskItem[0];

        for (int i = 0; i < tasks.Length; i++)
        {
            collection.Add(tasks[i]);
        }

        return collection;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        // helper method to convert IMyCollection into an array
        TaskItem[] arr = tasks.ToArray();

        string json = JsonSerializer.Serialize(arr, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}