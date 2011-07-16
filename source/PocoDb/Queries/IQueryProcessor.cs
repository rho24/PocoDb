using System;

namespace PocoDb.Queries
{
    public interface IQueryProcessor
    {
        SingleQueryResult ProcessSingle(IQuery query);
        ElementQueryResult ProcessElement(IQuery query);
        EnumerableQueryResult ProcessEnumerable(IQuery query);
    }
}