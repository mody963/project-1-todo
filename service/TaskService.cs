class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks; // change only this line to imycollection

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        _tasks = new MyArrayList<TaskItem>();
       // _tasks = _repository.LoadTasks();
       _tasks = _repository.LoadTasks();
    }

    //public IEnumerable<TaskItem> GetAllTasks() => _tasks;
    public IMyCollection<TaskItem> GetAllTasks() => _tasks;


    // public void AddTask(string description)
    // {
    //     int newId = _tasks.Count > 0
    //         ? _tasks[_tasks.Count - 1].Id + 1
    //         : 1;

    //     var newTask = new TaskItem
    //     {
    //         Id = newId,
    //         Description = description,
    //         Completed = false
    //     };

    //     _tasks.Add(newTask);
    //     _repository.SaveTasks(_tasks);
    // }
    public void AddTask(string description, string priority)
    {
        int newId = 1;
        var iterator = _tasks.GetIterator();
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if (task.Id >= newId)
                newId = task.Id + 1;
        }
        var newTask = new TaskItem
        {
            Id = newId,
            Description = description,
            Priority = priority,
            Status = "to do",
            CreationDate = DateTime.Now
        };

        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }

    public void UpdateTask(int id, string description, string priority, string status)
    {
        var task = _tasks.FindBy(id, (item, key) => item.Id == key);

        if (task != null)
        {
            task.Description = description;
            task.Priority = priority;
            task.Status = status;

            _repository.SaveTasks(_tasks);
        }
    }

    // public void RemoveTask(int id)
    // {
    //     var task = _tasks.Find(t => t.Id == id);

    //     if (task != null)
    //     {
    //         _tasks.Remove(task);
    //         _repository.SaveTasks(_tasks);
    //     }
    // }
    public void RemoveTask(int id)
    {
        var task = _tasks.FindBy(id, (item, key) => item.Id == key);

        if (task != null)
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
    }


    // public void ToggleTaskCompletion(int id)
    // {
    //     var task = _tasks.Find(t => t.Id == id);

    //     if (task != null)
    //     {
    //         task.Completed = !task.Completed;
    //         _repository.SaveTasks(_tasks);
    //     }
    // }
    public void ToggleTaskCompletion(int id)
    {
        var task = _tasks.FindBy(id, (item, key) => item.Id == key);

        if (task != null)
        {
            if (task.Status == "completed")
            {
                task.Status = "to do";
            }
            else
            {
                task.Status = "completed";
            }
            _repository.SaveTasks(_tasks);
        }
    }
}
