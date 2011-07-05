using System;
using Machine.Specifications;
using PocoDb.Pocos;
using PocoDb.Pocos.Proxies;
using PocoDb.Session;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Poco
{
    [Subject(typeof (IPocoFactory))]
    public class with_a_new_PocoFactory : Observes<PocoFactory>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();

            proxyBuilder = depends.on<IPocoProxyBuilder>();
            collectionProxyBuilder = depends.on<ICollectionProxyBuilder>();
            idsMetasAndProxies = new IdsMetasAndProxies();
        };

        protected static IInternalPocoSession session;
        protected static IPocoProxyBuilder proxyBuilder;
        protected static ICollectionProxyBuilder collectionProxyBuilder;
        protected static IIdsMetasAndProxies idsMetasAndProxies;
    }
}