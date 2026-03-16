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

    private void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("TASKS")
        .Color(Color.DarkViolet)
        .Centered());

        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No tasks found.[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            Console.ReadKey();
            return;
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

        while (iterator.HasNext())
        {
            var t = iterator.Next();
            table.AddRow(
                t.Id.ToString(),
                t.Description,
                FormatPriority(t.Priority),
                FormatStatus(t.Status),
                GetAssignedPersonName(t.Id),
                t.CreationDate.ToString("g")
            );
        }

        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey();
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
                    "Exit"
                }));
            switch (option)
            {
                case "Add Task":
                    string description = Prompt("Enter task description: ");
                    string priority = AskPriority();
                    _taskservice.AddTask(description, priority);
                    break;

                case "Remove Task":
                    int id = ChooseTasks(_taskservice.GetAllTasks(), true);
                    _taskservice.RemoveTask(id);
                    break;

                case "Update Task":  
                    UpdateTask();
                    break;

                case "Toggle Task State":
                    int toggleId = ChooseTasks(_taskservice.GetAllTasks(), true);
                    _taskservice.ToggleTaskCompletion(toggleId);
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
                case "Exit":
                    return;
            }
        }
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

        var todo = tasks.Filter(t => t.Status == "to do");
        var progress = tasks.Filter(t => t.Status == "in progress");
        var done = tasks.Filter(t => t.Status == "completed");

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
    private string AskPriority()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select task priority[/]")
                .AddChoices(
                    "must have",
                    "should have",
                    "could have"
                ));
    }
    private string AskStatus()
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select task status[/]")
                .AddChoices(
                    "to do",
                    "in progress",
                    "completed"
                ));
    }
    private void UpdateTask()
    {
        int id = ChooseTasks(_taskservice.GetAllTasks(), true);

        if (id == 0)
            return;

        string description = Prompt("Enter task description: ");
        string priority = AskPriority();
        string status = AskStatus();

        _taskservice.UpdateTask(id, description, priority, status);
    }

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

                case "Back":
                    return;
            }
        }
    }

    public void Assigntask()
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

    public void UnAssigntask()
    {
        Console.Clear();
        Task_Allocation chosen_allocation = ChooseAllocation(_allocationservice.GetAllAllocations());
        if(chosen_allocation != null)
        {
            _allocationservice.RemoveAllocation(chosen_allocation.Task, chosen_allocation.Person);
        }
    }

    private string FormatPriority(string priority)
    {
        switch (priority)
        {
            case "must have":
                return "[red]Must have[/]";
            case "should have":
                return "[yellow]Should have[/]";
            case "could have":
                return "[green]Could have[/]";
            default:
                return priority;
        }
    }
    private string FormatStatus(string status)
    {
        switch (status)
        {
            case "to do":
                return "[grey]To do[/]";
            case "in progress":
                return "[blue]In progress[/]";
            case "completed":
                return "[green]Completed[/]";
            default:
                return status;
        }
    }
    public void SelectPerson()
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
}
