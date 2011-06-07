using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class CollectionRemoval
    {
        public IPocoId CollectionId { get; private set; }
        public object Value { get; private set; }

        public CollectionRemoval(IPocoId collectionId, object value) {
            CollectionId = collectionId;
            Value = value;
        }
    }
}