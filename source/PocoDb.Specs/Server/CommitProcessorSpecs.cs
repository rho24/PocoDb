using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs.Server
{
    public class when_applying_a_commit_with_an_AddObject : with_a_new_CommitProcessor
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();

            A.CallTo(() => commit.AddedPocos).Returns(new[] {new AddedPoco(meta)});
        };

        Because of = () => sut.Apply(commit);

        It should_add_the_meta_to_the_store = () => A.CallTo(() => metaStore.AddNew(meta)).MustHaveHappened();
        It should_notify_the_IndexManager = () => A.CallTo(() => indexManager.NotifyMetaChange(meta)).MustHaveHappened();

        static IPocoMeta meta;
    }

    public class when_applying_a_commit_with_an_UpdatedPoco : with_a_new_CommitProcessor
    {
        Establish c = () => {
            id = fake.an<IPocoId>();
            meta = fake.an<IPocoMeta>();
            property = fake.an<IProperty>();
            metaProperties = new Dictionary<IProperty, object>();
            A.CallTo(() => meta.Properties).Returns(metaProperties);

            A.CallTo(() => metaStore.GetWritable(id)).Returns(meta);
            var updatedPocos = new List<Tuple<IPocoId, SetProperty>>() {
                                                                           Tuple.Create(id,
                                                                                        new SetProperty(property,
                                                                                                        "value"))
                                                                       };
            A.CallTo(() => commit.UpdatedPocos).Returns(updatedPocos.ToLookup(u => u.Item1, u => u.Item2));
        };

        Because of = () => sut.Apply(commit);

        It should_update_the_metas_property = () => meta.Properties[property].ShouldEqual("value");
        It should_save_the_meta_to_the_store = () => A.CallTo(() => metaStore.Update(meta)).MustHaveHappened();

        static IPocoId id;
        static IPocoMeta meta;
        static IProperty property;
        static IDictionary<IProperty, object> metaProperties;
    }

    public class when_applying_a_commit_with_an_AddToCollection : with_a_new_CommitProcessor
    {
        Establish c = () => {
            id = fake.an<IPocoId>();
            meta = fake.an<IPocoMeta>();
            metaCollection = new List<object>();

            A.CallTo(() => metaStore.GetWritable(id)).Returns(meta);
            A.CallTo(() => commit.CollectionAdditions).Returns(new[] {new CollectionAddition(id, "value")});
            A.CallTo(() => meta.Collection).Returns(metaCollection);
        };

        Because of = () => sut.Apply(commit);

        It should_add_to_the_metas_collection = () => meta.Collection.ShouldContain("value");
        It should_save_the_meta_to_the_store = () => A.CallTo(() => metaStore.Update(meta)).MustHaveHappened();

        static IPocoId id;
        static IPocoMeta meta;
        static ICollection<object> metaCollection;
    }

    public class when_applying_a_commit_with_a_RemoveFromCollection : with_a_new_CommitProcessor
    {
        Establish c = () => {
            id = fake.an<IPocoId>();
            meta = fake.an<IPocoMeta>();
            metaCollection = new List<object>() {"value"};

            A.CallTo(() => metaStore.GetWritable(id)).Returns(meta);
            A.CallTo(() => commit.CollectionRemovals).Returns(new[] {new CollectionRemoval(id, "value")});
            A.CallTo(() => meta.Collection).Returns(metaCollection);
        };

        Because of = () => sut.Apply(commit);

        It should_add_to_the_metas_collection = () => meta.Collection.ShouldNotContain("value");
        It should_save_the_meta_to_the_store = () => A.CallTo(() => metaStore.Update(meta)).MustHaveHappened();

        static IPocoId id;
        static IPocoMeta meta;
        static ICollection<object> metaCollection;
    }
}