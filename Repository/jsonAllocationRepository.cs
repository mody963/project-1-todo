using System.Text.Json;

class JsonAllocationRepository : IAllocationRepository
{
    private readonly string _filePath;
    

    public JsonAllocationRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IMyCollection<Task_Allocation> LoadTaskAllocation()
    {
        var collection = new MyArrayList<Task_Allocation>();

        if (!File.Exists(_filePath))
            return collection;
        string json = File.ReadAllText(_filePath);
        Task_Allocation[] tasks = JsonSerializer.Deserialize<Task_Allocation[]>(json) ?? new Task_Allocation[0];
        for (int i = 0; i < tasks.Length; i++)
        {
            collection.Add(tasks[i]);
        }

        return collection;
    }
    public void SaveTaskAllocations(IMyCollection<Task_Allocation> Allocations)
    {
        Task_Allocation [] arr = Allocations.ToArray();
        string json = JsonSerializer.Serialize(arr, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}