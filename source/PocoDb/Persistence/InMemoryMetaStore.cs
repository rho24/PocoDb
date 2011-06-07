using System;
using PocoDb.Meta;

namespace PocoDb.Persistence
{
    public class InMemoryMetaStore : IMetaStore
    {
        public void Save(IPocoMeta meta) {
            throw new NotImplementedException();
        }

        public IPocoMeta Get(IPocoId id) {
            throw new NotImplementedException();
        }
    }
}