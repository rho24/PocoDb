using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Meta
{
    public class when_building_a_meta : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            id = fake.an<IPocoId>();

            A.CallTo(() => pocoIdBuilder.New()).Returns(id);
        };

        Because of = () => metas = sut.Build(poco, idsMetasAndProxies);

        It should_return_a_meta = () => metas.Count().ShouldEqual(1);
        It should_set_the_meta_id = () => metas.First().Id.ShouldEqual(id);
        It should_set_the_meta_type = () => metas.First().Type.ShouldEqual(typeof (DummyObject));
        It should_track_the_new_id = () => idsMetasAndProxies.Ids[poco].ShouldEqual(id);

        static DummyObject poco;
        static IPocoId id;
        static IEnumerable<IPocoMeta> metas;
    }

    public class when_building_a_meta_with_properties : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            id = fake.an<IPocoId>();

            A.CallTo(() => pocoIdBuilder.New()).Returns(id);
        };

        Because of = () => meta = sut.Build(poco, idsMetasAndProxies).First();

        It should_set_all_properties = () => meta.Properties.Count().ShouldEqual(3);

        It should_set_value_properties =
            () =>
            meta.Properties.ContainsKey(new Property<DummyObject, string>(d => d.FirstName)).ShouldBeTrue();

        It should_set_poco_properties =
            () =>
            meta.Properties.ContainsKey(new Property<DummyObject, ChildObject>(d => d.Child)).ShouldBeTrue();

        static DummyObject poco;
        static IPocoId id;
        static IPocoMeta meta;
    }

    public class when_building_a_meta_with_a_set_value_property : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            poco.FirstName = "value";

            id = fake.an<IPocoId>();

            property = new Property<DummyObject, string>(d => d.FirstName);

            A.CallTo(() => pocoIdBuilder.New()).Returns(id);
        };

        Because of = () => meta = sut.Build(poco, idsMetasAndProxies).First();

        It should_set_the_value_property = () => meta.Properties[property].ShouldEqual("value");

        static DummyObject poco;
        static IPocoId id;
        static IPocoMeta meta;
        static IProperty property;
    }

    public class when_building_a_meta_with_a_set_existing_poco_property : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            child = new ChildObject();
            poco.Child = child;

            childId = fake.an<IPocoId>();

            property = new Property<DummyObject, ChildObject>(d => d.Child);

            idsMetasAndProxies.Ids.Add(child, childId);
        };

        Because of = () => metas = sut.Build(poco, idsMetasAndProxies);

        It should_poduce_one_meta = () => metas.Count().ShouldEqual(1);
        It should_set_the_poco_property_to_the_child_id = () => metas.First().Properties[property].ShouldEqual(childId);

        static DummyObject poco;
        static ChildObject child;
        static IPocoId childId;
        static IProperty property;
        static IEnumerable<IPocoMeta> metas;
    }

    public class when_building_a_meta_with_a_set_new_poco_property : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            child = new ChildObject();
            poco.Child = child;

            id = fake.an<IPocoId>();
            childId = fake.an<IPocoId>();

            property = new Property<DummyObject, ChildObject>(d => d.Child);

            A.CallTo(() => pocoIdBuilder.New()).ReturnsNextFromSequence(new[] {id, childId});
        };

        Because of = () => metas = sut.Build(poco, idsMetasAndProxies);

        It should_poduce_two_metas = () => metas.Count().ShouldEqual(2);
        It should_set_the_poco_property_to_the_child_id = () => metas.First().Properties[property].ShouldEqual(childId);
        It should_set_the_meta_id = () => metas.First().Id.ShouldEqual(id);
        It should_set_the_meta_type = () => metas.First().Type.ShouldEqual(typeof (DummyObject));
        It should_track_the_new_id = () => idsMetasAndProxies.Ids[poco].ShouldEqual(id);
        It should_set_the_child_meta_id = () => metas.Skip(1).First().Id.ShouldEqual(childId);
        It should_set_the_child_meta_type = () => metas.Skip(1).First().Type.ShouldEqual(typeof (ChildObject));
        It should_track_the_new_child_id = () => idsMetasAndProxies.Ids[child].ShouldEqual(childId);

        static DummyObject poco;
        static ChildObject child;
        static IPocoId id;
        static IPocoId childId;
        static IProperty property;
        static IEnumerable<IPocoMeta> metas;
    }

    public class when_building_a_value_collection_meta : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            collection = new List<string>();
            id = fake.an<IPocoId>();

            A.CallTo(() => pocoIdBuilder.New()).Returns(id);
        };

        Because of = () => metas = sut.Build(collection, idsMetasAndProxies);

        It should_return_a_meta = () => metas.Count().ShouldEqual(1);
        It should_set_the_meta_id = () => metas.First().Id.ShouldEqual(id);
        It should_set_the_meta_type = () => metas.First().Type.ShouldEqual(typeof (ICollection<string>));
        It should_track_the_new_id = () => idsMetasAndProxies.Ids[collection].ShouldEqual(id);

        static ICollection<string> collection;
        static IPocoId id;
        static IEnumerable<IPocoMeta> metas;
    }

    public class when_building_a_populated_value_collection_meta : with_a_new_PocoMetaBuilder
    {
        Establish c = () => { collection = new List<string>() {"value1", "value2"}; };

        Because of = () => meta = sut.Build(collection, idsMetasAndProxies).First();

        It should_process_all_values = () => meta.Collection.Count.ShouldEqual(2);
        It should_set_the_first_value = () => meta.Collection.ShouldContain("value1");
        It should_set_the_second_value = () => meta.Collection.ShouldContain("value2");

        static ICollection<string> collection;
        static IPocoMeta meta;
    }

    public class when_building_a_populated_poco_collection_meta : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            pocoId = fake.an<IPocoId>();

            collection = new List<DummyObject>() {poco};
            idsMetasAndProxies.Ids.Add(poco, pocoId);
        };

        Because of = () => meta = sut.Build(collection, idsMetasAndProxies).First();

        It should_set_the_value_to_the_pocoId = () => meta.Collection.ShouldContainOnly(pocoId);

        static ICollection<DummyObject> collection;
        static DummyObject poco;
        static IPocoId pocoId;
        static IPocoMeta meta;
    }

    public class when_building_a_populated_new_poco_collection_meta : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            pocoId = fake.an<IPocoId>();

            collection = new List<DummyObject>() {poco};

            A.CallTo(() => pocoIdBuilder.New()).Returns(pocoId);
        };

        Because of = () => metas = sut.Build(collection, idsMetasAndProxies);

        It should_create_two_metas = () => metas.Count().ShouldEqual(2);
        It should_set_the_value_to_the_pocoId = () => metas.First().Collection.ShouldContainOnly(pocoId);
        It should_set_the_child_meta_id = () => metas.Skip(1).First().Id.ShouldEqual(pocoId);
        It should_set_the_child_meta_type = () => metas.Skip(1).First().Type.ShouldEqual(typeof (DummyObject));
        It should_track_the_new_child_id = () => idsMetasAndProxies.Ids[poco].ShouldEqual(pocoId);

        static ICollection<DummyObject> collection;
        static DummyObject poco;
        static IPocoId pocoId;
        static IEnumerable<IPocoMeta> metas;
    }
}