[TestClass]
public class MyHashMapTests
{
    [TestMethod]
    public void TestAddAndRetrieve()
    {
        
        var map = new MyHashMap<int, string>(4);

        map.Add(new KeyValuePair<int, string>(1, "First"));
        map.Add(new KeyValuePair<int, string>(2, "Second"));

        bool found = map.TryFindBy(1, (pair, key) => pair.Key - key, out var result);
        Assert.IsTrue(found);
        Assert.AreEqual("First", result.Value);
        Assert.AreEqual(2, map.Count);
    }

    [TestMethod]
    public void TestAddDuplicateKey_ShouldThrowException()
    {
        var map = new MyHashMap<int, string>();
        map.Add(new KeyValuePair<int, string>(10, "Original"));

        try
        {
            map.Add(new KeyValuePair<int, string>(10, "Duplicate"));
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            // Test passes
        }
    }

    [TestMethod]
    public void TestResize_WhenLoadFactorExceeded()
    {
        // Arrange - Capacity 4, Load Factor 0.75. 4 * 0.75 = 3 items limit.
        var map = new MyHashMap<int, string>(4);

        // Act
        map.Add(new KeyValuePair<int, string>(1, "A"));
        map.Add(new KeyValuePair<int, string>(2, "B"));
        map.Add(new KeyValuePair<int, string>(3, "C"));
        map.Add(new KeyValuePair<int, string>(4, "D")); // Should trigger Resize()

        // Assert
        Assert.AreEqual(4, map.Count);
        // Verify items survived the rehash
        bool found = map.TryFindBy(4, (pair, key) => pair.Key - key, out var result);
        Assert.IsTrue(found);
        Assert.AreEqual("D", result.Value);
    }

    [TestMethod]
    public void TestRemove()
    {
        var map = new MyHashMap<string, int>();
        map.Add(new KeyValuePair<string, int>("Key1", 100));
        
        map.Remove(new KeyValuePair<string, int>("Key1", 100));

        Assert.AreEqual(0, map.Count);
        bool found = map.TryFindBy("Key1", (pair, key) => pair.Key.CompareTo(key), out _);
        Assert.IsFalse(found);
    }
}