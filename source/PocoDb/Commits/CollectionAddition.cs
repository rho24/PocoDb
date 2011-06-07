using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class CollectionAddition
    {
        public IPocoId CollectionId { get; private set; }
        public object Value { get; private set; }

        public CollectionAddition(IPocoId collectionId, object value) {
            CollectionId = collectionId;
            Value = value;
        }
    }
}