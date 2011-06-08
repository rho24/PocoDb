using System;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Pocos;

namespace PocoDb.Specs.Session
{
    public class when_a_poco_is_added : with_a_new_WritablePocoSession
    {
        Establish c = () => { poco = new DummyObject(); };

        Because of = () => sut.Add(poco);

        It should_track_the_added_poco =
            () => sut.ChangeTracker.Changes.AddedPocos.Where(a => a.Poco == poco).Count().ShouldEqual(1);

        static DummyObject poco;
    }

    public class when_saved_changes_is_called : with_a_new_WritablePocoSession
    {
        Establish c = () => {
            commitBuilder = depends.on<ICommitBuilder>();
            commit = fake.an<ICommit>();

            A.CallTo(() => commitBuilder.Build(A<ITrackedChanges>.Ignored, A<IIdsMetasAndProxies>.Ignored)).Returns(
                commit);
        };

        Because of = () => sut.SaveChanges();

        It should_create_commit =
            () =>
            A.CallTo(() => commitBuilder.Build(A<ITrackedChanges>.Ignored, A<IIdsMetasAndProxies>.Ignored)).
                MustHaveHappened();

        It should_send_commit_to_server = () => A.CallTo(() => server.Commit(commit)).MustHaveHappened();

        static ICommitBuilder commitBuilder;
        static ICommit commit;
    }
}