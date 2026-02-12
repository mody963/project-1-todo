interface ITaskRepository
{
    List<TaskItem> LoadTasks();
    void SaveTasks(List<TaskItem> tasks);
}