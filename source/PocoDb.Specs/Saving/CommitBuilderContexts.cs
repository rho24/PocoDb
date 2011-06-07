using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs.Saving
{
    [Subject(typeof (ICommitBuilder))]
    public class with_a_new_CommitBuilder : Observes<CommitBuilder>
    {
        Establish c = () => {
            pocoMetaBuilder = depends.on<IPocoMetaBuilder>();

            changes = fake.an<ITrackedChanges>();
        };

        Because of = () => commit = sut.Build(changes);

        protected static IPocoMetaBuilder pocoMetaBuilder;
        protected static ITrackedChanges changes;
        protected static ICommit commit;
    }
}