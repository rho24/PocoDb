using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;

namespace PocoDb.Server
{
    public class ServerPocoGetter : ICanGetPocos
    {
        public IPocoDbServer Server { get; private set; }
        public IIdsMetasAndProxies IdsMetasAndProxies { get; private set; }

        public ServerPocoGetter(IPocoDbServer server) {
            Server = server;
            IdsMetasAndProxies = new IdsMetasAndProxies();
        }

        public object GetPoco(IPocoId id) {
            var meta = Server.MetaStore.Get(id);

            if (meta == null)
                throw new ArgumentException("id is not recognised");

            var pocoProxyBuilder = new ReadOnlyPocoProxyBuilder();
            var collectionProxyBuilder = new ReadOnlyCollectionProxyBuilder();
            pocoProxyBuilder.Initialise(this);
            collectionProxyBuilder.Initialise(this);
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);

            return pocoFactory.Build(meta, IdsMetasAndProxies);
        }

        public IEnumerable<object> GetPocos(IEnumerable<IPocoId> ids) {
            var metas = Server.MetaStore.Get(ids);

            var pocoProxyBuilder = new ReadOnlyPocoProxyBuilder();
            var collectionProxyBuilder = new ReadOnlyCollectionProxyBuilder();
            pocoProxyBuilder.Initialise(this);
            collectionProxyBuilder.Initialise(this);
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);

            return metas.Select(meta => pocoFactory.Build(meta, IdsMetasAndProxies));
        }
    }
}