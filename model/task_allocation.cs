public class Task_Allocation : IEquatable<Task_Allocation> 
{
    public int Task_Id{get;}
    public int Person_Id{get;}
    public bool Equals(Task_Allocation? other) => other is not null && other.Task_Id == Task_Id && other.Person_Id == Person_Id;

    public override bool Equals(object? obj) => Equals(obj as Task_Allocation);
} //