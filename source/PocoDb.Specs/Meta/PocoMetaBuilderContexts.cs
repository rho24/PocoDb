using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Meta;
using PocoDb.Pocos;

namespace PocoDb.Specs.Meta
{
    [Subject(typeof (PocoMetaBuilder))]
    public class with_a_new_PocoMetaBuilder : Observes<PocoMetaBuilder>
    {
        Establish c = () => {
            pocoIdBuilder = depends.on<IPocoIdBuilder>();
            idsMetasAndProxies = new IdsMetasAndProxies();
        };

        protected static IPocoIdBuilder pocoIdBuilder;
        protected static IIdsMetasAndProxies idsMetasAndProxies;
    }
}