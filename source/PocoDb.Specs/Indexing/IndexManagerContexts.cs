using System;
using Machine.Specifications;
using PocoDb.Indexing;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Indexing
{
    [Subject(typeof (IndexManager))]
    public class with_a_new_IndexManager : Observes<IndexManager>
    {}

    [Subject(typeof (IndexManager))]
    public class with_a_new_IndexManager_containing_a_TypeIndex : Observes<IndexManager>
    {
        Establish c = () => {
            typeIndex = new TypeIndex<DummyObject>();
            sut_setup.run(sut => sut.TypeIndexes.Add(typeof (DummyObject), typeIndex));
        };

        protected static TypeIndex<DummyObject> typeIndex;
    }
}