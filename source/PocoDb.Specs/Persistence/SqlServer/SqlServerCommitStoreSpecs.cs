using System;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Persistence.SqlServer;

namespace PocoDb.Specs.Persistence.SqlServer
{
    public class when_a_commit_is_added : with_a_new_SqlServerCommitStore
    {
        Establish c = () => { commit = new SqlCommit() {Id = Guid.NewGuid()}; };

        Because of = () => sut.Save(commit);

        It should_be_retrievable = () => sut.Get(commit.Id).Id.ShouldEqual(commit.Id);

        static ICommit commit;
    }

    public class when_retrieving_a_non_existant_commit : with_a_new_SqlServerCommitStore
    {
        Because of = () => commit = sut.Get(new CommitId(Guid.Empty));

        It should_return_null = () => commit.ShouldBeNull();

        static ICommit commit;
    }
}