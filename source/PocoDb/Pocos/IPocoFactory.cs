using System;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public interface IPocoFactory
    {
        object Build(IPocoMeta meta, IIdsMetasAndProxies idsMetasAndProxies);
    }
}