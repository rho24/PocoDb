using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Meta;

namespace PocoDb.ChangeTracking
{
    public class TrackedChanges : ITrackedChanges
    {
        public List<AddObjectChange> AddObjectChanges { get; private set; }
        public List<PropertySetChange> PropertySetChanges { get; private set; }
        public List<AddToCollectionChange> AddToCollectionChanges { get; private set; }
        public List<RemoveFromCollectionChange> RemoveFromCollectionChanges { get; private set; }

        public TrackedChanges() {
            AddObjectChanges = new List<AddObjectChange>();
            PropertySetChanges = new List<PropertySetChange>();
            AddToCollectionChanges = new List<AddToCollectionChange>();
            RemoveFromCollectionChanges = new List<RemoveFromCollectionChange>();
        }

        public void TrackAddedObject(object poco) {
            if (poco == null)
                throw new ArgumentNullException("poco");

            if (AddObjectChanges.Any(a => a.Poco == poco))
                return;

            AddObjectChanges.Add(new AddObjectChange(poco));
        }

        public void TrackPropertySet(object poco, IProperty property, object value) {
            if (poco == null)
                throw new ArgumentNullException("poco");

            if (property == null)
                throw new ArgumentNullException("property");

            if (PropertySetChanges.Any(p => p.Poco == poco && p.Property == property))
                PropertySetChanges.RemoveAll(p => p.Poco == poco && p.Property == property);

            PropertySetChanges.Add(new PropertySetChange(poco, property, value));
        }

        public void TrackAddToCollection(ICollection collection, object value) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            AddToCollectionChanges.Add(new AddToCollectionChange(collection, value));
        }

        public void TrackRemoveFromCollection(ICollection collection, object value) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            RemoveFromCollectionChanges.Add(new RemoveFromCollectionChange(collection, value));
        }

        IEnumerable<AddObjectChange> ITrackedChanges.AddObjectChanges { get { return AddObjectChanges; } }

        IEnumerable<PropertySetChange> ITrackedChanges.PropertySetChanges { get { return PropertySetChanges; } }

        IEnumerable<AddToCollectionChange> ITrackedChanges.AddToCollectionChanges { get { return AddToCollectionChanges; } }

        IEnumerable<RemoveFromCollectionChange> ITrackedChanges.RemoveFromCollectionChanges { get { return RemoveFromCollectionChanges; } }
    }
}