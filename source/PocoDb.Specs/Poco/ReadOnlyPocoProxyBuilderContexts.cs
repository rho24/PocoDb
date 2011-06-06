using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Pocos;
using PocoDb.Session;

namespace PocoDb.Specs.Poco
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