interface IPersonService
{
    // IEnumerable<TaskItem> GetAllTasks();
    
    IMyCollection<Person> GetAllPersons();

    void AddPerson(string name);

    //public void UpdatePerson(int id, string name);
    void RemovePerson(int id);
}