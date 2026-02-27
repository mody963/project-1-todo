class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    // void DisplayTasks(IEnumerable<TaskItem> tasks)
    // {
    //     Console.Clear();
    //     Console.WriteLine("==== ToDo List ====");

    //     foreach (var task in tasks)
    //         Console.WriteLine($"{task}");
    // }
    void DisplayTasks(IMyCollection<TaskItem> tasks)
    {
        Console.WriteLine("==== ToDo List ====");

        var iterator = tasks.GetIterator();

        while (iterator.HasNext())
        {
            var task = iterator.Next();
            Console.WriteLine($"{task.Id} | {task.Description} | Completed: {task.Completed}");
        }
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
            main_options.Add("Toggle Task State");
            main_options.Add("List Tasks");
            main_options.Add("Exit");
        while (true)
        {
            

            // var main_options = new List<string>
            // {
            //     "Add Task",
            //     "Remove Task",
            //     "Toggle Task State",
            //     "List Tasks",
            //     "Exit"
            // };

            int select_index = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Main Menu ===\n");

                DisplayTasks(_service.GetAllTasks());
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
                    _service.AddTask(description);
                    break;

                case 1:
                    string removeIdStr = Prompt("Enter task id to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId))
                    {
                        _service.RemoveTask(removeId);
                    }
                    break;

                case 2:
                    string toggleIdStr = Prompt("Enter task id to toggle: ");
                    if (int.TryParse(toggleIdStr, out int toggleId))
                    {
                        _service.ToggleTaskCompletion(toggleId);
                    }
                    break;
                // case 3:



                case 4:
                    return;
            }
        }
    }
}
