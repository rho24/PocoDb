using System;
using System.Collections.Generic;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public interface IIdsMetasAndProxies
    {
        IDictionary<object, IPocoId> Ids { get; }
        IDictionary<IPocoId, IPocoMeta> Metas { get; }
        IDictionary<IPocoId, object> Pocos { get; }
    }

    public class IdsMetasAndProxies : IIdsMetasAndProxies
    {
        public IDictionary<object, IPocoId> Ids { get; private set; }
        public IDictionary<IPocoId, IPocoMeta> Metas { get; private set; }
        public IDictionary<IPocoId, object> Pocos { get; private set; }

        public IdsMetasAndProxies() {
            Ids = new Dictionary<object, IPocoId>();
            Metas = new Dictionary<IPocoId, IPocoMeta>();
            Pocos = new Dictionary<IPocoId, object>();
        }
    }
}