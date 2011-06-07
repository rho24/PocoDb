using System;
using PocoDb.Meta;

namespace PocoDb.ChangeTracking
{
    public interface IChangeTracker
    {
        ITrackedChanges Changes { get; }

        void TrackAddedObject(object poco);
        void TrackPropertySet(object poco, IProperty property, object value);
        void TrackAddToCollection(object collection, object value);
        void TrackRemoveFromCollection(object collection, object value);
    }
}