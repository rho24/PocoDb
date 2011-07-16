using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public class EnumerableQueryResult : QueryResultBase
    {
        public IEnumerable<IPocoId> ElementIds { get; set; }
    }
}