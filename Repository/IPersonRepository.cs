interface IPersonRepository
{
    // List<TaskItem> LoadTasks();
    // void SaveTasks(List<TaskItem> tasks);
    IMyCollection<Person> LoadPerson();
    void SavePerson(IMyCollection<Person> persons);
    void SaveIfDirty(IMyCollection<Person> persons);
}