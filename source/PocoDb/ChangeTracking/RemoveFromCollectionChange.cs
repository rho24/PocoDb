namespace PocoDb.ChangeTracking
{
    public class RemoveFromCollectionChange
    {
        public object Collection { get; private set; }
        public object Object { get; private set; }

        public RemoveFromCollectionChange(object collection, object o) {
            Collection = collection;
            Object = o;
        }
    }
}