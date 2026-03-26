public interface IMyIterable<T>
{
    IMyIterator<T> GetIterator();
}