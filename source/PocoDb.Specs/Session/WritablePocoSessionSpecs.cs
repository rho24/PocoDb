using System;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;

namespace PocoDb.Specs.Session
{
    public class when_saved_changes_is_called : with_a_new_WritablePocoSession
    {
        Establish c = () => {
            commitBuilder = depends.on<ICommitBuilder>();
            commit = fake.an<ICommit>();

            A.CallTo(() => commitBuilder.Build(A<ITrackedChanges>.Ignored)).Returns(commit);
        };

        Because of = () => sut.SaveChanges();

        It should_create_commit =
            () => A.CallTo(() => commitBuilder.Build(A<ITrackedChanges>.Ignored)).MustHaveHappened();

        It should_send_commit_to_server = () => A.CallTo(() => pocoDbServer.Commit(commit)).MustHaveHappened();

        static ICommitBuilder commitBuilder;
        static ICommit commit;
    }
}