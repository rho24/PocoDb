using System;
using System.Collections.Generic;

namespace PocoDb.ChangeTracking
{
    public interface ITrackedChanges
    {
        IEnumerable<ITrackedChange> AllChanges { get; }
        IEnumerable<TrackedAddedPoco> AddedPocos { get; }
        IEnumerable<TrackedSetProperty> SetProperties { get; }
        IEnumerable<TrackedCollectionAddition> CollectionAdditions { get; }
        IEnumerable<TrackedCollectionRemoval> CollectionRemovals { get; }
    }
}