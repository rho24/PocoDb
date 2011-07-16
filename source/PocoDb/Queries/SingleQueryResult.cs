using System;

namespace PocoDb.Queries
{
    public class SingleQueryResult : ElementQueryResult
    {
        public bool HasMany { get; set; }
    }
}