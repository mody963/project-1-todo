interface IAllocationRepository
{
    // List<TaskItem> LoadTasks();
    // void SaveTasks(List<TaskItem> tasks);
    IMyCollection<Task_Allocation> LoadTaskAllocation();
    void SaveTaskAllocations(IMyCollection<Task_Allocation> taskallocations);
}