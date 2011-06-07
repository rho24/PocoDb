using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Meta;
using PocoDb.Session;

namespace PocoDb.Specs.Meta
{
    [Subject(typeof (PocoMetaBuilder))]
    public class with_a_new_PocoMetaBuilder : Observes<PocoMetaBuilder>
    {
        Establish c = () => {
            pocoIdBuilder = depends.on<IPocoIdBuilder>();
            session = fake.an<IInternalWritablePocoSession>();

            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IPocoIdBuilder pocoIdBuilder;
        protected static IInternalWritablePocoSession session;
    }
}