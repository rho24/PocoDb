using System;
using System.Collections.Generic;

namespace PocoDb.Meta
{
    public class PocoMeta : IPocoMeta
    {
        public IPocoId Id { get; private set; }
        public IDictionary<IProperty, object> Properties { get; private set; }
        public ICollection<object> Collection { get; private set; }
        public Type Type { get; private set; }

        public PocoMeta(IPocoId id, Type type) {
            Id = id;
            Type = type;
            Properties = new Dictionary<IProperty, object>();
            Collection = new List<object>();
        }
    }
}