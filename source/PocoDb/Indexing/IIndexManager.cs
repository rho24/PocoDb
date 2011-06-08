using System;
using System.Linq.Expressions;

namespace PocoDb.Indexing
{
    public interface IIndexManager
    {
        IndexMatch RetrieveIndex(Expression expression);
    }
}