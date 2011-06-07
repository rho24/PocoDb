using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Persistence
{
    public class InMemoryMetaStore : IMetaStore
    {
        protected IDictionary<IPocoId, IPocoMeta> Metas { get; private set; }

        public InMemoryMetaStore() {
            Metas = new Dictionary<IPocoId, IPocoMeta>();
        }

        public IPocoMeta Get(IPocoId id) {
            if (Metas.ContainsKey(id))
                return Metas[id];

            return null;
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
            if (Metas.ContainsKey(meta.Id))
                throw new ArgumentException("meta is already in store");

            Metas.Add(meta.Id, meta);
        }

        public void Update(IPocoMeta meta) {
            if (!Metas.ContainsKey(meta.Id))
                throw new ArgumentException("meta does not exist in store");

            Metas[meta.Id] = meta;
        }
    }
}