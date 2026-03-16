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
        var newPerson = new Person{ Id = newId, Name = name};

        _persons.Add(newPerson);
        _repository.SavePerson(_persons);
    }

 
    public void RemovePerson(int id)
    {
        if (_persons.TryFindBy(id, (item, key) => item.Id.CompareTo(key), out var person))
        {
            _persons.Remove(person);
            _repository.SavePerson(_persons);
        }
    }



}
