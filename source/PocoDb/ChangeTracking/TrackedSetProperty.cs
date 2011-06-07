using System;
using PocoDb.Meta;

namespace PocoDb.ChangeTracking
{
    public class TrackedSetProperty : ITrackedChange
    {
        public object Poco { get; private set; }
        public IProperty Property { get; private set; }
        public object Value { get; private set; }

        public TrackedSetProperty(object poco, IProperty property, object value) {
            Poco = poco;
            Property = property;
            Value = value;
        }
    }
}