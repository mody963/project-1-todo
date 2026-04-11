public class Task_Allocation : IEquatable<Task_Allocation>, IComparable<Task_Allocation>
{
    public TaskItem Task{get; set;}
    public Person Person{get; set;}
    // public Task_Allocation(int taskid, int personid)
    // {
    //     Task_Id = taskid;
    //     Person_Id = personid;
    // }
    public bool Equals(Task_Allocation? other) => other is not null && other.Task == Task && other.Person == Person;

    public override bool Equals(object? obj) => Equals(obj as Task_Allocation);

    public int CompareTo(Task_Allocation other)
    {
        if (other.Person == null || other.Task == null) return 1; // Current object is greater than null

        // Compare salaries (ascending order)
        int comparePerson =  this.Person.Id.CompareTo(other.Person.Id);
        if(comparePerson == 0)
        {
            return this.Task.Id.CompareTo(other.Task.Id);
        }
        return comparePerson;
    }
} //