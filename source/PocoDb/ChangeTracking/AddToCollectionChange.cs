namespace PocoDb.ChangeTracking
{
    public class AddToCollectionChange
    {
        public object Collection { get; private set; }
        public object Object { get; private set; }

        public AddToCollectionChange(object collection, object o) {
            Collection = collection;
            Object = o;
        }
    }
}