[TestClass]
public class TaskHashMapCollectionTests
{
    [TestMethod]
    public void TestWrapper_AddAndRemove()
    {
        var taskCollection = new TaskHashMapCollection();
        var task = new TaskItem { Id = 50, Description = "Fix Hashmap" };

        taskCollection.Add(task);
        Assert.AreEqual(1, taskCollection.Count);

        // Test FindBy using the ID
        bool found = taskCollection.TryFindBy(50, (t, id) => t.Id - id, out var result);
        Assert.IsTrue(found);
        Assert.AreEqual("Fix Hashmap", result?.Description);

        taskCollection.Remove(task);
        Assert.AreEqual(0, taskCollection.Count);
    }

    [TestMethod]
    public void TestWrapper_Iterator()
    {
        var taskCollection = new TaskHashMapCollection();
        taskCollection.Add(new TaskItem { Id = 1, Description = "Task A" });
        taskCollection.Add(new TaskItem { Id = 2, Description = "Task B" });

        int count = 0;
        var it = taskCollection.GetIterator();
        while (it.HasNext())
        {
            it.Next();
            count++;
        }

        Assert.AreEqual(2, count);
    }
}