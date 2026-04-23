// fernando, mo 

class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks; // change only this line to imycollection

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
        //_tasks = new MyArrayList<TaskItem>();
       // _tasks = _repository.LoadTasks();
       _tasks = _repository.LoadTasks();
    }

    //public IEnumerable<TaskItem> GetAllTasks() => _tasks;
    public IMyCollection<TaskItem> GetAllTasks() => _tasks;

    // Aimee
    public void AddTask(string description, TaskPriority priority, MyArrayList<int> chosenTask)
    {
        int newId = 1;
        var iterator = _tasks.GetIterator();
        
        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if (task.Id >= newId)
            {
                newId = task.Id + 1; // om te kijken welke id's er al gebruikt zijn zodat het nooit dubbele id heeft. 
            }
        }

        var newTask = new TaskItem
        {
            Id = newId,
            Description = description,
            Priority = priority,
            Status = TaskStatus.todo,
            CreationDate = DateTime.Now,
            dependant = chosenTask
        };

        _tasks.Add(newTask);
        SaveIfDirty();
    }
    public void UpdateTask(int id, string description, TaskPriority priority, TaskStatus status)
    {
        if (_tasks.TryFindBy(id, (item, key) => item.Id.CompareTo(key), out var task))
        {
            task.Description = description;
            task.Priority = priority;
            task.Status = status;
            _tasks.Dirty = true;
        }
    }

    public void UpdateDependantTask(TaskItem currenttask, int id, string description, TaskPriority priority, TaskStatus status)
    {
        // if (_tasks.TryFindBy(id, (item, key) => (item.dependant != null && item.dependant.Id == key && currenttask.Id == item.Id) ? 0 : 1, out var task))
        // {
        //     task.dependant.Description = description;
        //     task.dependant.Priority = priority;
        //     task.dependant.Status = status;
        // }
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
        if (_tasks.TryFindBy(id, (item, key) => item.Id.CompareTo(key), out var task))
        {
            _tasks.Remove(task);
        }
    }

    public void RemoveDependantTask(int id, TaskItem currenttask)
    {
        if (_tasks.TryFindBy(id, (item, key) => (item.dependant != null && item.dependant.Count != 0 && currenttask.Id == item.Id) ? 0 : 1, out var task))
        {
            var it = task.dependant.GetIterator();
            while(it.HasNext())
            {
                var item_id = it.Next();
                {
                    if(item_id == id)
                    {
                        task.dependant.Remove(item_id);
                    }
                }
            }
        }
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
    public void ToggleTaskCompletion(int id, TaskStatus status)
    {
        if (_tasks.TryFindBy(id, (item, key) => item.Id.CompareTo(key), out var task))
        {
            task.Status = status;
            _tasks.Dirty = true;
        }
    }
    public void SaveIfDirty()
    {
        _repository.SaveIfDirty(_tasks);
    }
}
