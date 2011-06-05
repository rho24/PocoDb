using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Commits;

namespace PocoDb.Specs
{
    [Subject(typeof (ICommitProcessor))]
    public class with_a_new_CommitProcessor : Observes<CommitProcessor>
    {
        Establish c = () => { };
    }
}