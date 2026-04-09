class Program
{
    static void Main(string[] args)
    {
        // Dependency injection: wiring up our components
        string FilePath_Tasks = "tasks.json";
        string FilePath_Persons = "Persons.json";
        string FilePath_Allocations = "Allocations.json";


        //collection
        ICollectionFactory<TaskItem> taskFactory;
        ICollectionFactory<Person> personFactory;
        ICollectionFactory<Task_Allocation> allocationFactory;


        // hierin ervoor zorgen stel dat jekrijgt dat het ll is dan linked list implemeten enz. 
        if (args.Length > 0)
        {
            string collectionType = args[0].ToLower();
            switch (collectionType)
            {
                case "arraylist":
                    taskFactory = new MyArrayListFactory<TaskItem>();
                    personFactory = new MyArrayListFactory<Person>();
                    allocationFactory = new MyArrayListFactory<Task_Allocation>();
                    break;
                case "linkedlist":
                    taskFactory = new MyLinkedListFactory<TaskItem>();
                    personFactory = new MyLinkedListFactory<Person>();
                    allocationFactory = new MyLinkedListFactory<Task_Allocation>();
                    break;
                default:
                    Console.WriteLine("Unknown collection type specified. Defaulting to ArrayList.");
                    taskFactory = new MyArrayListFactory<TaskItem>();
                    personFactory = new MyArrayListFactory<Person>();
                    allocationFactory = new MyArrayListFactory<Task_Allocation>();
                    break;
            }
        }
        ITaskRepository repository = new TaskRepository(FilePath_Tasks, taskFactory);
        IPersonRepository repository2 = new PersonRepository(FilePath_Persons, personFactory);
        IAllocationRepository repository3 = new AllocationRepository(FilePath_Allocations, allocationFactory);


        ITaskService service = new TaskService(repository);
        IPersonService service2 = new PersonService(repository2);
        IAllocationService service3 = new AllocationService(repository3);


        ITaskView view = new ConsoleTaskView(service, service2, service3);
        // Run the view
        view.Run();
    }
}
