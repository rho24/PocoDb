using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Meta;
using PocoDb.Persistence;

namespace PocoDb.Specs.Persistence
{
    [Subject(typeof (InMemoryMetaStore))]
    public class with_a_new_InMemoryMetaStore : Observes<InMemoryMetaStore>
    {
        Establish c = () => { };
    }

    [Subject(typeof (InMemoryMetaStore))]
    public class with_populated_InMemoryMetaStore : with_a_new_InMemoryMetaStore
    {
        Establish c = () => {
            id = fake.an<IPocoId>();
            var meta = new PocoMeta(id, typeof (DummyObject));

            sut_setup.run(sut => sut.AddNew(meta));
        };

        protected static IPocoId id;
    }
}