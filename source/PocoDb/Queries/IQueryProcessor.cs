using System;

namespace PocoDb.Queries
{
    public interface IQueryProcessor
    {
        IQueryResult Process(IQuery query);
    }
}