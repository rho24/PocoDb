using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs.Server
{
    [Subject(typeof (ICommitProcessor))]
    public class with_a_new_CommitProcessor : Observes<CommitProcessor>
    {
        Establish c = () => {
            metaStore = depends.on<IMetaStore>();
            commit = fake.an<ICommit>();
        };

        protected static IMetaStore metaStore;
        protected static ICommit commit;
    }
}