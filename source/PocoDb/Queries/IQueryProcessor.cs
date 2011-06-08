using System;

namespace PocoDb.Queries
{
    public interface IQueryProcessor
    {
        IPocoQueryResult Process(IPocoQuery query);
    }
}