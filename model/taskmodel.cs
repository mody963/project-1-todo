class TaskItem
{
    public int Id { get; set; }
    public required string Description { get; set; }
    
    public string Priority { get; set; }

    private string _status;

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
}