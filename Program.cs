// Aimee

using Spectre.Console;

class Program
{
// Wanneer je dotnet run gebruikt, moet je de argumenten doorgeven na een dubbel koppelteken --. 
// Dit vertelt de .NET CLI dat de argumenten niet voor de compiler zijn, maar voor jouw programma:
// Voor de Linked List: dotnet run -- ll
// Voor de Array List: dotnet run -- al 
// enz enz. 
    static void Main(string[] args)
    {
        // Dependency injection: wiring up our components
        string FilePath_Tasks = "tasks.json";
        string FilePath_Persons = "Persons.json";
        string FilePath_Allocations = "Allocations.json";


        //collection
        ICollectionFactory<TaskItem> taskFactory = new MyArrayListFactory<TaskItem>();
        ICollectionFactory<Person> personFactory = new MyArrayListFactory<Person>();
        ICollectionFactory<Task_Allocation> allocationFactory = new MyArrayListFactory<Task_Allocation>();


        // kijken of er een value wordt mee gegeven wat dan de bijbehorende collectie is. 
        string collectionType = (args.Length > 0) ? args[0].ToLower() : "al";


        // hierin ervoor zorgen stel dat jekrijgt dat het ll is dan linked list implemeten enz. 
        switch (collectionType)
        {
            case "ll":
                Console.WriteLine("[INFO] Modus: Linked List");
                taskFactory = new MyLinkedListFactory<TaskItem>();
                personFactory = new MyLinkedListFactory<Person>();
                allocationFactory = new MyLinkedListFactory<Task_Allocation>();
                break;
            case "bt":
                Console.WriteLine("[INFO] Modus: Binary Tree");
                taskFactory = new MyBinaryTreeFactory<TaskItem>();
                personFactory = new MyBinaryTreeFactory<Person>();
                allocationFactory = new MyBinaryTreeFactory<Task_Allocation>();
                break;
            // als het fout gaat dan als default array list gebruiken.
            case "al":
            default:
                Console.WriteLine("[INFO] Modus: Array List");
                taskFactory = new MyArrayListFactory<TaskItem>();
                personFactory = new MyArrayListFactory<Person>();
                allocationFactory = new MyArrayListFactory<Task_Allocation>();
                break;
            case "hm":
                Console.WriteLine("[INFO] Modus: HashMap");
                taskFactory = new MyHashMapFactory<TaskItem>();
                personFactory = new MyHashMapFactory<Person>();
                allocationFactory = new MyHashMapFactory<Task_Allocation>();
                break;     
        }

        // repositorys. 
        ITaskRepository taskRepo = new TaskRepository(FilePath_Tasks, taskFactory);
        IPersonRepository personRepo = new PersonRepository(FilePath_Persons, personFactory);   
        IAllocationRepository allocationRepo = new AllocationRepository(FilePath_Allocations, allocationFactory);

        //de Services
        ITaskService taskService = new TaskService(taskRepo);
        IPersonService personService = new PersonService(personRepo);
        IAllocationService allocationService = new AllocationService(allocationRepo);

        // 6. View opstarten
        ITaskView view = new ConsoleTaskView(taskService, personService, allocationService);
        
        try 
        {
            view.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"fix dit stuk: {ex.Message}");
        }
    }
}
