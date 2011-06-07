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
}