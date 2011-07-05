using System;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Meta;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Indexing
{
    [Subject(typeof (TypeIndex<>))]
    public class with_a_new_TypeIndex : Observes<TypeIndex<DummyObject>>
    {
        Establish c = () => { };
    }

    [Subject(typeof (TypeIndex<>))]
    public class with_a_populated_TypeIndex : with_a_new_TypeIndex
    {
        Establish c = () => {
            id = fake.an<IPocoId>();
            sut_setup.run(sut => sut.Ids.Add(id));
        };

        protected static IPocoId id;
    }
}