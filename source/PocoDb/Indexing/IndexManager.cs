using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PocoDb.Extensions;

namespace PocoDb.Indexing
{
    public class IndexManager : IIndexManager
    {
        public List<IIndex> Indexes { get; private set; }

        public IndexManager() {
            Indexes = new List<IIndex>();
        }

        public IndexMatch RetrieveIndex(Expression expression) {
            if (!expression.IsQuery())
                throw new ArgumentException("expression is not a PocoQuery");

            IndexMatch currentMatch = null;

            foreach (var index in Indexes) {
                var match = index.GetMatch(expression);

                if (match.IsExact)
                    return match;

                if (match.IsPartial) {
                    if (currentMatch == null || match.PartialDepth > currentMatch.PartialDepth)
                        currentMatch = match;
                }
            }

            if (currentMatch == null)
                throw new NoIndexFoundException(expression.QueryPocoType());

            return currentMatch;
        }
    }
}