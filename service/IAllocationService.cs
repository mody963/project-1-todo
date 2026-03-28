interface IAllocationService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<Task_Allocation> GetAllAllocations();

    void AddAllocation(TaskItem task, Person person);

    void RemoveAllocation(TaskItem task, Person person);

    public void RemoveDependantAllocations(TaskItem currenttask, Person person, int id);

    void UpdateAllocations(TaskItem task, Person person, string description, string priority, string status);

    public void UpdateDependantAllocations(TaskItem task, TaskItem currentTask, Person person, string description, string priority, string status);

    bool CheckIfAllocationExists(TaskItem task, Person person);

}