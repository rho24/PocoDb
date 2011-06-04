using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class RemoveFromCollection
    {
        public IPocoId CollectionId { get; private set; }
        public object Value { get; private set; }

        public RemoveFromCollection(IPocoId collectionId, object value) {
            Value = value;
            CollectionId = collectionId;
        }
    }
}