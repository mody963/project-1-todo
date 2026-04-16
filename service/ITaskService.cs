interface ITaskService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<TaskItem> GetAllTasks();

    void AddTask(string description, TaskPriority priority, MyArrayList<int> chosenTask);

    public void UpdateTask(int id, string description, TaskPriority priority, TaskStatus status);

    public void UpdateDependantTask(TaskItem task, int id, string description, TaskPriority priority, TaskStatus status);
    void RemoveTask(int id);

    public void RemoveDependantTask(int id, TaskItem currenttask);

    void ToggleTaskCompletion(int id);
    void SaveIfDirty();
}