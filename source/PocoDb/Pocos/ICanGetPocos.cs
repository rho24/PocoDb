using System;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public interface ICanGetPocos
    {
        object GetPoco(IPocoId id);
    }
}