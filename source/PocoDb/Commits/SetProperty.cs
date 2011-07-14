using System;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class SetProperty
    {
        public IProperty Property { get; private set; }
        public object Value { get; private set; }

        public SetProperty(IProperty property, object value) {
            Property = property;
            Value = value;
        }
    }
}