interface IAllocationService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<Task_Allocation> GetAllAllocations();

    void AddAllocation(int id, int id2);

    void RemoveAllocation(int id, int id2);

}