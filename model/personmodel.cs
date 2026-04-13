// fernando

public class Person : IEquatable<Person>, IComparable<Person>
{
    public int Id{get; set;}
    public string Name{get; set;}

    // public Person(int id, string name)
    // {
    //     Id = id;
    //     Name = name;
    // }

    // aimee
    public bool Equals(Person? other) => other is not null && other.Id == Id;

    public override bool Equals(object? obj) => Equals(obj as Person);

    public int CompareTo(Person other)
    {
        if (other == null) return 1; // Current object is greater than null

        // Compare salaries (ascending order)
        return this.Id.CompareTo(other.Id);
    }


}