[TestClass]
public class MyBinaryTreeTests
{
    [TestMethod]
    public void TestAdd()
    {
        var list = new MyBinaryTree<string>();

        list.Add("Task mo");
        list.Add("Task fernando");

        Assert.AreEqual(2, list.Count);
        CollectionAssert.AreEqual(new[] { "Task fernando", "Task mo" }, list.array); // The two items are swapped because they are automatically ordered
    }

    [TestMethod]
    public void TestRemove()
    {
        var list = new MyBinaryTree<string>();

        list.Add("Task mo");
        list.Add("Task fernando");
        list.Remove("Task mo");

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual("Task fernando", list.array[0]);
    }

    [TestMethod]
    public void TestTryFindBy()
    {
        var list = new MyBinaryTree<string>();
        list.Add("Task mo");

        bool found = list.TryFindBy("Task mo", (item, key) => item.CompareTo(key), out string? result);

        Assert.IsTrue(found);
        Assert.AreEqual("Task mo", result);
    }

    [TestMethod]
    public void TestSort()
    {
        var list = new MyBinaryTree<int>();
        list.Add(60);
        list.Add(10);
        list.Add(20);

        list.Sort((a, b) => a.CompareTo(b));

        CollectionAssert.AreEqual(new[] { 10, 20, 60 }, list.array);
    }

    [TestMethod]
    public void TestReduce()
    {
        var list = new MyBinaryTree<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        int sum = list.Reduce(0, (acc, item) => acc + item);

        Assert.AreEqual(6, sum);
    }

    [TestMethod]
    public void TestIterator()
    {
        // arrange
        var list = new MyBinaryTree<int>();
        list.Add(1);
        list.Add(10);
        list.Add(2);
        list.Add(5);

        // act
        var iterator = list.GetIterator();

        // assert
        // Items are excpected in order because binary tree is automatically ordered
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(1, iterator.Next());
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(2, iterator.Next());
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(5, iterator.Next());
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(10, iterator.Next());
        Assert.IsFalse(iterator.HasNext());
    }

    [TestMethod]
    public void TestFilter()
    {
        // arrange
        var list = new MyBinaryTree<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(4);

        // act
        var evenItems = list.Filter(x => x % 2 == 0);
        var result = evenItems.ToArray();

        // assert
        Assert.AreEqual(2, evenItems.Count);
        Assert.AreEqual(2, result[0]);
        Assert.AreEqual(4, result[1]);
    }
}