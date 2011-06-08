using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoDb.Indexing
{
    public class IndexManager : IIndexManager
    {
        public List<IIndex> Indexes { get; private set; }

        public IndexManager() {
            Indexes = new List<IIndex>();
        }

        public IIndex RetrieveIndex(Expression expression) {
            var exactIndex = Indexes.Where(i => i.IsExactMatch(expression)).FirstOrDefault();

            if (exactIndex != null)
                return exactIndex;

            throw new NotImplementedException();
        }
    }
}