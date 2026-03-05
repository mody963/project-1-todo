public class Person : IEquatable<Person> 
{
    public int Id{get;}
    public string Name{get; private set;}

    public Person(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public bool Equals(Person? other) => other is not null && other.Id == Id;

    public override bool Equals(object? obj) => Equals(obj as Person);


}