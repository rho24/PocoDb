using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Indexing;

namespace PocoDb.Specs.Indexing
{
    [Subject(typeof (IndexManager))]
    public class with_a_new_IndexManager : Observes<IndexManager>
    {
        Establish c = () => { sut_setup.run(sut => sut.Indexes.Add(new TypeIndex<DummyObject>())); };
    }
}