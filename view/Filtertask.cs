using Spectre.Console;
public static class FilterTasks
{
    private static readonly MyArrayList<(string Name, int Rank)> PriorityRanking;

    static FilterTasks()
    {
        PriorityRanking = new MyArrayList<(string, int)>();
        PriorityRanking.Add(("must have", 3));
        PriorityRanking.Add(("should have", 2));
        PriorityRanking.Add(("could have", 1));
    }
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
    private static IMyCollection<TaskItem> FilterByStatus(IMyCollection<TaskItem> tasks)
    {
        var statusOptions = new MyArrayList<string>();
        statusOptions.Add("to do"); //0
        statusOptions.Add("in progress");//1
        statusOptions.Add("completed");//2
        statusOptions.Add("Back");//3

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Select Status[/]")
                .AddChoices(statusOptions.ToArray())
        );

        if (selected == "Back")
            return null;

        return tasks.Filter(t =>
            !string.IsNullOrWhiteSpace(t.Status) &&
            t.Status.Trim().Equals(selected.Trim(), StringComparison.OrdinalIgnoreCase));
    }
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

        return tasks.Filter(t =>
            !string.IsNullOrWhiteSpace(t.Priority) &&
            t.Priority.Trim().Equals(selected.Trim(), StringComparison.OrdinalIgnoreCase));
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
                t.Priority,
                t.Status,
                t.CreationDate.ToString("g")
            );
        }

        AnsiConsole.Write(table);
    }
}