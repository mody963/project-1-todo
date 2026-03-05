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
        ITaskView view = new ConsoleTaskView(service);
        // Run the view
        view.Run();
    }
}
