using System.Collections.Generic;
using PocoDb.Meta;
using System.Collections;

namespace PocoDb.ChangeTracking
{
    public interface ITrackedChanges
    {
        IEnumerable<AddObjectChange> AddObjectChanges { get; }
        IEnumerable<PropertySetChange> PropertySetChanges { get; }
        IEnumerable<AddToCollectionChange> AddToCollectionChanges { get; }
        IEnumerable<RemoveFromCollectionChange> RemoveFromCollectionChanges { get; }

        void TrackAddedObject(object obj);
        void TrackPropertySet(object obj, IProperty prop, object val);
        void TrackAddToCollection(ICollection collection, object obj);
        void TrackRemoveFromCollection(ICollection collection, object obj);
    }
}