using System.Collections;

namespace PocoDb.ChangeTracking
{
    public class RemoveFromCollectionChange
    {
        public ICollection Collection { get; private set; }
        public object Value { get; private set; }

        public RemoveFromCollectionChange(ICollection collection, object value) {
            Collection = collection;
            Value = value;
        }
    }
}