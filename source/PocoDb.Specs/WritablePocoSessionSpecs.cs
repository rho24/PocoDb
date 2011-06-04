using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;

namespace PocoDb.Specs
{
    public class when_saved_changes_is_called : with_a_new_WritablePocoSession
    {
        Establish c = () => {
            commitBuilder = depends.on<ICommitBuilder>();
            commit = new Commit();

            A.CallTo(() => commitBuilder.Build(A<TrackedChanges>._)).Returns(commit);
        };

        Because of = () => sut.SaveChanges();

        It should_create_commit = () => A.CallTo(() => commitBuilder.Build(A<TrackedChanges>._)).MustHaveHappened();
        It should_send_commit_to_server = () => A.CallTo(() => server.Commit(commit)).MustHaveHappened();

        static ICommitBuilder commitBuilder;
        static Commit commit;
    }
}