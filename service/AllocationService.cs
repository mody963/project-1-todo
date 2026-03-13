class AllocationService : IAllocationService
{
    private readonly IAllocationRepository _repository;
    private readonly IMyCollection<Task_Allocation> _Task_Allocations; // change only this line to imycollection

    public AllocationService(IAllocationRepository repository)
    {
        _repository = repository;
        _Task_Allocations = new MyArrayList<Task_Allocation>();
       // _Task_Allocations = _repository.LoadTasks();
       _Task_Allocations = _repository.LoadTaskAllocation();
    }

    //public IEnumerable<Task_Allocation> GetAllTasks() => _Task_Allocations;
    public IMyCollection<Task_Allocation> GetAllAllocations() => _Task_Allocations;


    public void AddAllocation(TaskItem task, Person person)
    {
        
        var newTask_Allocation = new Task_Allocation{Task = task, Person = person};

        _Task_Allocations.Add(newTask_Allocation);
        _repository.SaveTaskAllocations(_Task_Allocations);
    }

 
    public void RemoveAllocation(TaskItem task, Person person)
    {
        if (_Task_Allocations.TryFindBy(task, (item, key) =>
        (item.Task == key && item.Person == person) ? 0 : 1,
        out var allocation))
        {
            _Task_Allocations.Remove(allocation);
            _repository.SaveTaskAllocations(_Task_Allocations);
        }
    }

    public bool CheckIfAllocationExists(TaskItem task, Person person)
    {
        if (_Task_Allocations.TryFindBy(task, (item, key) =>
        (item.Task.Id == key.Id && item.Person.Id == person.Id) ? 0 : 1,
        out var allocation))
        {
            return true;
        }
        return false;
    }



}
