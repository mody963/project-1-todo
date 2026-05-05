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


        public void TestRemoveItem()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // act
        list.Remove(2);

        // assert
        Assert.AreEqual(2, list.Count);
        Assert.IsTrue(list.Dirty);
    }

    [TestMethod]
    public void TestInsertAtIndex()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(3);

        // act
        list.Insert(2, 1);
        var result = list.ToArray();

        // assert
        Assert.AreEqual(3, list.Count);
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(2, result[1]);
        Assert.AreEqual(3, result[2]);
    }

 
}