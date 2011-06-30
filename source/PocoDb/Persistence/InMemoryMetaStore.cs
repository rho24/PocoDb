using System;
using System.Collections.Generic;
using System.Threading;
using PocoDb.Meta;

namespace PocoDb.Persistence
{
    public class InMemoryMetaStore : IMetaStore
    {
        protected IDictionary<IPocoId, IPocoMeta> Metas { get; private set; }
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public InMemoryMetaStore() {
            Metas = new Dictionary<IPocoId, IPocoMeta>();
        }

        public IPocoMeta Get(IPocoId id) {
            if (!_lock.TryEnterReadLock(TimeSpan.FromMilliseconds(100)))
                throw new ApplicationException("Could not enter read lock");

            try {
                return Metas.ContainsKey(id) ? Metas[id] : null;
            }
            finally {
                _lock.ExitReadLock();
            }
        }

        public IEnumerable<IPocoMeta> Get(IEnumerable<IPocoId> ids) {
            foreach (var id in ids) {
                yield return Get(id);
            }
        }

        public IPocoMeta GetWritable(IPocoId id) {
            return Get(id);
        }

        public void AddNew(IPocoMeta meta) {
            if (!_lock.TryEnterWriteLock(TimeSpan.FromMilliseconds(100)))
                throw new ApplicationException("Could not enter write lock");

            try {
                if (Metas.ContainsKey(meta.Id))
                    throw new ArgumentException("meta is already in store");

                Metas.Add(meta.Id, meta);
            }
            finally {
                _lock.ExitWriteLock();
            }
        }

        public void Update(IPocoMeta meta) {
            if (!_lock.TryEnterWriteLock(TimeSpan.FromMilliseconds(100)))
                throw new ApplicationException("Could not enter write lock");

            try {
                if (!Metas.ContainsKey(meta.Id))
                    throw new ArgumentException("meta does not exist in store");

                Metas[meta.Id] = meta;
            }
            finally {
                _lock.ExitWriteLock();
            }
        }
    }
}