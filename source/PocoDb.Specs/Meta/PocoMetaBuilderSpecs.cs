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
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>());
        };

        Because of = () => metas = sut.Build(poco);

        It should_return_a_meta = () => metas.Count().ShouldEqual(1);
        It should_set_the_meta_id = () => metas.First().Id.ShouldEqual(id);
        It should_set_the_meta_type = () => metas.First().Type.ShouldEqual(typeof (DummyObject));
        It should_track_the_new_id = () => session.TrackedIds[poco].ShouldEqual(id);

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
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>());
        };

        Because of = () => meta = sut.Build(poco).First();

        It should_set_all_properties = () => meta.Properties.Count().ShouldEqual(3);

        It should_set_value_properties =
            () =>
            meta.Properties.ContainsKey(new Property<DummyObject, string>(d => d.FirstName)).ShouldBeTrue();

        It should_set_poco_properties =
            () =>
            meta.Properties.ContainsKey(new Property<DummyObject, DummyObject>(d => d.Child)).ShouldBeTrue();

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
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>());
        };

        Because of = () => meta = sut.Build(poco).First();

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
            child = new DummyObject();
            poco.Child = child;

            childId = fake.an<IPocoId>();

            property = new Property<DummyObject, DummyObject>(d => d.Child);

            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>() {{child, childId}});
        };

        Because of = () => metas = sut.Build(poco);

        It should_poduce_one_meta = () => metas.Count().ShouldEqual(1);
        It should_set_the_poco_property_to_the_child_id = () => metas.First().Properties[property].ShouldEqual(childId);

        static DummyObject poco;
        static DummyObject child;
        static IPocoId childId;
        static IProperty property;
        static IEnumerable<IPocoMeta> metas;
    }

    public class when_building_a_meta_with_a_set_new_poco_property : with_a_new_PocoMetaBuilder
    {
        Establish c = () => {
            poco = new DummyObject();
            child = new DummyObject();
            poco.Child = child;

            id = fake.an<IPocoId>();
            childId = fake.an<IPocoId>();

            property = new Property<DummyObject, DummyObject>(d => d.Child);

            A.CallTo(() => pocoIdBuilder.New()).ReturnsNextFromSequence(new[] {id, childId});
            A.CallTo(() => session.TrackedIds).Returns(new Dictionary<object, IPocoId>());
        };

        Because of = () => metas = sut.Build(poco);

        It should_poduce_two_metas = () => metas.Count().ShouldEqual(2);
        It should_set_the_poco_property_to_the_child_id = () => metas.First().Properties[property].ShouldEqual(childId);
        It should_set_the_meta_id = () => metas.First().Id.ShouldEqual(id);
        It should_set_the_meta_type = () => metas.First().Type.ShouldEqual(typeof (DummyObject));
        It should_track_the_new_id = () => session.TrackedIds[poco].ShouldEqual(id);
        It should_set_the_child_meta_id = () => metas.Skip(1).First().Id.ShouldEqual(childId);
        It should_set_the_child_meta_type = () => metas.Skip(1).First().Type.ShouldEqual(typeof (DummyObject));
        It should_track_the_new_child_id = () => session.TrackedIds[child].ShouldEqual(childId);

        static DummyObject poco;
        static DummyObject child;
        static IPocoId id;
        static IPocoId childId;
        static IProperty property;
        static IEnumerable<IPocoMeta> metas;
    }
}