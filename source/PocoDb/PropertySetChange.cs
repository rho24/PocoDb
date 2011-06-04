using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocoDb
{
    public class PropertySetChange
    {
        public object Object { get; private set; }
        public Property Property { get; private set; }
        public object Value { get; private set; }

        public PropertySetChange(object o, Property property, object value) {
            Object = o;
            Property = property;
            Value = value;
        }
    }
}