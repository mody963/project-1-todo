public class Person
{
    public int Id{get;}
    public string Name{get; private set;}

    public Person(int id, string name)
    {
        Id = id;
        Name = name;
    }

}