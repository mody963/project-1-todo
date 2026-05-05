using Spectre.Console;


class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _taskservice;

    private readonly IPersonService _personservice;

    private readonly IAllocationService _allocationservice;
    private Person? activePerson = null;

    public ConsoleTaskView(ITaskService taskService, IPersonService personService, IAllocationService allocationService)
    {
        _taskservice = taskService;
        _personservice = personService;
        _allocationservice = allocationService;
    }
     
    // Aimee
    // nu krijg je maar 10 tasks per pagina te zien. heb je dr meer dan ga je door naar yt volgende stukkie. 
    private void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        const int pageSize = 10;

        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[red]No tasks found.[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            Console.ReadKey();
            return;
        }


        var taskList = new MyArrayList<TaskItem>();
        while (iterator.HasNext())
        {
            taskList.Add(iterator.Next());
        }

        int currentPage = 0;
        int totalPages = (int)Math.Ceiling(taskList.Count / (double)pageSize);
        
        while (true)
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("TASKS")
                .Color(Color.DarkViolet)
                .Centered());

            // Get tasks for current page
            var pageIterator = taskList.GetIterator();
            var pageTaskList = new MyArrayList<TaskItem>();
            int index = 0;
            int startIndex = currentPage * pageSize;
            int endIndex = startIndex + pageSize;

            while (pageIterator.HasNext())
            {
                var task = pageIterator.Next();
                if (index >= startIndex && index < endIndex)
                {
                    pageTaskList.Add(task);
                }
                index++;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.DarkViolet)
                .AddColumn("[bold]ID[/]")
                .AddColumn("[bold]Description[/]")
                .AddColumn("[bold]Priority[/]")
                .AddColumn("[bold]Status[/]")
                .AddColumn("[bold]Assigned To[/]")
                .AddColumn("[bold]Creation Date[/]")
                .Centered();

            var tableIterator = pageTaskList.GetIterator();
            while (tableIterator.HasNext())
            {
                var t = tableIterator.Next();
                table.AddRow(
                    t.Id.ToString(),
                    t.Description,
                    FormatPriority(t.Priority),
                    FormatStatus(t.Status),
                    GetAssignedPersonName(t.Id),
                    t.CreationDate.ToString("g")
                );
            }

            AnsiConsole.MarkupLine($"[bold cyan]Task List[/] [grey](Page {currentPage + 1} / {totalPages})[/]\n");
            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("\n[grey]← →  Page   Esc: Back[/]");

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (currentPage > 0)
                    {
                        currentPage--;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (currentPage < totalPages - 1)
                    {
                        currentPage++;
                    }
                    break;

                case ConsoleKey.Escape:
                    return;
            }
        }
    }
    private TaskItem ChooseTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();

        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No tasks found.[/]");
            Console.ReadKey();
            return null;
        }

        // Put tasks into MyArrayList
        var taskArray = new MyArrayList<TaskItem>();
        while (iterator.HasNext())
            taskArray.Add(iterator.Next());

        // back for going back
        var back = new TaskItem { Id = -1, Description = "Back" };
        taskArray.Add(back);

        // Spectre selection
        var selectedTask = AnsiConsole.Prompt(
            new SelectionPrompt<TaskItem>()
                .Title("[yellow]Select a task[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Cyan1))
                .UseConverter(task =>
                    task.Id == -1
                        ? "[red]<- Back[/]"
                        : $"[bold]{task.Id}[/]. {task.Description} | {FormatPriority(task.Priority)} | {FormatStatus(task.Status)}")
                .AddChoices(taskArray.ToArray())  // convert MyArrayList to array
        );

        if (selectedTask.Id == -1)
            return null;

        return selectedTask;
    }

    public int ChooseTasks(IMyCollection<TaskItem> tasks, bool check)
    {
        TaskItem task = ChooseTasks(tasks);
        if(task != null)
        {
            return task.Id;
        }
        return 0;
    }

    public static Person Chooseperson(IMyCollection<Person> people)
    {
        Console.Clear();

        var iterator = people.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No people found.[/]");
            Console.ReadKey();
            return null;
        }

        // Put people into MyArrayList
        var peopleArray = new MyArrayList<Person>();
        while (iterator.HasNext())
            peopleArray.Add(iterator.Next());

        // back for going back
        var back = new Person {Id = -1, Name = "Back"};
        peopleArray.Add(back);

        // Spectre selection
        var selectedPerson = AnsiConsole.Prompt(
            new SelectionPrompt<Person>()
                .Title("[yellow]Select a person[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Cyan1))
                .UseConverter(person =>
                    person.Id == -1
                        ? "[red]<- Back[/]"
                        : $"[bold]{person.Id}[/]. {person.Name}")
                .AddChoices(peopleArray.ToArray())  // convert MyArrayList to array
        );

        if (selectedPerson.Id == -1)
            return null;

        return selectedPerson;
    }

    public static int Chooseperson(IMyCollection<Person> tasks, bool check)
    {
        Person person = Chooseperson(tasks);
        if(person != null)
        {
            return person.Id;
        }
        return 0;
    }

    public static Task_Allocation ChooseAllocation(IMyCollection<Task_Allocation> allocations)
    {
        Console.Clear();

        var iterator = allocations.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No allocations found.[/]");
            Console.ReadKey();
            return null;
        }

        // Put tasks into MyArrayList
        var allocation_Array = new MyArrayList<Task_Allocation>();
        while (iterator.HasNext())
            allocation_Array.Add(iterator.Next());

        // back for going back
        var back = new Task_Allocation {Task = null, Person = new()};
        back.Person.Name = "back";
        allocation_Array.Add(back);

        // Spectre selection
        var selectedAllocation = AnsiConsole.Prompt(
            new SelectionPrompt<Task_Allocation>()
                .Title("[yellow]Select an allocation[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Cyan1))
                .UseConverter(allocation =>
                    allocation.Task == null
                        ? "[red]<- Back[/]"
                        : $"[bold]{allocation.Task.Id}[/]. {allocation.Person.Name}: {allocation.Task.Description}")
                .AddChoices(allocation_Array.ToArray())  // convert MyArrayList to array
        );

        if (selectedAllocation.Task == null)
            return null;

        return selectedAllocation;
    }

    private string Prompt(string message)
    {
        return AnsiConsole.Ask<string>($"[green]{message}[/]");
    }

    public void Run()
    {
        Console.Clear();
        SelectPerson();
        while (true)
        {
            Console.Clear();
           
            // 2 options either use like this or use like choosetask where you use custom collection then convert to array, spectreconsole needs ienumerable which isnt implemented
            var option = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[yellow]Choose an option[/]")
                .PageSize(10)
                .HighlightStyle(new Style(Color.Cyan1))
                .AddChoices(new[]
                {
                    "Add Task",
                    "Remove Task",
                    "Update Task",
                    "Toggle Task State",
                    "Assign task",
                    "List Tasks",
                    "Filter Tasks",
                    "Dependency Graph",
                    "Exit"
                }));
            switch (option)
            {
                case "Add Task":
                    string description = Prompt("Enter task description: ");
                    TaskPriority priority = AskPriority();
                    var isItDependant = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("[yellow]is it dependant[/]")
                    .HighlightStyle(new Style(Color.Cyan1))
                    .AddChoices(new[]
                    {
                        "Yes",
                        "No"
                    }));
                    TaskItem chosenTask = null;
                    MyArrayList<int> ids = new();
                    switch (isItDependant)
                    {
                        case "Yes":
                            var list = _taskservice.GetAllTasks();
                            do
                            {
                                chosenTask = ChooseTasks(list);
                                if(chosenTask != null)
                                {
                                    if(!ids.TryFindBy(chosenTask.Id, (item, key) => item.CompareTo(key), out var foundTask))
                                    {
                                        ids.Add(chosenTask.Id);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Already dependant on that task.");
                                        Console.ReadLine();
                                    }
                                }
                            }while(chosenTask != null);
                            break;
                        case "No":
                            break;
                    }
                    _taskservice.AddTask(description, priority, ids);
                    break;

                case "Remove Task":
                    Remove();
                    break;

                case "Update Task":  
                    UpdateTask();
                    break;

                case "Toggle Task State":
                    ToggleTaskCompletion();
                    break;

                case "Assign task":
                    AssignMenu();
                    break;
                case "List Tasks":
                    ListTasksMenu();
                    break;
                case "Filter Tasks":
                    FilterTasks.FiltersTasks(_taskservice.GetAllTasks());
                    break;
                case "Dependency Graph":
                    DisplayDependencyGraph();
                    break;
                case "Exit":
                    SaveIfNeeded();
                    return;
            }
            SaveIfNeeded();
        }
    }
    private void SaveIfNeeded()
    {
        _taskservice.SaveIfDirty();
        _allocationservice.SaveIfDirty();
        _personservice.SaveIfDirty();
    }
    private void ListTasksMenu()
    {
        while (true)
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Task Views[/]")
                    .HighlightStyle(new Style(Color.Cyan1))
                    .AddChoices(new[]
                    {
                        "View All",
                        "View per Status",
                        "View tasks per person",
                        "Back"
                    }));

            switch (option)
            {
                case "View All":
                    DisplayTasks(_taskservice.GetAllTasks());
                    break;

                case "View per Status":
                    DisplayKanbanView();
                    break;

                case "View tasks per person":
                    DisplayTasksPerPerson();
                    break;

                case "Back":
                    return;
            }
        }
    }
    private void DisplayKanbanView()
    {
        Console.Clear();

        var tasks = _taskservice.GetAllTasks();

        var todo = tasks.Filter(t => t.Status == TaskStatus.todo);
        var progress = tasks.Filter(t => t.Status == TaskStatus.InProgress);
        var done = tasks.Filter(t => t.Status == TaskStatus.Completed);

        var todoTable = CreateStatusTable("To Do", todo);
        var progressTable = CreateStatusTable("In Progress", progress);
        var doneTable = CreateStatusTable("Done", done);

        AnsiConsole.Write(
            new Columns(new[] { todoTable, progressTable, doneTable })
        );
        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
    private Table CreateStatusTable(string title, IMyCollection<TaskItem> tasks)
    {
        var table = new Table()
            .Title($"[yellow]{title}[/]")
            .Border(TableBorder.Rounded)
            .AddColumn("[bold]Task[/]");

        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            table.AddRow("[grey]No tasks[/]");
            return table;
        }

        while (iterator.HasNext())
        {
            var t = iterator.Next();

            string person = GetAssignedPersonName(t.Id);

            string card =
                $"[bold]#{t.Id}[/] {t.Description}\n" +
                $"👤 {person}\n" +
                $"{FormatPriority(t.Priority)}\n";

            table.AddRow(card);
        }

        return table;
    }
    private void DisplayTasksPerPerson()
    {
        int personId = Chooseperson(_personservice.GetAllPersons(), true);

        if (personId == 0)
            return;

        var tasks = _taskservice.GetAllTasks();

        var allocations = _allocationservice.GetAllAllocations();

        var filtered = tasks.Filter(t =>
        {
            var it = allocations.GetIterator();

            while (it.HasNext())
            {
                var a = it.Next();

                if (a.Task.Id == t.Id && a.Person.Id == personId)
                    return true;
            }

            return false;
        });

        DisplayTasks(filtered);
    }
    private TaskPriority AskPriority()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<TaskPriority>()
                .Title("[yellow]Select task priority[/]")
                .AddChoices(
                    TaskPriority.MustHave,
                    TaskPriority.ShouldHave,
                    TaskPriority.CouldHave
                ));
    }
    private TaskStatus AskStatus()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<TaskStatus>()
                .Title("[yellow]Select task status[/]")
                .AddChoices(
                    TaskStatus.todo,
                    TaskStatus.InProgress,
                    TaskStatus.Completed
                ));
    }
    
    private IMyCollection<TaskItem> GetUserTasks()
    {
        IMyCollection<Task_Allocation> allocations = _allocationservice.GetAllAllocations();

        if (activePerson == null)
        {
            return new MyArrayList<TaskItem>();
        }   
        IMyCollection<Task_Allocation> filtered = allocations.Filter(t =>
            !string.IsNullOrWhiteSpace(Convert.ToString(t.Person.Id)) &&
            Convert.ToString(t.Person.Id).Trim().Equals(Convert.ToString(activePerson.Id).Trim(), StringComparison.OrdinalIgnoreCase));
        var iterator = filtered.GetIterator();
        IMyCollection<TaskItem> tasks = new MyArrayList<TaskItem>();
        while(iterator.HasNext())
        {
            tasks.Add(iterator.Next().Task);
        }
        return tasks;
    }

    private IMyCollection<TaskItem> GetNonDependantTasks(IMyCollection<TaskItem> tasks)
    {
        IMyCollection<TaskItem> filtered = new MyArrayList<TaskItem>();
        var it = tasks.GetIterator();
        while(it.HasNext())
        {
            TaskItem task = it.Next();
            bool completed = true;
            if(task.dependant != null && task.dependant.Count != 0)
            {
                var it2 = task.dependant.GetIterator();
                while(it2.HasNext())
                {
                    var id = it2.Next();
                    if(_taskservice.GetAllTasks().TryFindBy(id, (item, key) => item.Id.CompareTo(key), out var foundTask))
                    {
                        if(foundTask.Status != TaskStatus.Completed)
                        {
                            completed = false;
                        }
                    }
                }
            }
            if(completed)
            {
                filtered.Add(task);
            }
        }
        return filtered;
    }

    private void UpdateAllocations(TaskItem Task, string description, TaskPriority priority, TaskStatus status)
    {
        IMyCollection<Person> people = _personservice.GetAllPersons();
        var it = people.GetIterator();
        while(it.HasNext())
        {
            Person person = it.Next();
            var iterator = _taskservice.GetAllTasks().GetIterator();
            while(iterator.HasNext())
            {
                TaskItem item = iterator.Next();
                _allocationservice.UpdateAllocations(Task, person, description, priority, status);
                _allocationservice.UpdateDependantAllocations(Task, item, person, description, priority, status);
            }
        }
    }

    private void UpdateDependantTask(int id, string description, TaskPriority priority, TaskStatus status)
    {
        var iterator = _taskservice.GetAllTasks().GetIterator();
        while(iterator.HasNext())
        {
            TaskItem item = iterator.Next();
            _taskservice.UpdateDependantTask(item, id, description, priority, status);
        }
    }
    
    private void Remove()
    {
        IMyCollection<TaskItem> tasks = GetNonDependantTasks(GetUserTasks());
        TaskItem task = ChooseTasks(tasks);
        if(task == null)
        {
            return;
        }
        var iterator = _taskservice.GetAllTasks().GetIterator();
        while(iterator.HasNext())
        {
            TaskItem item = iterator.Next();
            _taskservice.RemoveDependantTask(task.Id, item);
        }
        IMyCollection<Person> people = _personservice.GetAllPersons();
        var it = people.GetIterator();
        while(it.HasNext())
        {
            Person person = it.Next();
            var iterator2 = _taskservice.GetAllTasks().GetIterator();
            while(iterator2.HasNext())
            {
                TaskItem item = iterator2.Next();
                _allocationservice.RemoveAllocation(task, person);
                _allocationservice.RemoveDependantAllocations(item, person, task.Id);
            }
        }
        _taskservice.RemoveTask(task.Id);
    }
    
    private void UpdateTask()
    {
        IMyCollection<TaskItem> tasks = GetNonDependantTasks(GetUserTasks());
        TaskItem Task = ChooseTasks(tasks);

        if (Task == null)
            return;

        string description = Prompt("Enter task description: ");
        TaskPriority priority = AskPriority();
        TaskStatus status = AskStatus();
        UpdateAllocations(Task, description, priority, status);
        UpdateDependantTask(Task.Id, description, priority, status);
        _taskservice.UpdateTask(Task.Id, description, priority, status);
    }

    // 
    private void ToggleTaskCompletion()
    {
        IMyCollection<TaskItem> tasks = GetNonDependantTasks(GetUserTasks());
        TaskItem toggleTask = ChooseTasks(tasks);
        if(toggleTask == null)
        {
            return;
        }
        TaskStatus status;
        if(toggleTask.Status != TaskStatus.Completed)
        {
            status = TaskStatus.Completed;
        }
        else
        {
            status = TaskStatus.todo;
        }
        UpdateAllocations(toggleTask, toggleTask.Description, toggleTask.Priority, status);
        UpdateDependantTask(toggleTask.Id, toggleTask.Description, toggleTask.Priority, status);
        _taskservice.ToggleTaskCompletion(toggleTask.Id, status);
    }

    // fernando 
    private void AssignMenu()
    {
        while (true)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Task Views[/]")
                    .HighlightStyle(new Style(Color.Cyan1))
                    .AddChoices(new[]
                    {
                        "Assign Task",
                        "Unassign Task",
                        "Add Person",
                        "Back"
                    }));

            switch (option)
            {
                case "Assign Task":
                    Assigntask();
                    break;

                case "Unassign Task":
                    UnAssigntask();
                    break;
                // Aimee
                case "Add Person":
                    string name = Prompt("Enter person name: ");
                    _personservice.AddPerson(name);
                    break;
                    
                case "Back":
                    return;
            }
        }
    }

    // fernando
    private void Assigntask()
    {
        Console.Clear();
        TaskItem task = ChooseTasks(_taskservice.GetAllTasks());
        if(task is null)
        {
            return;
        }
        Person person = Chooseperson(_personservice.GetAllPersons());
        if(person is null)
        {
            return;
        }
        if(!_allocationservice.CheckIfAllocationExists(task, person))
        {
            _allocationservice.AddAllocation(task, person);
        }
        else
        {
            AnsiConsole.Write("That person is already assigned to that task.");
            AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
            Console.ReadKey();
        }  
    }

    // fernando
    private void UnAssigntask()
    {
        Console.Clear();
        Task_Allocation chosen_allocation = ChooseAllocation(_allocationservice.GetAllAllocations());
        if(chosen_allocation != null)
        {
            _allocationservice.RemoveAllocation(chosen_allocation.Task, chosen_allocation.Person);
        }
    }

    // mo
    private string FormatPriority(TaskPriority priority)
    {
        switch (priority)
        {
            case TaskPriority.MustHave:
                return "[grey]Must Have[/]";
            case TaskPriority.ShouldHave:
                return "[yellow]Should Have[/]";
            case TaskPriority.CouldHave:
                return "[red]Could Have[/]";
            default:
                return priority.ToString();
        }
    }

    // mo
    private string FormatStatus(TaskStatus status)
    {
        switch (status)
        {
            case TaskStatus.todo:
                return "[grey]To do[/]";
            case TaskStatus.InProgress:
                return "[blue]In progress[/]";
            case TaskStatus.Completed:
                return "[green]Completed[/]";
            default:
                return status.ToString();
        }
    }
    // fernando
    private void SelectPerson()
    {
        // _personservice.AddPerson("Fernando");
        // _personservice.AddPerson("Aimee");
        // _personservice.AddPerson("Mouhamad");

        var people = _personservice.GetAllPersons();

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<Person>()
                .Title("[yellow]Who are you?[/]")
                .PageSize(5)
                .HighlightStyle(new Style(Color.DarkViolet))
                .UseConverter(p => p.Name)
                .AddChoices(people.ToArray())
        );

        activePerson = selected;

        AnsiConsole.MarkupLine($"[green]Welcome, {selected.Name}![/]");
    }

    // mo
    private string GetAssignedPersonName(int taskId)
    {
        var allocations = _allocationservice.GetAllAllocations();
        var allocationIterator = allocations.GetIterator();

        string result = "";

        while (allocationIterator.HasNext())
        {
            var allocation = allocationIterator.Next();

            if (allocation.Task.Id == taskId)
            {
                if (result != "")
                    result += ", ";

                result += allocation.Person.Name;
            }
        }

        if (result == "")
            return "[grey]Unassigned[/]";

        return result;
    }


    // Aimee
    private void DisplayDependencyGraph()
    {
        Console.Clear();

        var tasks = _taskservice.GetAllTasks();
        var root = new Tree("[yellow]Task Dependency Graph[/]"); // het zit in spectre console zelf.

        var iterator = tasks.GetIterator();

        while (iterator.HasNext())
        {
            var task = iterator.Next();
            if (task.dependant == null || task.dependant.Count == 0) // een task die op zichzelf nergens van dependant is. 
            {
                var node = root.AddNode(FormatTaskForGraph(task)); // spectre console build in functions. 
                AddChildren(node, task, tasks, new MyArrayList<int>()); // recursive. 
            }
        }

        AnsiConsole.Write(root);
        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
    // Aimee
    private void AddChildren(TreeNode parentNode, TaskItem parentTask, IMyCollection<TaskItem> allTasks, IMyCollection<int> visited)
    {
        var visitedIterator = visited.GetIterator();
        while (visitedIterator.HasNext())
        {
            if (visitedIterator.Next() == parentTask.Id)
                return;
        }

        visited.Add(parentTask.Id);

        var iterator = allTasks.GetIterator();

        while (iterator.HasNext())
        {
            var task = iterator.Next();

            if (task.dependant != null && task.dependant.Count != 0)
            {
                var dependant = task.dependant.GetIterator();
                while (dependant.HasNext())
                {
                    var id = dependant.Next();

                    if (id == parentTask.Id)
                    {
                        var childNode = parentNode.AddNode(FormatTaskForGraph(task));

                        AddChildren(childNode, task, allTasks, visited);
                    }
                }
            }
        }
    }
    // Aimee
    private string FormatTaskForGraph(TaskItem t)
    {
        return $"[bold]#{t.Id}[/] {t.Description} ({FormatStatus(t.Status)})";
    }
}
