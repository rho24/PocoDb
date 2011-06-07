using System;
using System.Collections.Generic;
using System.Linq;

namespace PocoDb.ChangeTracking
{
    public class TrackedChanges : ITrackedChanges
    {
        public IEnumerable<ITrackedChange> AllChanges {
            get {
                return
                    AddedPocos.Cast<ITrackedChange>()
                        .Concat(SetProperties)
                        .Concat(CollectionAdditions)
                        .Concat(CollectionRemovals);
            }
        }

        public List<TrackedAddedPoco> AddedPocos { get; private set; }
        public List<TrackedSetProperty> SetProperties { get; private set; }
        public List<TrackedCollectionAddition> CollectionAdditions { get; private set; }
        public List<TrackedCollectionRemoval> CollectionRemovals { get; private set; }

        public TrackedChanges() {
            AddedPocos = new List<TrackedAddedPoco>();
            SetProperties = new List<TrackedSetProperty>();
            CollectionAdditions = new List<TrackedCollectionAddition>();
            CollectionRemovals = new List<TrackedCollectionRemoval>();
        }

        IEnumerable<TrackedAddedPoco> ITrackedChanges.AddedPocos { get { return AddedPocos; } }
        IEnumerable<TrackedSetProperty> ITrackedChanges.SetProperties { get { return SetProperties; } }
        IEnumerable<TrackedCollectionAddition> ITrackedChanges.CollectionAdditions { get { return CollectionAdditions; } }
        IEnumerable<TrackedCollectionRemoval> ITrackedChanges.CollectionRemovals { get { return CollectionRemovals; } }
    }
}