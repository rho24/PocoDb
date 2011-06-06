using System;

namespace PocoDb.ChangeTracking
{
    public class AddToCollectionChange
    {
        public object Collection { get; private set; }
        public object Value { get; private set; }

        public AddToCollectionChange(object collection, object value) {
            Collection = collection;
            Value = value;
        }
    }
}