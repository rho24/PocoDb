using System;
using PocoDb.Meta;

namespace PocoDb.Queries
{
    public class ElementQueryResult : QueryResultBase
    {
        public IPocoId ElementId { get; set; }
    }
}