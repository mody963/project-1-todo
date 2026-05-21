// Aimee


using Spectre.Console;

public static class FilterTasks
{
    public static void FiltersTasks(IMyCollection<TaskItem> tasks)
    {
        while (true)
        {
            IMyCollection<string> menu = new MyArrayList<string>();
            menu.Add("Status");
            menu.Add("Priority");
            menu.Add("Creation Date");
            menu.Add("Back");

            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Filter Tasks[/]")
                    .HighlightStyle(new Style(Color.Cyan1))
                    .AddChoices(menu.ToArray())
            );

            if (selected == "Back")
                return;

            IMyCollection<TaskItem>? result = null;

            if (selected == "Status")
                result = FilterByStatus(tasks);

            else if (selected == "Priority")
                result = FilterByPriority(tasks);

            else if (selected == "Creation Date")
                result = SortByCreationDate(tasks);

            if (result != null)
            {
                DisplayTasks(result);

                AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
                Console.ReadKey();
            }
        }
    }
    // aimee
    private static IMyCollection<TaskItem> FilterByStatus(IMyCollection<TaskItem> tasks)
    {
        var statusOptions = new MyArrayList<string>();
        statusOptions.Add("to do");
        statusOptions.Add("in progress");
        statusOptions.Add("completed");
        statusOptions.Add("Back");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select Status[/]")
                .AddChoices(statusOptions.ToArray())
        );

        if (selected == "Back")
            return null;

        return tasks.Filter(t => t.Status == selected switch
        {
            "to do"       => TaskStatus.todo,
            "in progress" => TaskStatus.InProgress,
            "completed"   => TaskStatus.Completed,
            _             => (TaskStatus)(-1)
        });
    }
    // aimee
    private static IMyCollection<TaskItem> FilterByPriority(IMyCollection<TaskItem> tasks)
    {
        var priorityOptions = new MyArrayList<string>();
        priorityOptions.Add("must have");
        priorityOptions.Add("should have");
        priorityOptions.Add("could have");
        priorityOptions.Add("Back");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select Priority[/]")
                .AddChoices(priorityOptions.ToArray())
        );

        if (selected == "Back")
            return null;

        return tasks.Filter(t => t.Priority == selected switch
        {
            "must have"   => TaskPriority.MustHave,
            "should have" => TaskPriority.ShouldHave,
            "could have"  => TaskPriority.CouldHave,
            _             => (TaskPriority)(-1)
        });
    }

    private static IMyCollection<TaskItem> SortByCreationDate(IMyCollection<TaskItem> tasks)
    {
        var dateOptions = new MyArrayList<string>();
        dateOptions.Add("Ascending");
        dateOptions.Add("Descending");
        dateOptions.Add("Back");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Creation Date Order[/]")
                .AddChoices(dateOptions.ToArray())
        );

        if (selected == "Back")
            return null;

        tasks.Sort((a, b) =>
            selected == "Ascending"
                ? a.CreationDate.CompareTo(b.CreationDate)
                : b.CreationDate.CompareTo(a.CreationDate));
    // sort verwacht een int terug door de comparisinn die het heeft in de imycollection. 
        return tasks;
    }

    private static void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        var iterator = tasks.GetIterator();
        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red] no tasks found. [/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.DarkViolet)
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Description[/]")
            .AddColumn("[bold]Priority[/]")
            .AddColumn("[bold]Status[/]")
            .AddColumn("[bold]Created[/]");

        while (iterator.HasNext())
        {
            var t = iterator.Next();

            table.AddRow(
                t.Id.ToString(),
                t.Description,
                t.Priority.ToString(),
                t.Status.ToString(),
                t.CreationDate.ToString("g")
            );
        }

        AnsiConsole.Write(table);
    }
}