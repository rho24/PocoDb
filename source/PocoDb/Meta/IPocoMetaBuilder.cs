using System;

namespace PocoDb.Meta
{
    public interface IPocoMetaBuilder
    {
        IPocoMeta Build(object poco);
        IPocoId Resolve(object poco);
    }
}