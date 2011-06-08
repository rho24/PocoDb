using System;
using System.Linq.Expressions;
using PocoDb.Meta;

namespace PocoDb.Indexing
{
    public interface IIndexManager
    {
        IndexMatch RetrieveIndex(Expression expression);
        void NotifyMetaChange(IPocoMeta meta);
    }
}