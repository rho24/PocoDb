using System;
using System.Linq.Expressions;

namespace PocoDb.Indexing
{
    public interface IIndexManager
    {
        IIndex RetrieveIndex(Expression expression);
    }
}