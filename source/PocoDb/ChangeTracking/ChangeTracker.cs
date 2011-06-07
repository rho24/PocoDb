using System;
using System.Linq;
using PocoDb.Meta;

namespace PocoDb.ChangeTracking
{
    public class ChangeTracker : IChangeTracker
    {
        public TrackedChanges Changes { get; private set; }
        ITrackedChanges IChangeTracker.Changes { get { return Changes; } }

        public ChangeTracker() {
            Changes = new TrackedChanges();
        }

        public void TrackAddedObject(object poco) {
            if (poco == null)
                throw new ArgumentNullException("poco");

            if (Changes.AddedPocos.Any(a => a.Poco == poco))
                return;

            Changes.AddedPocos.Add(new TrackedAddedPoco(poco));
        }

        public void TrackPropertySet(object poco, IProperty property, object value) {
            if (poco == null)
                throw new ArgumentNullException("poco");

            if (property == null)
                throw new ArgumentNullException("property");

            if (Changes.SetProperties.Any(p => p.Poco == poco && p.Property == property))
                Changes.SetProperties.RemoveAll(p => p.Poco == poco && p.Property == property);

            Changes.SetProperties.Add(new TrackedSetProperty(poco, property, value));
        }

        public void TrackAddToCollection(object collection, object value) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            Changes.CollectionAdditions.Add(new TrackedCollectionAddition(collection, value));
        }

        public void TrackRemoveFromCollection(object collection, object value) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            Changes.CollectionRemovals.Add(new TrackedCollectionRemoval(collection, value));
        }
    }
}