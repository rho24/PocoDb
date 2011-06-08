using System;
using PocoDb.Extensions;
using PocoDb.Meta;

namespace PocoDb.Pocos
{
    public class PocoFactory : IPocoFactory
    {
        public IPocoProxyBuilder PocoProxyBuilder { get; private set; }
        public ICollectionProxyBuilder CollectionProxyBuilder { get; private set; }

        public PocoFactory(IPocoProxyBuilder pocoProxyBuilder, ICollectionProxyBuilder collectionProxyBuilder) {
            PocoProxyBuilder = pocoProxyBuilder;
            CollectionProxyBuilder = collectionProxyBuilder;
        }

        public object Build(IPocoMeta meta, IIdsMetasAndProxies idsMetasAndProxies) {
            if (idsMetasAndProxies.Pocos.ContainsKey(meta.Id))
                return idsMetasAndProxies.Pocos[meta.Id];

            var proxy = meta.Type.IsCollectionType()
                            ? CollectionProxyBuilder.BuildProxy(meta)
                            : PocoProxyBuilder.BuildProxy(meta);

            idsMetasAndProxies.Pocos.Add(meta.Id, proxy);
            idsMetasAndProxies.Ids.Add(proxy, meta.Id);

            return proxy;
        }
    }
}