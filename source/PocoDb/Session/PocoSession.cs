using System;
using System.Linq;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Server;

namespace PocoDb.Session
{
    public class PocoSession : IPocoSession, IInternalPocoSession
    {
        public IPocoDbServer Server { get; private set; }
        public IPocoFactory PocoFactory { get; private set; }
        public IIdsMetasAndProxies IdsMetasAndProxies { get; private set; }


        public PocoSession(IPocoDbServer server, IPocoFactory pocoFactory) {
            Server = server;
            PocoFactory = pocoFactory;

            IdsMetasAndProxies = new IdsMetasAndProxies();
        }

        //IPocoSession
        public IQueryable<T> Get<T>() {
            return new PocoQueryable<T>(new PocoQueryProvider(new PocoQueryableExecutor(this)));
        }

        //IInternalPocoSession
        public object GetPoco(IPocoId id) {
            if (IdsMetasAndProxies.Metas.ContainsKey(id))
                return PocoFactory.Build(IdsMetasAndProxies.Metas[id], IdsMetasAndProxies);
            else {
                var meta = Server.GetMeta(id);

                if (meta == null)
                    throw new ArgumentException("id is not recognised");

                IdsMetasAndProxies.Metas.Add(meta.Id, meta);
                return PocoFactory.Build(meta, IdsMetasAndProxies);
            }
        }

        public void Dispose() {}
    }
}