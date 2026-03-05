using Spectre.Console;


class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    private void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No tasks found.[/]");
            AnsiConsole.Ask<string>("Press any key to return...");
            return;
        }

        var table = new Table()
            .AddColumn("ID")
            .AddColumn("Description")
            .AddColumn("Priority")
            .AddColumn("Status")
            .AddColumn("Created At");

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


    private int DisplayAndChooseTasks(IMyCollection<TaskItem> tasks)
    {
        Console.Clear();
        var iterator = tasks.GetIterator();

        if (!iterator.HasNext())
        {
            AnsiConsole.MarkupLine("[red]No tasks found.[/]");
            AnsiConsole.Ask<string>("Press any key to return...");
            return 0;
        }

        // Zet taken in MyArrayList
        var taskArray = new MyArrayList<TaskItem>();
        while (iterator.HasNext())
        {
            taskArray.Add(iterator.Next());
        }

        // Tabel maken
        var table = new Table()
            .AddColumn("ID")
            .AddColumn("Description")
            .AddColumn("Priority")
            .AddColumn("Status")
            .AddColumn("Created At");

        var taskIter = taskArray.GetIterator();
        while (taskIter.HasNext())
        {
            var t = taskIter.Next();
            table.AddRow(
                t.Id.ToString(),
                t.Description,
                t.Priority,
                t.Status,
                t.CreationDate.ToString("g")
            );
        }

        AnsiConsole.Write(table);

        // choise menu 
        var choices = new MyArrayList<string>();
        var iter2 = taskArray.GetIterator();
        while (iter2.HasNext())
        {
            var t = iter2.Next();
            choices.Add($"{t.Id}. {t.Description} (Priority: {t.Priority}, Status: {t.Status}, Created: {t.CreationDate:g})");
        }
        choices.Add("Back");

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a task:")
                .PageSize(10)
                .AddChoices(choices.ToArray())
        );

        if (selected == "Back")
            return 0;

        // returning of the id of the selected task
        int dotIndex = selected.IndexOf('.');
        if (dotIndex == -1) return 0;
        string idStr = selected.Substring(0, dotIndex);
        return int.Parse(idStr); // converts string to int 
    }

    string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    public void Run()
    {
        IMyCollection<string> main_options = new MyArrayList<string>();

            main_options.Add("Add Task");
            main_options.Add("Remove Task");
            main_options.Add("Update Task");
            main_options.Add("Toggle Task State");
            main_options.Add("List Tasks");
            main_options.Add("Filter Tasks");
            main_options.Add("Exit");
        while (true)
        {

            int select_index = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Main Menu ===\n");

                Console.WriteLine();
                var optionIterator = main_options.GetIterator();
                int i = 0;

                while (optionIterator.HasNext())
                {
                    var option = optionIterator.Next();

                    if (i == select_index)
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

                //Console.WriteLine("\nUse Arrow Keys to navigate, Enter to select");

                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow)
                {
                    select_index--;
                    if (select_index < 0)
                    {
                        select_index = main_options.Count - 1;
                    }
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    select_index++;
                    if (select_index >= main_options.Count)
                    {
                        select_index = 0;
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    return;
                }
            }

            switch (select_index)
            {
                case 0:
                    string description = Prompt("Enter task description: ");
                    string priority;
                    while (true)
                    {
                        System.Console.WriteLine("Enter one of these priority levels:");
                        System.Console.Write("Must have, Should have, Could have\n");
                        string priority_input = Console.ReadLine()?.Trim().ToLower() ?? "";

                        if (priority_input == "must have" || priority_input == "should have" || priority_input == "could have")
                        {
                            priority = priority_input;
                            break;
                        }
                        System.Console.WriteLine("Please enter one of the available priority levels.");
                    }
                    _service.AddTask(description, priority);
                    break;

                case 1:
                    int id = DisplayAndChooseTasks(_service.GetAllTasks());
                    _service.RemoveTask(id);
                    break;

                case 2:
                    int updateid = DisplayAndChooseTasks(_service.GetAllTasks());
                    if(updateid == 0)
                    {
                        break;
                    }
                    string descr = Prompt("Enter task description: ");
                    string prio;
                    while (true)
                    {
                        System.Console.WriteLine("Enter one of these priority levels:");
                        System.Console.Write("Must have, Should have, Could have\n");
                        string priority_input = Console.ReadLine()?.Trim().ToLower() ?? "";

                        if (priority_input == "must have" || priority_input == "should have" || priority_input == "could have")
                        {
                            prio = priority_input;
                            break;
                        }
                        System.Console.WriteLine("Please enter one of the available priority levels.");
                    }
                    string status;
                    while (true)
                    {
                        System.Console.WriteLine("Enter one of these statuses:");
                        System.Console.Write("to do, in progress, completed\n");
                        string status_input = Console.ReadLine()?.Trim().ToLower() ?? "";

                        if (status_input == "to do" || status_input == "in progress" || status_input == "completed")
                        {
                            status = status_input;
                            break;
                        }
                        System.Console.WriteLine("Please enter one of the available statuses.");
                    }
                    
                    _service.UpdateTask(updateid, descr, prio, status);
                    break;

                case 3:
                    int toggleId = DisplayAndChooseTasks(_service.GetAllTasks());
                    _service.ToggleTaskCompletion(toggleId);
                    break;
                case 4:
                    DisplayTasks(_service.GetAllTasks());
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    break;
                case 5:
                    FilterTasks.FiltersTasks(_service.GetAllTasks());
                    break;
                case 6:
                    return;
            }
        }
    }
}
