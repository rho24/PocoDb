using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Server;

namespace PocoDb.Session
{
    public class PocoSession : IPocoSession, IInternalPocoSession
    {
        public IPocoDbServer Server { get; private set; }
        public ICollection<IPocoMeta> Metas { get; private set; }

        public PocoSession(IPocoDbServer server) {
            Server = server;
            Metas = new List<IPocoMeta>();
        }

        public IQueryable<T> Get<T>() {
            return new PocoQueryable<T>(new PocoQueryProvider(new PocoQueryableExecutor(this)));
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public object GetPoco(IPocoId id) {
            throw new NotImplementedException();
        }
    }
}