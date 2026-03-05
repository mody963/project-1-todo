interface IPersonRepository
{
    // List<TaskItem> LoadTasks();
    // void SaveTasks(List<TaskItem> tasks);
    IMyCollection<Person> LoadPerson();
    void SavePerson(IMyCollection<Person> persons);
}