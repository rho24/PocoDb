using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Indexing
{
    public class IndexManager : IIndexManager
    {
        public IDictionary<Type, IIndex> TypeIndexes { get; private set; }
        public IEnumerable<IIndex> Indexes { get { return TypeIndexes.Values; } }

        public IndexManager() {
            TypeIndexes = new Dictionary<Type, IIndex>();
        }

        public IndexMatch RetrieveIndex(Expression expression) {
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

            return currentMatch;
        }

        public void NotifyMetaChange(IPocoMeta meta) {
            if (!TypeIndexes.ContainsKey(meta.Type))
                TypeIndexes.Add(meta.Type,
                                (IIndex) GenericHelper.InvokeGeneric(() => new TypeIndex<object>(), meta.Type));

            foreach (var index in Indexes)
                index.NotifyMetaChange(meta);
        }
    }
}