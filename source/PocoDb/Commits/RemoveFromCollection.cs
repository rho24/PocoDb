using System.Collections;

namespace PocoDb.Commits
{
    public class RemoveFromCollection
    {
        public ICollection Collection { get; private set; }
        public object Value { get; private set; }

        public RemoveFromCollection(ICollection collection, object value) {
            Value = value;
            Collection = collection;
        }
    }
}