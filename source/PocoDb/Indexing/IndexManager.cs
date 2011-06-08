using System;
using System.Linq.Expressions;

namespace PocoDb.Indexing
{
    public class IndexManager : IIndexManager
    {
        public IIndex RetrieveIndex(Expression expression) {
            throw new NotImplementedException();
        }
    }
}