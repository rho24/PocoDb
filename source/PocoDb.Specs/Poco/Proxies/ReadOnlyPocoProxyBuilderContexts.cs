using System;
using Machine.Specifications;
using PocoDb.Pocos.Proxies;
using PocoDb.Session;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Poco.Proxies
{
    [Subject(typeof (ReadOnlyPocoProxyBuilder))]
    public class with_a_new_ReadOnlyPocoProxyBuilder : Observes<ReadOnlyPocoProxyBuilder>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();
            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IInternalPocoSession session;
    }
}