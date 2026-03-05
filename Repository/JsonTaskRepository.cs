// using System.Text.Json;
// class JsonTaskRepository : ITaskRepository
// {
//     private readonly string _filePath;

//     public JsonTaskRepository(string filePath)
//     {
//         _filePath = filePath;
//     }

//     public List<TaskItem> LoadTasks()
//     {
//         if (!File.Exists(_filePath))
//         {
//             return new List<TaskItem>();
//         }

//         string json = File.ReadAllText(_filePath);
//         var tasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

//         return tasks ?? new List<TaskItem>();
//     }

//     public void SaveTasks(List<TaskItem> tasks)
//     {
//         string json = JsonSerializer.Serialize(
//             tasks,
//             new JsonSerializerOptions { WriteIndented = true }
//         );

//         File.WriteAllText(_filePath, json);
//     }
// }
using System.Text.Json;

class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private readonly string _filePath2;

    public JsonTaskRepository(string filePath, string filepath2)
    {
        _filePath = filePath;
        _filePath2 = filepath2;
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
        // Use your helper method to convert IMyCollection into an array
        TaskItem[] arr = tasks.ToArray();

        string json = JsonSerializer.Serialize(arr, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
    public IMyCollection<Task_Allocation> LoadAllocation()
    {
        var collection = new MyArrayList<Task_Allocation>();

        if (!File.Exists(_filePath))
            return collection;
        string json = File.ReadAllText(_filePath2);
        Task_Allocation[] tasks = JsonSerializer.Deserialize<Task_Allocation[]>(json) ?? new Task_Allocation[0];
        for (int i = 0; i < tasks.Length; i++)
        {
            collection.Add(tasks[i]);
        }

        return collection;
    }

    public void SaveAllocation(IMyCollection<Task_Allocation> Allocations)
    {
        Task_Allocation [] arr = Allocations.ToArray();
        string json = JsonSerializer.Serialize(arr, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath2, json);
    }
}