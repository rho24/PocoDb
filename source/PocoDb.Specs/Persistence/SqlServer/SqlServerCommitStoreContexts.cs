using System;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Meta;
using PocoDb.Persistence.SqlServer;
using PocoDb.Serialisation;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Persistence.SqlServer
{
    [Subject(typeof (SqlServerCommitStore))]
    [SetupForEachSpecification]
    public class with_a_new_SqlServerCommitStore : Observes<SqlServerCommitStore>
    {
        Establish c = () => {
            var connectionFactory = CompactDbHelper.CreateFreshDb();

            depends.on(connectionFactory);
            depends.on<ISerializer>(new JsonSerializer());
        };
    }

    [Subject(typeof (SqlServerCommitStore))]
    public class with_a_populated_SqlServerCommitStore : with_a_new_SqlServerCommitStore
    {
        Establish c = () => {
            commit = new Commit(new CommitId(Guid.Empty, DateTime.MinValue));
            commit.AddedPocos.Add(new AddedPoco(new PocoMeta(new PocoId(Guid.Empty), typeof (DummyObject))));
            commit.SetProperties.Add(new SetProperty(new PocoId(Guid.Empty),
                                                     new Property<DummyObject, string>(d => d.FirstName), "value"));
            commit.CollectionAdditions.Add(new CollectionAddition(new PocoId(Guid.Empty), "value"));
            commit.CollectionRemovals.Add(new CollectionRemoval(new PocoId(Guid.Empty), "value"));

            sut_setup.run(sut => sut.Save(commit));
        };

        protected static Commit commit;
    }
}