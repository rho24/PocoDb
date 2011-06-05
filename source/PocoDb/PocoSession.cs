using System;
using System.Linq;
using PocoDb.Linq;
using PocoDb.Server;

namespace PocoDb
{
    public class PocoSession : IPocoSession
    {
        public IPocoDbServer Server { get; private set; }

        public PocoSession(IPocoDbServer server) {
            Server = server;
        }

        public IQueryable<T> Get<T>() {
            return new PocoQueryable<T>(Server);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}