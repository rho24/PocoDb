using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Meta;
using PocoDb.Pocos;

namespace PocoDb.Specs.Saving
{
    [Subject(typeof (ICommitBuilder))]
    public class with_a_new_CommitBuilder : Observes<CommitBuilder>
    {
        Establish c = () => {
            pocoMetaBuilder = depends.on<IPocoMetaBuilder>();
            idsMetasAndProxies = new IdsMetasAndProxies();
            changes = fake.an<ITrackedChanges>();
        };

        Because of = () => commit = sut.Build(changes, idsMetasAndProxies);

        protected static IPocoMetaBuilder pocoMetaBuilder;
        protected static IIdsMetasAndProxies idsMetasAndProxies;
        protected static ITrackedChanges changes;
        protected static ICommit commit;
    }
}