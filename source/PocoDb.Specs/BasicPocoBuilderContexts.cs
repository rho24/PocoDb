using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Pocos;
using PocoDb.Session;

namespace PocoDb.Specs
{
    [Subject(typeof (BasicPocoBuilder))]
    public class with_a_new_BasicPocoBuilder : Observes<BasicPocoBuilder>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();
            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IInternalPocoSession session;
    }
}