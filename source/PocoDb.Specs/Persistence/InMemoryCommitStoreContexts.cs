using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Persistence;

namespace PocoDb.Specs.Persistence
{
    [Subject(typeof (InMemoryCommitStore))]
    public class with_a_new_InMemoryCommitStore : Observes<InMemoryCommitStore>
    {
        Establish c = () => { };
    }
}