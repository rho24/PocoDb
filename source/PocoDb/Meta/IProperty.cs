using System;

namespace PocoDb.Meta
{
    public interface IProperty
    {
        string PropertyName { get; }
        void Set(object poco, object value);
        object Get(object poco);
    }
}