using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class PropertySet
    {
        public IPocoId ParentId { get; private set; }
        public IProperty Property { get; private set; }
        public object Value { get; private set; }

        public PropertySet(IPocoId parentId, IProperty property, object value) {
            ParentId = parentId;
            Property = property;
            Value = value;
        }
    }
}