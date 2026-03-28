interface ITaskService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<TaskItem> GetAllTasks();

    void AddTask(string description, string priority, TaskItem chosenTask);

    public void UpdateTask(int id, string description, string priority, string status);

    public void UpdateDependantTask(TaskItem task, int id, string description, string priority, string status);
    void RemoveTask(int id);

    public void RemoveDependantTask(int id, TaskItem currenttask);

    void ToggleTaskCompletion(int id);
    void SaveIfDirty();
}