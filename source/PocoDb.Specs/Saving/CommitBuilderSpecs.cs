using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Meta;

namespace PocoDb.Specs.Saving
{
    public class when_a_commit_is_build : with_a_new_CommitBuilder
    {
        Establish c = () => {
            commitIdGenerator = depends.on<ICommitIdGenerator>();
            id = fake.an<ICommitId>();

            A.CallTo(() => commitIdGenerator.New()).Returns(id);
        };

        It should_contain_have_an_id_generated = () => commit.Id.ShouldEqual(id);

        static ICommitIdGenerator commitIdGenerator;
        static ICommitId id;
    }

    public class when_a_new_object_is_being_tracked : with_a_new_CommitBuilder
    {
        Establish c = () => {
            poco = new DummyObject();

            A.CallTo(() => changes.AddedPocos).Returns(new[] {new TrackedAddedPoco(poco)}.ToArray());

            A.CallTo(() => pocoMetaBuilder.Build(poco)).Returns(new[] {fake.an<IPocoMeta>()});
        };

        It should_contain_an_added_PocoMeta = () => commit.AddedPocos.Count().ShouldEqual(1);
        It should_create_a_PocoMeta = () => A.CallTo(() => pocoMetaBuilder.Build(poco)).MustHaveHappened();

        static DummyObject poco;
    }

    public class when_a_property_set_is_being_tracked : with_a_new_CommitBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            property = new Property<DummyObject, string>(o => o.FirstName);

            A.CallTo(() => changes.SetProperties).Returns(
                new[] {new TrackedSetProperty(poco, property, "value")}.ToArray());

            pocoId = A.Fake<IPocoId>();
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>() {{poco, pocoId}});
        };

        It should_contain_a_set_property = () => commit.SetProperties.Count().ShouldEqual(1);
        It should_reference_the_PocoId = () => commit.SetProperties.First().PocoId.ShouldEqual(pocoId);
        It should_reference_the_Property = () => commit.SetProperties.First().Property.ShouldEqual(property);
        It should_reference_the_value = () => commit.SetProperties.First().Value.ShouldEqual("value");

        static DummyObject poco;
        static IPocoId pocoId;
        static IProperty property;
    }

    public class when_an_add_to_collection_is_being_tracked : with_a_new_CommitBuilder
    {
        Establish c = () => {
            collection = new List<string>();

            A.CallTo(() => changes.CollectionAdditions).Returns(
                new[] {new TrackedCollectionAddition(collection, "value")}.ToArray());

            collectionId = A.Fake<IPocoId>();
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>() {{collection, collectionId}});
        };

        It should_contain_an_AddToCollection = () => commit.CollectionAdditions.Count().ShouldEqual(1);

        It should_reference_the_collection =
            () => commit.CollectionAdditions.First().CollectionId.ShouldEqual(collectionId);

        It should_reference_the_value = () => commit.CollectionAdditions.First().Value.ShouldEqual("value");

        static ICollection collection;
        static IPocoId collectionId;
    }

    public class when_an_remove_from_collection_is_being_tracked : with_a_new_CommitBuilder
    {
        Establish c = () => {
            collection = new List<string>();

            A.CallTo(() => changes.CollectionRemovals).Returns(
                new[] {new TrackedCollectionRemoval(collection, "value")}.ToArray());

            collectionId = A.Fake<IPocoId>();
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>() {{collection, collectionId}});
        };

        It should_contain_a_RemovedFromCollection = () => commit.CollectionRemovals.Count().ShouldEqual(1);

        It should_reference_the_collection =
            () => commit.CollectionRemovals.First().CollectionId.ShouldEqual(collectionId);

        It should_reference_the_value = () => commit.CollectionRemovals.First().Value.ShouldEqual("value");

        static ICollection collection;
        static IPocoId collectionId;
    }
}