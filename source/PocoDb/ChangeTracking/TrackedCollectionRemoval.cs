using System;

namespace PocoDb.ChangeTracking
{
    public class TrackedCollectionRemoval : ITrackedChange
    {
        public object Collection { get; private set; }
        public object Value { get; private set; }

        public TrackedCollectionRemoval(object collection, object value) {
            Collection = collection;
            Value = value;
        }
    }
}