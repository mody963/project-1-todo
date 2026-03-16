class Program
{
    static void Main(string[] args)
    {
        // Dependency injection: wiring up our components
        string filePath = "tasks.json";
        string filePath2 = "Persons.json";
        string filePath3 = "Allocations.json";
        ITaskRepository repository = new JsonTaskRepository(filePath);
        IPersonRepository repository2 = new JsonPersonRepository(filePath2);
        IAllocationRepository repository3 = new JsonAllocationRepository(filePath3);
        ITaskService service = new TaskService(repository);
        IPersonService service2 = new PersonService(repository2);
        IAllocationService service3 = new AllocationService(repository3);
        ITaskView view = new ConsoleTaskView(service, service2, service3);
        // Run the view
        view.Run();
    }
}
