﻿using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.ChangeTracking
{
    public interface ITrackedChanges
    {
        IEnumerable<AddObjectChange> AddObjectChanges { get; }
        IEnumerable<PropertySetChange> PropertySetChanges { get; }
        IEnumerable<AddToCollectionChange> AddToCollectionChanges { get; }
        IEnumerable<RemoveFromCollectionChange> RemoveFromCollectionChanges { get; }

        void TrackAddedObject(object poco);
        void TrackPropertySet(object poco, IProperty property, object value);
        void TrackAddToCollection(object collection, object value);
        void TrackRemoveFromCollection(object collection, object value);
    }
}