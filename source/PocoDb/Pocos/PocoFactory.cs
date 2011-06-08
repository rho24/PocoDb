using System;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class PocoFactory : IPocoFactory
    {
        public IInternalPocoSession Session { get; private set; }
        protected IPocoProxyBuilder PocoProxyBuilder { get; private set; }
        protected ICollectionProxyBuilder CollectionProxyBuilder { get; private set; }

        public PocoFactory(IPocoProxyBuilder pocoProxyBuilder, ICollectionProxyBuilder collectionProxyBuilder) {
            PocoProxyBuilder = pocoProxyBuilder;
            CollectionProxyBuilder = collectionProxyBuilder;
        }

        public void Initialise(IInternalPocoSession session) {
            Session = session;
        }

        public object Build(IPocoMeta meta) {
            if (Session.TrackedPocos.ContainsKey(meta.Id))
                return Session.TrackedPocos[meta.Id];

            var proxy = meta.Type.IsCollectionType()
                            ? CollectionProxyBuilder.BuildProxy(meta)
                            : PocoProxyBuilder.BuildProxy(meta);

            Session.TrackedPocos.Add(meta.Id, proxy);
            Session.TrackedIds.Add(proxy, meta.Id);

            return proxy;
        }
    }
}