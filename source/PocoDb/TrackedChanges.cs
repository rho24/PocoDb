using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocoDb
{
    public class TrackedChanges
    {
        public IEnumerable<AddObjectChange> AddObjectChanges { get; private set; }
        public IEnumerable<PropertySetChange> PropertySetChanges { get; private set; }
        public IEnumerable<AddToCollectionChange> AddToCollectionChanges { get; private set; }
        public IEnumerable<RemoveFromCollectionChange> RemoveFromCollectionChanges { get; private set; }

        public void TrackAddedObject(object obj)
        {
            throw new NotImplementedException();
        }

        public object TrackPropertySet(object obj, Property prop, object val)
        {
            throw new NotImplementedException();
        }

        public object TrackAddToCollection(ICollection<object> collection, object obj)
        {
            throw new NotImplementedException();
        }

        public object TrackRemoveFromCollection(ICollection<object> collection, object obj)
        {
            throw new NotImplementedException();
        }
    }
}
