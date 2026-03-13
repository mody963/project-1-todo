interface IAllocationService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<Task_Allocation> GetAllAllocations();

    void AddAllocation(TaskItem task, Person person);

    void RemoveAllocation(TaskItem task, Person person);

    bool CheckIfAllocationExists(TaskItem task, Person person);

}