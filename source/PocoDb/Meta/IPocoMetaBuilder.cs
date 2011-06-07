using System;
using System.Collections.Generic;

namespace PocoDb.Meta
{
    public interface IPocoMetaBuilder
    {
        IEnumerable<IPocoMeta> Build(object poco);
    }
}