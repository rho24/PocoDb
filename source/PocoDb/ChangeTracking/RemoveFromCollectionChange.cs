using System;

namespace PocoDb.ChangeTracking
{
    public class RemoveFromCollectionChange
    {
        public object Collection { get; private set; }
        public object Value { get; private set; }

        public RemoveFromCollectionChange(object collection, object value) {
            Collection = collection;
            Value = value;
        }
    }
}