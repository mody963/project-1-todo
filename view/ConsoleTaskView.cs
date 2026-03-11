using Spectre.Console;


class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;
    private Person? activePerson = null;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
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
            AnsiConsole.Ask<string>("Press any key to return...");
            return;
        }

        var table = new Table()
        .Border(TableBorder.Rounded)
        .BorderColor(Color.DarkViolet)
        .AddColumn("[bold]ID[/]")
        .AddColumn("[bold]Description[/]")
        .AddColumn("[bold]Priority[/]")
        .AddColumn("[bold]Status[/]")
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
                t.CreationDate.ToString("g")
            );
        }

        AnsiConsole.Write(table);
    }


    private int ChooseTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();

        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No tasks found.[/]");
            Console.ReadKey();
            return 0;
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
            return 0;

        return selectedTask.Id;
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
                    "List Tasks",
                    "Filter Tasks",
                    "Exit"
                }));
            switch (option)
            {
                case "Add Task":
                    string description = Prompt("Enter task description: ");
                    string priority = AskPriority();
                    _service.AddTask(description, priority);
                    break;

                case "Remove Task":
                    int id = ChooseTasks(_service.GetAllTasks());
                    _service.RemoveTask(id);
                    break;

                case "Update Task":  
                    UpdateTask();
                    break;

                case "Toggle Task State":
                    int toggleId = ChooseTasks(_service.GetAllTasks());
                    _service.ToggleTaskCompletion(toggleId);
                    break;
                case "List Tasks":
                    DisplayTasks(_service.GetAllTasks());
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    break;
                case "Filter Tasks":
                    FilterTasks.FiltersTasks(_service.GetAllTasks());
                    break;
                case "Exit":
                    return;
            }
        }
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
        int id = ChooseTasks(_service.GetAllTasks());

        if (id == 0)
            return;

        string description = Prompt("Enter task description: ");
        string priority = AskPriority();
        string status = AskStatus();

        _service.UpdateTask(id, description, priority, status);
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
        var fernando = new Person(1, "Fernando");
        var aimee = new Person(2, "Aimee");
        var mouhamad = new Person(3, "Mouhamad");

        var people = new MyArrayList<Person>();
        people.Add(fernando);
        people.Add(aimee);
        people.Add(mouhamad);

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
}
