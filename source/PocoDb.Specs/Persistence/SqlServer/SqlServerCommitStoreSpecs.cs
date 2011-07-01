using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using PocoDb.Commits;

namespace PocoDb.Specs.Persistence.SqlServer
{
    public class when_a_commit_is_added : with_a_new_SqlServerCommitStore
    {
        Establish c = () => { commit = new Commit(new CommitId(Guid.Empty, DateTime.MinValue)); };

        Because of = () => sut.Save(commit);

        It should_be_retrievable = () => sut.Get(commit.Id).ShouldNotBeNull();

        static ICommit commit;
    }

    public class when_retrieving_a_non_existant_commit : with_a_new_SqlServerCommitStore
    {
        Because of = () => commit = sut.Get(new CommitId(Guid.Empty, DateTime.MinValue));

        It should_return_null = () => commit.ShouldBeNull();

        static ICommit commit;
    }

    public class when_retrieving_a_populated_commit : with_a_populated_SqlServerCommitStore
    {
        Because of = () => retrievedCommit = sut.Get((commit as ICommit).Id);

        It should_not_be_null = () => retrievedCommit.ShouldNotBeNull();
        It should_have_an_AddedPoco = () => retrievedCommit.AddedPocos.Count().ShouldEqual(1);
        It should_have_a_SetProperty = () => retrievedCommit.SetProperties.Count().ShouldEqual(1);
        It should_have_a_CollectionAddition = () => retrievedCommit.CollectionAdditions.Count().ShouldEqual(1);
        It should_have_a_CollectionRemoval = () => retrievedCommit.CollectionRemovals.Count().ShouldEqual(1);

        static ICommit retrievedCommit;
    }

    public class when_retrieving_multiple_commits : with_a_new_SqlServerCommitStore
    {
        Establish c = () => {
            var time = DateTime.Now;
            commit1 = new Commit(new CommitId(Guid.NewGuid(), time));

            commit2 = new Commit(new CommitId(Guid.NewGuid(), time.AddSeconds(5)));

            sut_setup.run(sut => {
                sut.Save(commit1);
                sut.Save(commit2);
            });
        };

        Because of = () => commits = sut.GetAll().ToList();

        It should_return_all_commits = () => commits.Count.ShouldEqual(2);
        It should_return_the_first = () => commits[0].Id.ShouldEqual(commit1.Id);
        It should_return_the_second = () => commits[1].Id.ShouldEqual(commit2.Id);

        static ICommit commit1;
        static ICommit commit2;
        static List<ICommit> commits;
    }
}