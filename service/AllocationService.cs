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


    public void AddAllocation(int id, int id2)
    {
        
        var newTask_Allocation = new Task_Allocation(id, id2);

        _Task_Allocations.Add(newTask_Allocation);
        _repository.SaveTaskAllocations(_Task_Allocations);
    }

 
    public void RemoveAllocation(int id, int id2)
    {
        if (_Task_Allocations.TryFindBy(id, (item, key) =>
        (item.Task_Id == key && item.Person_Id == id2) ? 0 : 1,
        out var task))
        {
            _Task_Allocations.Remove(task);
            _repository.SaveTaskAllocations(_Task_Allocations);
        }
    }



}
