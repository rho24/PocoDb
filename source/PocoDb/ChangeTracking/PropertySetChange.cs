using PocoDb.Meta;
namespace PocoDb.ChangeTracking
{
    public class PropertySetChange
    {
        public object Object { get; private set; }
        public IProperty Property { get; private set; }
        public object Value { get; private set; }

        public PropertySetChange(object o, IProperty property, object value) {
            Object = o;
            Property = property;
            Value = value;
        }
    }
}