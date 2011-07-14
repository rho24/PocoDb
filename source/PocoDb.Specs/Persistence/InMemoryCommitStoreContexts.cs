using System;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Meta;
using PocoDb.Persistence;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Persistence
{
    [Subject(typeof (InMemoryCommitStore))]
    public class with_a_new_InMemoryCommitStore : Observes<InMemoryCommitStore>
    {
        Establish c = () => { };
    }

    [Subject(typeof (InMemoryCommitStore))]
    public class with_a_populated_InMemoryCommitStore : with_a_new_InMemoryCommitStore
    {
        Establish c = () => {
            commit = new Commit(new CommitId(Guid.Empty, DateTime.MinValue));
            commit.AddedPocos.Add(new AddedPoco(new PocoMeta(new PocoId(Guid.Empty), typeof (DummyObject))));
            commit.UpdatedPocos.Add(Tuple.Create((IPocoId) new PocoId(Guid.Empty),
                                                 new SetProperty(new Property<DummyObject, string>(d => d.FirstName),
                                                                 "value")));
            commit.CollectionAdditions.Add(new CollectionAddition(new PocoId(Guid.Empty), "value"));
            commit.CollectionRemovals.Add(new CollectionRemoval(new PocoId(Guid.Empty), "value"));

            sut_setup.run(sut => sut.Save(commit));
        };

        protected static Commit commit;
    }
}