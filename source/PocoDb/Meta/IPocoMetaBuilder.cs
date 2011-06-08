using System;
using System.Collections.Generic;
using PocoDb.Pocos;

namespace PocoDb.Meta
{
    public interface IPocoMetaBuilder
    {
        IEnumerable<IPocoMeta> Build(object poco, IIdsMetasAndProxies idsMetasAndProxies);
    }
}