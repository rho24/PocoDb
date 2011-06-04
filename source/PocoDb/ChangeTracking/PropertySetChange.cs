using System;
using PocoDb.Meta;

namespace PocoDb.ChangeTracking
{
    public class PropertySetChange
    {
        public object Poco { get; private set; }
        public IProperty Property { get; private set; }
        public object Value { get; private set; }

        public PropertySetChange(object poco, IProperty property, object value) {
            Poco = poco;
            Property = property;
            Value = value;
        }
    }
}