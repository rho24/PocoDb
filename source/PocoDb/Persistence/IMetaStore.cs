using System;
using PocoDb.Meta;

namespace PocoDb.Persistence
{
    public interface IMetaStore
    {
        void Save(IPocoMeta meta);
        IPocoMeta Get(IPocoId id);
    }
}