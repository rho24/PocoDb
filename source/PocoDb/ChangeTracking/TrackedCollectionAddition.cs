using System;

namespace PocoDb.ChangeTracking
{
    public class TrackedCollectionAddition : ITrackedChange
    {
        public object Collection { get; private set; }
        public object Value { get; private set; }

        public TrackedCollectionAddition(object collection, object value) {
            Collection = collection;
            Value = value;
        }
    }
}