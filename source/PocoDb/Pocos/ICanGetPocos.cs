using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public interface ICanGetPocos
    {
        object GetPoco(IPocoId id);
        IEnumerable<object> GetPocos(IEnumerable<IPocoId> ids);
    }
}