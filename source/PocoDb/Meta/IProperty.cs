using System;

namespace PocoDb.Meta
{
    public interface IProperty
    {
        void Set(object poco, object value);
        object Get(object poco);
    }
}