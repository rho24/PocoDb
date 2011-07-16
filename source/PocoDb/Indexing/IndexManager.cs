using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Indexing
{
    public class IndexManager : IIndexManager
    {
        public IDictionary<Type, IIndex> TypeIndexes { get; private set; }
        public IEnumerable<IIndex> Indexes { get { return TypeIndexes.Values; } }
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public IndexManager() {
            TypeIndexes = new Dictionary<Type, IIndex>();
        }

        public IndexMatch RetrieveIndex(Expression expression) {
            if (!_lock.TryEnterReadLock(TimeSpan.FromMilliseconds(100)))
                throw new ApplicationException("Could not enter read lock");

            try {
                IndexMatch currentMatch = IndexMatch.NoMatch();

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
            finally {
                _lock.ExitReadLock();
            }
        }

        public void NotifyMetaChange(IPocoMeta meta) {
            if (!_lock.TryEnterWriteLock(TimeSpan.FromMilliseconds(100)))
                throw new ApplicationException("Could not enter write lock");

            try {
                if (!TypeIndexes.ContainsKey(meta.Type))
                    TypeIndexes.Add(meta.Type,
                                    (IIndex) GenericHelper.InvokeGeneric(() => new TypeIndex<object>(), meta.Type));

                foreach (var index in Indexes)
                    index.NotifyMetaChange(meta);
            }
            finally {
                _lock.ExitWriteLock();
            }
        }
    }
}