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

    [TestMethod]
    public void TestIterator()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // act
        var iterator = list.GetIterator();

        // assert
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(1, iterator.Next());
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(2, iterator.Next());
        Assert.IsTrue(iterator.HasNext());
        Assert.AreEqual(3, iterator.Next());
        Assert.IsFalse(iterator.HasNext());
    }

    [TestMethod]
    public void TestFilter()
    {
        // arrange
        var list = new MyLinkedList<int>();
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

    [TestMethod]
    public void TestSort()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(3);
        list.Add(1);
        list.Add(4);
        list.Add(2);

        // act
        list.Sort((a, b) => a.CompareTo(b));
        var result = list.ToArray();

        // assert
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(2, result[1]);
        Assert.AreEqual(3, result[2]);
        Assert.AreEqual(4, result[3]);
    }

    [TestMethod]
    public void TestReduce()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // act
        var sum = list.Reduce((a, b) => a + b);

        // assert
        Assert.AreEqual(6, sum);
    }

    [TestMethod]
    public void TestToArray()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // act
        var result = list.ToArray();

        // assert
        Assert.AreEqual(3, result.Length);
        Assert.AreEqual(1, result[0]);
        Assert.AreEqual(2, result[1]);
        Assert.AreEqual(3, result[2]);
    }

    [TestMethod]
    public void TestTryFindBy()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // act
        var found = list.TryFindBy(2, (data, key) => data.CompareTo(key), out int value);

        // assert
        Assert.IsTrue(found);
        Assert.AreEqual(2, value);
    }

    [TestMethod]
    public void TestResetDirty()
    {
        // arrange
        var list = new MyLinkedList<int>();
        list.Add(1);

        // act
        list.ResetDirty();

        // assert
        Assert.IsFalse(list.Dirty);
    }
}