// class TaskRepository : ITaskRepository
// {
//     private readonly JsonRepository<TaskItem> _repo;

//     public TaskRepository(string path, ICollectionFactory<TaskItem> factory)
//     {
//         _repo = new JsonRepository<TaskItem>(path, factory);
//     }

//     public IMyCollection<TaskItem> LoadTasks() => _repo.Load();

//     public void SaveTasks(IMyCollection<TaskItem> tasks) => _repo.Save(tasks);

//     public void SaveIfDirty(IMyCollection<TaskItem> tasks) => _repo.SaveIfDirty(tasks);
// }
using System.Text.Json;
// Aimee
class TaskRepository : ITaskRepository
{
    private readonly string _directoryPath;
    private readonly ICollectionFactory<TaskItem> _factory;

    public TaskRepository(string path, ICollectionFactory<TaskItem> factory)
    {
        _directoryPath = path;
        _factory = factory;
        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }
    }

    public IMyCollection<TaskItem> LoadTasks()
    {
        var collection = _factory.Create();
        
        // Zoek alle .json bestanden in de map
        var files = Directory.GetFiles(_directoryPath, "*.json");

        foreach (var file in files)
        {
            try 
            {
                string jsonString = File.ReadAllText(file);
                var task = JsonSerializer.Deserialize<TaskItem>(jsonString);
                if (task != null)
                {
                    collection.Add(task);
                }
            }
            catch 
            {
                Console.WriteLine($"[ERROR] Fout bij het laden van bestand: {file}");
            }
        }
        return collection;
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        var iterator = tasks.GetIterator();
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            string safeDesc = System.Text.RegularExpressions.Regex.Replace(task.Description, @"[^a-zA-Z0-9]", "_");
            // Bestandsnamen mogen vaak geen spaties of speciale tekens bevatten. 
            // Deze regel vervangt alles wat geen letter of cijfer is door een underscore (_). 
            // "Was de ramen!" wordt dan "Was_de_ramen_".

            string fileName = $"{task.Id}_{safeDesc}.json";
            string filePath = Path.Combine(_directoryPath, fileName);
            // combineerd de map en de bestandnaam tot een afleesbaar pasd. 

            string jsonString = JsonSerializer.Serialize(task, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        }
    }

    public void SaveIfDirty(IMyCollection<TaskItem> tasks)
    {
        if (tasks.Dirty)
        {
            if (Directory.Exists(_directoryPath))
            {
                var existingFiles = Directory.GetFiles(_directoryPath, "*.json");
                foreach (var file in existingFiles)
                {
                    File.Delete(file);
                }
            }
            SaveTasks(tasks);
            
            tasks.ResetDirty();
        }
    }
}


// mo

class PersonRepository : IPersonRepository
{
    private readonly JsonRepository<Person> _repo;

    public PersonRepository(string path, ICollectionFactory<Person> factory)
    {
        _repo = new JsonRepository<Person>(path, factory);
    }

    public IMyCollection<Person> LoadPerson() => _repo.Load();

    public void SavePerson(IMyCollection<Person> persons) => _repo.Save(persons);

    public void SaveIfDirty(IMyCollection<Person> persons) => _repo.SaveIfDirty(persons);
}


class AllocationRepository : IAllocationRepository
{
    private readonly JsonRepository<Task_Allocation> _repo;

    public AllocationRepository(string path, ICollectionFactory<Task_Allocation> factory)
    {
        _repo = new JsonRepository<Task_Allocation>(path, factory);
    }

    public IMyCollection<Task_Allocation> LoadTaskAllocation() => _repo.Load();

    public void SaveTaskAllocations(IMyCollection<Task_Allocation> allocations) => _repo.Save(allocations);

    public void SaveIfDirty(IMyCollection<Task_Allocation> allocations) => _repo.SaveIfDirty(allocations);
}


