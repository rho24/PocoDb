using System;
using System.Collections.Generic;
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
        public IDictionary<IPocoId, IPocoMeta> Metas { get; private set; }
        public IDictionary<IPocoId, object> TrackedPocos { get; private set; }
        public IDictionary<object, IPocoId> TrackedIds { get; private set; }

        public PocoSession(IPocoDbServer server, IPocoFactory pocoFactory) {
            Server = server;
            PocoFactory = pocoFactory;

            Metas = new Dictionary<IPocoId, IPocoMeta>();
            TrackedPocos = new Dictionary<IPocoId, object>();
            TrackedIds = new Dictionary<object, IPocoId>();
        }

        //IPocoSession
        public IQueryable<T> Get<T>() {
            return new PocoQueryable<T>(new PocoQueryProvider(new PocoQueryableExecutor(this)));
        }

        //IInternalPocoSession
        public object GetPoco(IPocoId id) {
            var meta = Metas[id];
            if (meta == null)
                throw new ArgumentException("id is not recognised");

            return PocoFactory.Build(meta);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}