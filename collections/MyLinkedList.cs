// 

class LinkedList<T>: IMyLinkedList<T>
{
    public interface IMyLinkedList<T> 
{

    public class LinkedList<T>
    {
        // This LinkedList is a doubly-Linked circular list.
        internal LinkedListNode<T>? head;
        internal int count;
        internal int version;


        // names for serialization
        private const string VersionName = "Version"; // Do not rename (binary serialization)
        private const string CountName = "Count"; // Do not rename (binary serialization)
        private const string ValuesName = "Data";
    public int Count
        {
            get { return count; }
        }

    public LinkedListNode<T>? First
        {
            get { return head; }
        }

    public LinkedListNode<T>? Last
            {
                
            }
    
    //IEnumerator<T> GetEnumerator();
    //IMyEnumerator<T> GetMyEnumerator();
}


}