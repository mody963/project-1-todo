interface IAllocationService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<Task_Allocation> GetAllAllocations();

    void AddAllocation(TaskItem task, Person person);

    void RemoveAllocation(TaskItem task, Person person);

    public void RemoveDependantAllocations(TaskItem currenttask, Person person, int id);

    void UpdateAllocations(TaskItem task, Person person, string description, TaskPriority priority, TaskStatus status);

    public void UpdateDependantAllocations(TaskItem task, TaskItem currentTask, Person person, string description, TaskPriority priority, TaskStatus status);

    bool CheckIfAllocationExists(TaskItem task, Person person);
    void SaveIfDirty();

}