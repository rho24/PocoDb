using System;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Pocos
{
    public class PocoFactory : IPocoFactory
    {
        public IInternalPocoSession Session { get; private set; }
        protected IPocoProxyBuilder PocoProxyBuilder { get; private set; }

        public PocoFactory(IPocoProxyBuilder pocoProxyBuilder) {
            PocoProxyBuilder = pocoProxyBuilder;
        }

        public void Initialise(IInternalPocoSession session) {
            Session = session;
        }

        public object Build(IPocoMeta meta) {
            if (Session.TrackedPocos.ContainsKey(meta.Id))
                return Session.TrackedPocos[meta.Id];

            var proxy = PocoProxyBuilder.BuildProxy(meta);

            Session.TrackedPocos.Add(meta.Id, proxy);

            return proxy;
        }
    }
}