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
            AddObjectChanges.Add(new AddObjectChange(obj));
        }

        public void TrackPropertySet(object obj, Property prop, object val) {
            PropertySetChanges.Add(new PropertySetChange(obj, prop, val));
        }

        public void TrackAddToCollection(ICollection<object> collection, object obj) {
            AddToCollectionChanges.Add(new AddToCollectionChange(collection, obj));
        }

        public void TrackRemoveFromCollection(ICollection<object> collection, object obj) {
            RemoveFromCollectionChanges.Add(new RemoveFromCollectionChange(collection, obj));
        }

        IEnumerable<AddObjectChange> ITrackedChanges.AddObjectChanges {
            get { return AddObjectChanges; }
        }

        IEnumerable<PropertySetChange> ITrackedChanges.PropertySetChanges {
            get { return PropertySetChanges; }
        }

        IEnumerable<AddToCollectionChange> ITrackedChanges.AddToCollectionChanges {
            get { return AddToCollectionChanges; }
        }

        IEnumerable<RemoveFromCollectionChange> ITrackedChanges.RemoveFromCollectionChanges {
            get { return RemoveFromCollectionChanges; }
        }
    }
}