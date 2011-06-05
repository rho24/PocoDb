using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public class PocoQueryResult
    {
        public IEnumerable<IPocoMeta> Metas { get; private set; }
        public IEnumerable<IPocoId> Ids { get; private set; }

        public PocoQueryResult() {
            Metas = new List<IPocoMeta>();
            Ids = new List<IPocoId>();
        }
    }
}