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

            trackedChanges = new TrackedChanges();
        };

        Because of = () => commit = sut.Build(trackedChanges);

        protected static IPocoMetaBuilder pocoMetaBuilder;
        protected static ITrackedChanges trackedChanges;
        protected static ICommit commit;
    }
}