using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public interface IQueryResult
    {
        IEnumerable<IPocoMeta> Metas { get; }
    }
}