interface IAllocationService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<Task_Allocation> GetAllAllocations();

    void AddAllocation(TaskItem task, Person person);

    void RemoveAllocation(TaskItem task, Person person);

    void UpdateAllocations(TaskItem task, Person person, string description, string priority, string status);

    public void UpdateDependantAllocations(TaskItem task, Person person, string description, string priority, string status);

    bool CheckIfAllocationExists(TaskItem task, Person person);
    void SaveIfDirty();

}