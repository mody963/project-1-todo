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
    public static IMyCollection<TaskItem> FiltersTasks(IMyCollection<TaskItem> tasks)
    {
        IMyCollection<TaskItem> filteredTasks = tasks;

        var filterOptions = new MyArrayList<string>();
        filterOptions.Add("Status");
        filterOptions.Add("Priority");
        filterOptions.Add("Creation Date");
        filterOptions.Add("Back");

        int selectedFilter = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Filter Tasks ===\n");
            DisplayMenu(filterOptions, selectedFilter);

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedFilter--;
                if (selectedFilter < 0) selectedFilter = filterOptions.Count - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedFilter++;
                if (selectedFilter >= filterOptions.Count) selectedFilter = 0;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                if (selectedFilter == filterOptions.Count - 1) return filteredTasks; // Back

                switch (selectedFilter)
                {
                    case 0:
                        filteredTasks = FilterByStatus(filteredTasks);
                        break;
                    case 1:
                        filteredTasks = FilterByPriority(filteredTasks);
                        break;
                    case 2:
                        filteredTasks = SortByCreationDate(filteredTasks);
                        break;
                }

                // Toon resultaten
                Console.Clear();
                Console.WriteLine("=== Filtered Tasks ===\n");
                DisplayTasks(filteredTasks);

                Console.WriteLine("\nPress any key to return to filter menu...");
                Console.ReadKey();
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                return filteredTasks;
            }
        }
    }

    private static IMyCollection<TaskItem> FilterByStatus(IMyCollection<TaskItem> tasks)
    {
        var statusOptions = new MyArrayList<string>();
        statusOptions.Add("to do");
        statusOptions.Add("in progress");
        statusOptions.Add("completed");
        statusOptions.Add("Back");

        int selection = ChooseOption("Select Status:", statusOptions);

        if (selection == 3)
            return tasks;

        string selectedStatus = statusOptions.ToArray()[selection];

        return tasks.Filter(t =>
            !string.IsNullOrWhiteSpace(t.Status) &&
            t.Status.Trim().Equals(selectedStatus.Trim(), StringComparison.OrdinalIgnoreCase));
    }
    private static IMyCollection<TaskItem> FilterByPriority(IMyCollection<TaskItem> tasks)
    {
        var priorityOptions = new MyArrayList<string>();
        priorityOptions.Add("Most important");
        priorityOptions.Add("Least important");
        priorityOptions.Add("Back");

        int selection = ChooseOption("Select Priority:", priorityOptions);
        if (selection == 2) return tasks; // Back

        tasks.Sort((a, b) =>
        {
            int rankA = GetPriorityRank(a.Priority);
            int rankB = GetPriorityRank(b.Priority);
            return selection == 0 ? rankB - rankA : rankA - rankB;
        });

        return tasks;
    }

    private static int GetPriorityRank(string priority)
    {
        var iterator = PriorityRanking.GetIterator();
        while (iterator.HasNext())
        {
            var p = iterator.Next();
            if (p.Name == priority) return p.Rank;
        }
        return 0;
    }

    private static IMyCollection<TaskItem> SortByCreationDate(IMyCollection<TaskItem> tasks)
    {
        var dateOptions = new MyArrayList<string>();
        dateOptions.Add("Ascending");
        dateOptions.Add("Descending");
        dateOptions.Add("Back");

        int selection = ChooseOption("Select Creation Date Order:", dateOptions);
        if (selection == 2) return tasks; // Back

        tasks.Sort((a, b) =>
            selection == 0 ? a.CreationDate.CompareTo(b.CreationDate)
                            : b.CreationDate.CompareTo(a.CreationDate));

        return tasks;
    }

    private static int ChooseOption(string title, IMyCollection<string> options)
    {
        int selectedIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== {title} ===\n");
            DisplayMenu(options, selectedIndex);

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.UpArrow)
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = options.Count - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                selectedIndex++;
                if (selectedIndex >= options.Count) selectedIndex = 0;
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                return selectedIndex;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                return options.Count - 1; // Back
            }
        }
    }

    private static void DisplayMenu(IMyCollection<string> options, int selectedIndex)
    {
        var iterator = options.GetIterator();
        int i = 0;
        while (iterator.HasNext())
        {
            string option = iterator.Next();
            if (i == selectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"> {option}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"  {option}");
            }
            i++;
        }
    }

    private static void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        var iterator = tasks.GetIterator();
        if (!iterator.HasNext())
        {
            Console.WriteLine("No tasks found.");
            return;
        }

        while (iterator.HasNext())
        {
            var task = iterator.Next();
            string status = task.Completed ? "X" : " ";
            Console.WriteLine($"{task.Id}. [{status}] {task.Description} (Priority: {task.Priority}, Created: {task.CreationDate})");
        }
    }
}