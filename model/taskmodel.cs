class TaskItem
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }

    public string Priority { get; set; }

    public string Status { get; set; }

    public DateTime CreationDate { get; set; }
}