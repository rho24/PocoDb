using System;

namespace PocoDb.Meta
{
    public interface IMetaStore
    {
        void Add(IPocoMeta meta);
    }
}