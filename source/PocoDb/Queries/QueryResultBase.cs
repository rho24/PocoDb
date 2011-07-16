using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public abstract class QueryResultBase : IQueryResult
    {
        public IEnumerable<IPocoMeta> Metas { get; set; }

        protected QueryResultBase() {
            Metas = Enumerable.Empty<IPocoMeta>();
        }
    }
}