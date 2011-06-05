using System;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    public class when_applying_a_commit_with_an_AddObject : with_a_new_CommitProcessor
    {
        Establish c = () => {
            metaStore = depends.on<IMetaStore>();

            commit = fake.an<ICommit>();
            meta = fake.an<IPocoMeta>();

            A.CallTo(() => commit.AddObjects).Returns(new[] {new AddObject(meta)});
        };

        Because of = () => sut.Apply(commit);

        It should_add_the_meta_to_the_store = () => A.CallTo(() => metaStore.Add(meta)).MustHaveHappened();

        static IMetaStore metaStore;
        static ICommit commit;
        static IPocoMeta meta;
    }
}