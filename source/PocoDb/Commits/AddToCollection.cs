using System;
using System.Collections;

namespace PocoDb.Commits
{
    public class AddToCollection
    {
        public ICollection Collection { get; private set; }
        public object Value { get; private set; }

        public AddToCollection(ICollection collection, object value) {
            Value = value;
            Collection = collection;
        }
    }
}