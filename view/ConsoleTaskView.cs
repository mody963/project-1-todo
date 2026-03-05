using System.Reflection.Metadata.Ecma335;
using Spectre.Console;

class ConsoleTaskView : ITaskView
{
    private readonly ITaskService _service;

    private Person activeperson;

    public ConsoleTaskView(ITaskService service)
    {
        _service = service;
    }

    private int DisplayAndChooseTasks(IMyCollection<TaskItem> tasks)
{

    // "Id": 11,
    // "Description": "task11",
    // "Priority": "must have",
    // "Status": "to do",
    // "Completed": false,
    // "CreationDate": "2026-03-01T12:02:46.260302+01:00"
    var table = new Table()
        .AddColumn("ID")
        .AddColumn("Description")
        .AddColumn("Priority")
        .AddColumn("Status")
        .AddColumn("Created At");

    var iterator = tasks.GetIterator();

    if (!iterator.HasNext())
    {
        AnsiConsole.MarkupLine("[red]No tasks found.[/]");
        AnsiConsole.Ask<string>("Press any key to return to menu...");
        return 0;
    }

    var taskArray = new MyArrayList<TaskItem>();

    while (iterator.HasNext())
    {
        var task = iterator.Next();
        taskArray.Add(task);

        string status = task.Status;
        table.AddRow(
            task.Id.ToString(),
            task.Description,
            task.Priority,
            status,
            task.CreationDate.ToString("g")
        );
    }
    AnsiConsole.Write(table);
    AnsiConsole.MarkupLine("\nUse Arrow Keys to navigate, Enter to select, Esc to go back");
    return 0;


}


    string Prompt(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    public void Select_person()
    {
        Person fernando = new Person(1, "fernando");
        Person aimee = new Person(1, "aimee");
        Person mouhamad = new Person(1, "mouhamad");
        Person who_is_this;
        while (true)
        {
            System.Console.WriteLine("Who are you:");
            System.Console.Write("Fernando, Aimee, Mouhamad\n");
            string name = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (name == "fernando")
            {
                who_is_this = fernando;
                break;
            }
            else if(name == "aimee")
            {
                who_is_this = aimee;
                break;
            }
            else if(name == "mouhamad")
            {
                who_is_this = mouhamad;
                break;
            }
            else
            {
                System.Console.WriteLine("Please enter one of the available names.");
            }
        }
        activeperson = who_is_this;
    }
    
    public void Run()
    {
        Console.Clear();
        Select_person();
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
                    DisplayAndChooseTasks(_service.GetAllTasks());
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
