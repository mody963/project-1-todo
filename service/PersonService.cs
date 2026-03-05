class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;
    private readonly IMyCollection<Person> _persons; // change only this line to imycollection

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
        _persons = new MyArrayList<Person>();
       // _persons = _repository.LoadTasks();
       _persons = _repository.LoadPerson();
    }

    //public IEnumerable<Person> GetAllTasks() => _persons;
    public IMyCollection<Person> GetAllPersons() => _persons;


    public void AddPerson(string name)
    {
        int newId = 1;
        var iterator = _persons.GetIterator();
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if (task.Id >= newId)
                newId = task.Id + 1;
        }
        var newPerson = new Person(newId, name);

        _persons.Add(newPerson);
        _repository.SavePerson(_persons);
    }

 
    public void RemovePerson(int id)
    {
        var task = _persons.FindBy(id, (item, key) => item.Id == key);

        if (task != null)
            _persons.Remove(task);
            _repository.SavePerson(_persons);
    }



}
