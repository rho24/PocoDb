using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public interface IPocoQueryResult
    {
        IEnumerable<IPocoMeta> Metas { get; }
        IEnumerable<IPocoId> Ids { get; }
    }
}