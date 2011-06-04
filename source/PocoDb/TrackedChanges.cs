using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocoDb
{
    public interface ITrackedChanges
    {
        IEnumerable<AddObjectChange> AddObjectChanges { get; }
        IEnumerable<PropertySetChange> PropertySetChanges { get; }
        IEnumerable<AddToCollectionChange> AddToCollectionChanges { get; }
        IEnumerable<RemoveFromCollectionChange> RemoveFromCollectionChanges { get; }

        void TrackAddedObject(object obj);
        void TrackPropertySet(object obj, Property prop, object val);
        void TrackAddToCollection(ICollection<object> collection, object obj);
        void TrackRemoveFromCollection(ICollection<object> collection, object obj);
    }

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

        public void TrackAddedObject(object obj) {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (AddObjectChanges.Any(a => a.Object == obj))
                return;

            AddObjectChanges.Add(new AddObjectChange(obj));
        }

        public void TrackPropertySet(object obj, Property prop, object val) {
            if (obj == null)
                throw new ArgumentNullException("obj");

            if (prop == null)
                throw new ArgumentNullException("prop");

            if (PropertySetChanges.Any(p => p.Object == obj && p.Property == prop))
                PropertySetChanges.RemoveAll(p => p.Object == obj && p.Property == prop);

            PropertySetChanges.Add(new PropertySetChange(obj, prop, val));
        }

        public void TrackAddToCollection(ICollection<object> collection, object obj) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            AddToCollectionChanges.Add(new AddToCollectionChange(collection, obj));
        }

        public void TrackRemoveFromCollection(ICollection<object> collection, object obj) {
            if (collection == null)
                throw new ArgumentNullException("collection");

            RemoveFromCollectionChanges.Add(new RemoveFromCollectionChange(collection, obj));
        }

        IEnumerable<AddObjectChange> ITrackedChanges.AddObjectChanges { get { return AddObjectChanges; } }

        IEnumerable<PropertySetChange> ITrackedChanges.PropertySetChanges { get { return PropertySetChanges; } }

        IEnumerable<AddToCollectionChange> ITrackedChanges.AddToCollectionChanges { get { return AddToCollectionChanges; } }

        IEnumerable<RemoveFromCollectionChange> ITrackedChanges.RemoveFromCollectionChanges { get { return RemoveFromCollectionChanges; } }
    }
}