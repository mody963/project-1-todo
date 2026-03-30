class Program
{
    static void Main(string[] args)
    {
        // Dependency injection: wiring up our components
        string filePath = "tasks.json";
        string filePath2 = "Persons.json";
        string filePath3 = "Allocations.json";
        //collection
        ICollectionFactory<TaskItem> taskFactory = new MyArrayListFactory<TaskItem>();
        ICollectionFactory<Person> personFactory = new MyArrayListFactory<Person>();
        ICollectionFactory<Task_Allocation> allocationFactory = new MyArrayListFactory<Task_Allocation>();


        // hierin ervoor zorgen stel dat jekrijgt dat het ll is dan linked list implemeten enz. 

        ITaskRepository repository = new TaskRepository(filePath, taskFactory);
        IPersonRepository repository2 = new PersonRepository(filePath2, personFactory);
        IAllocationRepository repository3 = new AllocationRepository(filePath3, allocationFactory);


        ITaskService service = new TaskService(repository);
        IPersonService service2 = new PersonService(repository2);
        IAllocationService service3 = new AllocationService(repository3);


        ITaskView view = new ConsoleTaskView(service, service2, service3);
        // Run the view
        view.Run();
    }
}
