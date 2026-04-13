// fernando

public class TaskItem : IEquatable<TaskItem>, IComparable<TaskItem>
{
    public int Id { get; set; }
    public required string Description { get; set; }
    
    public string Priority { get; set; }

    private string _status;
    
    public TaskItem dependant{get; set;}

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
        }
    }

    public bool Completed
    {
        get => _status == "completed";
    }
    public DateTime CreationDate { get; set; }

    public bool Equals(TaskItem? other) => other is not null && other.Id == Id;

    public override bool Equals(object? obj) => Equals(obj as TaskItem);

    public int CompareTo(TaskItem other)
    {
        if (other == null) return 1; // Current object is greater than null

        // Compare salaries (ascending order)
        return this.Id.CompareTo(other.Id);
    }

}