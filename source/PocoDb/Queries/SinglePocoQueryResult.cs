using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public class SinglePocoQueryResult : IQueryResult
    {
        public IPocoId Id { get; set; }
        public IEnumerable<IPocoMeta> Metas { get; set; }
    }
}