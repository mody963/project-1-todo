class Program
{
    static void Main(string[] args)
    {
        // Dependency injection: wiring up our components
        string filepath_task = "tasks.json";
        string filepath_persons = "Persons.json";
        string filepath_allocations = "Allocations.json";
        //collection
        ICollectionFactory<TaskItem> taskFactory = new MyArrayListFactory<TaskItem>();
        ICollectionFactory<Person> personFactory = new MyArrayListFactory<Person>();
        ICollectionFactory<Task_Allocation> allocationFactory = new MyArrayListFactory<Task_Allocation>();

        // switch(args[0])
        // {
        //     case "arraylist":
        //         taskFactory = new MyArrayListFactory<TaskItem>();
        //         personFactory = new MyArrayListFactory<Person>();
        //         allocationFactory = new MyArrayListFactory<Task_Allocation>();
        //         break;
        //     case "linkedlist":
        //         taskFactory = new MyLinkedListFactory<TaskItem>();
        //         personFactory = new MyLinkedListFactory<Person>();
        //         allocationFactory = new MyLinkedListFactory<Task_Allocation>();
        //         break;
        //     default:
        //         Console.WriteLine("Invalid collection type specified. Defaulting to ArrayList.");
        //         taskFactory = new MyArrayListFactory<TaskItem>();
        //         personFactory = new MyArrayListFactory<Person>();
        //         allocationFactory = new MyArrayListFactory<Task_Allocation>();
        //         break;
        // }

        ITaskRepository repository = new TaskRepository(filepath_task, taskFactory);
        IPersonRepository repository2 = new PersonRepository(filepath_persons, personFactory);
        IAllocationRepository repository3 = new AllocationRepository(filepath_allocations, allocationFactory);


        ITaskService service = new TaskService(repository);
        IPersonService service2 = new PersonService(repository2);
        IAllocationService service3 = new AllocationService(repository3);


        ITaskView view = new ConsoleTaskView(service, service2, service3);
        // Run the view
        view.Run();
    }
}
