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


