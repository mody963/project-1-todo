interface ITaskRepository
{
    // List<TaskItem> LoadTasks();
    // void SaveTasks(List<TaskItem> tasks);
    IMyCollection<TaskItem> LoadTasks();
    void SaveTasks(IMyCollection<TaskItem> tasks);
}