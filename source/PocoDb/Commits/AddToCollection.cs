using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class AddToCollection
    {
        public IPocoId CollectionId { get; private set; }
        public object Value { get; private set; }

        public AddToCollection(IPocoId collectionId, object value) {
            Value = value;
            CollectionId = collectionId;
        }
    }
}