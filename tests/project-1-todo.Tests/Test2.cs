namespace project_1_todo.Tests;

[TestClass]
public sealed class Test2
{
    [TestMethod]
    public void TestEmptyList()
    {
        // arrange
        var list = new MyLinkedList<int>();

        // act
        var count = list.Count;
        var isDirty = list.Dirty;

        // assert
        Assert.AreEqual(0, count);
        Assert.IsFalse(isDirty);
    }

    [TestMethod]
    public void TestAddItems()
    {
        // arrange
        var list = new MyLinkedList<int>();

        // act
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // assert
        Assert.AreEqual(3, list.Count);
        Assert.IsTrue(list.Dirty);
    }

    [TestMethod]

}