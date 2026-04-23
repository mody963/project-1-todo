// fernando
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
public class TaskItem : IEquatable<TaskItem>, IComparable<TaskItem>
{
    public int Id { get; set; }
    public required string Description { get; set; }
    
    public TaskPriority Priority { get; set; }

    private TaskStatus _status;
    
    [JsonConverter(typeof(CollectionConverter<int>))]
    public IMyCollection<int> dependant{ get; set; }

    public TaskStatus Status{get;set;}

    public bool Completed
    {
        get => _status == TaskStatus.Completed;
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