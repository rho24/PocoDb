using System;

namespace PocoDb.Meta
{
    public interface IMetaStore
    {
        void Save(IPocoMeta meta);
        IPocoMeta Get(IPocoId id);
    }
}