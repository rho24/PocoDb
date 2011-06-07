using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Persistence
{
    public interface IMetaStore
    {
        IPocoMeta Get(IPocoId id);
        IEnumerable<IPocoMeta> Get(IEnumerable<IPocoId> ids);

        IPocoMeta GetWritable(IPocoId id);
        void AddNew(IPocoMeta meta);
        void Update(IPocoMeta meta);
    }
}