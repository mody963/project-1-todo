[TestClass]
public class MyArrayListTests
{
    [TestMethod]
    public void TestAdd()
    {
        var list = new MyArrayList<string>();

        list.Add("Task mo");
        list.Add("Task fernando");

        Assert.AreEqual(2, list.Count);
        CollectionAssert.AreEqual(new[] { "Task mo", "Task fernando" }, list.array);
    }

    [TestMethod]
    public void TestRemove()
    {
        var list = new MyArrayList<string>();

        list.Add("Task mo");
        list.Add("Task fernando");
        list.Remove("Task mo");

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual("Task fernando", list.array[0]);
    }

    [TestMethod]
    public void TestTryFindBy()
    {
        var list = new MyArrayList<string>();
        list.Add("Task mo");

        bool found = list.TryFindBy("Task mo", (item, key) => item.CompareTo(key), out string? result);

        Assert.IsTrue(found);
        Assert.AreEqual("Task mo", result);
    }

    [TestMethod]
    public void TestSort()
    {
        var list = new MyArrayList<int>();
        list.Add(60);
        list.Add(10);
        list.Add(20);

        list.Sort((a, b) => a.CompareTo(b));

        CollectionAssert.AreEqual(new[] { 10, 20, 60 }, list.array);
    }

    [TestMethod]
    public void TestReduce()
    {
        var list = new MyArrayList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        int sum = list.Reduce(0, (acc, item) => acc + item);

        Assert.AreEqual(6, sum);
    }
}