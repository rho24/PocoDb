using System;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Persistence;

namespace PocoDb.Specs.Server
{
    public class when_a_commit_is_recieved : with_a_new_PocoDbServer
    {
        Establish c = () => {
            commit = fake.an<ICommit>();
            commitStore = depends.on<ICommitStore>();
            commitProcessor = depends.on<ICommitProcessor>();
        };

        Because of = () => sut.Commit(commit);
        It should_save_the_commit = () => A.CallTo(() => commitStore.Save(commit)).MustHaveHappened();
        It should_apply_the_commit = () => A.CallTo(() => commitProcessor.Apply(commit));

        static ICommit commit;
        static ICommitStore commitStore;
        static ICommitProcessor commitProcessor;
    }
}