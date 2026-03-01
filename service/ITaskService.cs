interface ITaskService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<TaskItem> GetAllTasks();

    void AddTask(string description, string priority);

    public void UpdateTask(int id, string description, string priority, string status);
    void RemoveTask(int id);

    void ToggleTaskCompletion(int id);
}