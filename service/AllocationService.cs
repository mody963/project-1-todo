class AllocationService : IAllocationService
{
    private readonly IAllocationRepository _repository;
    private readonly IMyCollection<Task_Allocation> _Task_Allocations; // change only this line to imycollection

    public AllocationService(IAllocationRepository repository)
    {
        _repository = repository;
        //_Task_Allocations = new MyArrayList<Task_Allocation>();
       // _Task_Allocations = _repository.LoadTasks();
       _Task_Allocations = _repository.LoadTaskAllocation();
    }

    //public IEnumerable<Task_Allocation> GetAllTasks() => _Task_Allocations;
    public IMyCollection<Task_Allocation> GetAllAllocations() => _Task_Allocations;


    public void AddAllocation(TaskItem task, Person person)
    {
        
        var newTask_Allocation = new Task_Allocation{Task = task, Person = person};

        _Task_Allocations.Add(newTask_Allocation);
        
    }

 
    public void RemoveAllocation(TaskItem task, Person person)
    {
        if (_Task_Allocations.TryFindBy(task, (item, key) =>
        (item.Task.Id == key.Id && item.Person.Id == person.Id) ? 0 : 1,
        out var allocation))
        {
            _Task_Allocations.Remove(allocation);
        }
    }
    
    public void RemoveDependantAllocations(TaskItem currenttask, Person person, int id)
    {
        if (_Task_Allocations.TryFindBy(id, (item, key) =>
        (item.Task.dependant != null && currenttask.Id == item.Task.Id && item.Person.Id == person.Id && item.Task.dependant.Id == key) ? 0 : 1,
        out var allocation))
        {
            allocation.Task.dependant = null;
            _repository.SaveTaskAllocations(_Task_Allocations);
        }
    }

    public void UpdateAllocations(TaskItem task, Person person, string description, string priority, string status)
    {
        if (_Task_Allocations.TryFindBy(task, (item, key) =>
        (item.Task.Id == key.Id && item.Person.Id == person.Id) ? 0 : 1,
        out var allocation))
        {
            allocation.Task.Description = description;
            allocation.Task.Priority = priority;
            allocation.Task.Status = status;
        }
    }

    public void UpdateDependantAllocations(TaskItem task, TaskItem currentTask, Person person, string description, string priority, string status)
    {
        if (_Task_Allocations.TryFindBy(task, (item, key) =>
        (item.Task.dependant != null && item.Task.dependant.Id == key.Id && item.Person.Id == person.Id && currentTask.Id == item.Task.Id) ? 0 : 1,
        out var allocation))
        {
            allocation.Task.dependant.Description = description;
            allocation.Task.dependant.Priority = priority;
            allocation.Task.dependant.Status = status;
        }
    }

    public bool CheckIfAllocationExists(TaskItem task, Person person)
    {
        // use the find method on item and then compare the id's if the same return 0 so equal otherwise 1 so not equal and save it in the out variable and return true.
        if (_Task_Allocations.TryFindBy(task, (item, key) =>
        (item.Task.Id == key.Id && item.Person.Id == person.Id) ? 0 : 1,
        out var allocation))
        {
            return true;
        }
        return false;
    }
    public void SaveIfDirty()
    {
        _repository.SaveIfDirty(_Task_Allocations);
    }



}
