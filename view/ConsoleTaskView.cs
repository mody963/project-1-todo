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
    // void DisplayTasks(IMyCollection<TaskItem> tasks)
    // {
    //     Console.Clear();
    //     Console.ForegroundColor = ConsoleColor.Cyan;
    //     Console.WriteLine("==== ToDo List ====");
    //     Console.ForegroundColor = ConsoleColor.White;

    //     var iterator = tasks.GetIterator();

    //     while (iterator.HasNext())
    //     {
    //         var task = iterator.Next();
    //         Console.WriteLine($"{task.Id}. [{(task.Completed ? "X" : " ")}] {task.Description}");
    //     }
    // }

void DisplayTasks(IMyCollection<TaskItem> tasks)
{
    Console.Clear();

    int width = 50;
    string lijn = new string('═', width);

    // headers
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔" + lijn + "╗");
    string title = "TODO LIST";
    int average_legte_header = (width - title.Length) / 2;
    Console.WriteLine("║" + new string(' ', average_legte_header) + title +
                      new string(' ', width - title.Length - average_legte_header) + "║");

    Console.WriteLine("╠" + lijn + "╣");
    Console.ResetColor();



    // taken zelf geprint 
    var iterator = tasks.GetIterator();

    if (!iterator.HasNext())
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string emptyMessage = "No tasks have been added yet";
        Console.WriteLine("║ " + emptyMessage.PadRight(width - 1) + "║");
        Console.ResetColor();
    }

    while (iterator.HasNext())
    {
        var task = iterator.Next();

        string status_tasks = task.Completed ? "X" : " ";
        

        string Id_en_status = $"{task.Id}. [{status_tasks}] ";
        string taak_descriptie = task.Description;

        int descriptionwidth = width - 1;
        int firstLineWidth = descriptionwidth - Id_en_status.Length;

        string descriptie_na_bewerk = taak_descriptie.Length > firstLineWidth
            ? taak_descriptie.Substring(0, firstLineWidth)
            : taak_descriptie; // neem alles wat past anders neem alles. 

        Console.WriteLine("║ " + (Id_en_status + descriptie_na_bewerk).PadRight(descriptionwidth) + "║"); // padright zorgt dat het hetzelfde is als die :16 enz maar dan makkelijker

        taak_descriptie = taak_descriptie.Length > firstLineWidth
            ? taak_descriptie.Substring(firstLineWidth)
            : ""; // resterende tekst die nog over zou zijn geweest

        // vervolg regels als tekst te lang is
        while (taak_descriptie.Length > 0)
        {
            string part = taak_descriptie.Length > descriptionwidth
                ? taak_descriptie.Substring(0, descriptionwidth)
                : taak_descriptie;

            Console.WriteLine("║ " + part.PadRight(descriptionwidth) + "║");

            taak_descriptie = taak_descriptie.Length > descriptionwidth
                ? taak_descriptie.Substring(descriptionwidth)
                : "";
        }

        Console.ResetColor();
    }

    // grondstukkie
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╚" + lijn + "╝");
    Console.ResetColor();
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
                    DisplayTasks(_service.GetAllTasks());
                    string toggleIdStr = Prompt("Enter task id to toggle: ");
                    if (int.TryParse(toggleIdStr, out int toggleId))
                    {
                        _service.ToggleTaskCompletion(toggleId);
                    }
                    break;
                case 3:
                    DisplayTasks(_service.GetAllTasks());
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                    break;
                case 4:
                    return;
            }
        }
    }
}
