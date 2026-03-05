using System.Text.Json;

class JsonPersonRepository : IPersonRepository
{
    private readonly string _filePath;

    public JsonPersonRepository(string filePath)
    {
        _filePath = filePath;
    }

    public IMyCollection<Person> LoadPerson()
    {
        var collection = new MyArrayList<Person>();

        if (!File.Exists(_filePath))
            return collection;

        string json = File.ReadAllText(_filePath);

        // Deserialize into an array       IK MOET VRAGEN OF DIT MAG MET ARRAYS MAAR VGM KAN HET NIET ANDERS
        Person[] persons = JsonSerializer.Deserialize<Person[]>(json) ?? new Person[0];

        for (int i = 0; i < persons.Length; i++)
        {
            collection.Add(persons[i]);
        }

        return collection;
    }

    public void SavePerson(IMyCollection<Person> persons)
    {
        // Use your helper method to convert IMyCollection into an array
        Person[] arr = persons.ToArray();

        string json = JsonSerializer.Serialize(arr, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}