using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public class PocoQueryResult : IPocoQueryResult
    {
        public IEnumerable<IPocoMeta> Metas { get; set; }
        public IEnumerable<IPocoId> Ids { get; set; }

        public PocoQueryResult() {
            Metas = new List<IPocoMeta>();
            Ids = new List<IPocoId>();
        }
    }
}