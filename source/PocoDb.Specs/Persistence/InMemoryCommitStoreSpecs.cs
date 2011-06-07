using System;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Commits;

namespace PocoDb.Specs.Persistence
{
    public class when_a_commit_is_added : with_a_new_InMemoryCommitStore
    {
        Establish c = () => {
            commit = fake.an<ICommit>();
            id = fake.an<ICommitId>();

            A.CallTo(() => commit.Id).Returns(id);
        };

        Because of = () => sut.Save(commit);

        It should_be_retrievable = () => sut.Get(id).ShouldEqual(commit);

        static ICommit commit;
        static ICommitId id;
    }

    public class when_retrieving_a_non_existant_commit : with_a_new_InMemoryCommitStore
    {
        Because of = () => commit = sut.Get(fake.an<ICommitId>());

        It should_return_null = () => commit.ShouldBeNull();

        static ICommit commit;
    }
}