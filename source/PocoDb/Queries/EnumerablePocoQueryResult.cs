using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public class EnumerablePocoQueryResult : IQueryResult
    {
        public IEnumerable<IPocoId> Ids { get; set; }
        public IEnumerable<IPocoMeta> Metas { get; set; }
    }
}