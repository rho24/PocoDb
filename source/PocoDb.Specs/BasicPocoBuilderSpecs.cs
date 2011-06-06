using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    public class when_a_poco_is_built : with_a_new_BasicPocoBuilder
    {
        Establish c = () => {
            trackedPocos = fake.an<IDictionary<IPocoId, object>>();
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();

            A.CallTo(() => session.TrackedPocos).Returns(trackedPocos);

            A.CallTo(() => meta.Id).Returns(id);
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>());
        };

        Because of = () => poco = sut.Build(meta);

        It should_not_be_null = () => poco.ShouldNotBeNull();
        It should_be_the_correct_type = () => (poco is DummyObject).ShouldBeTrue();
        It should_be_tracked = () => A.CallTo(() => trackedPocos.Add(id, poco)).MustHaveHappened();

        static IDictionary<IPocoId, object> trackedPocos;
        static IPocoMeta meta;
        static IPocoId id;
        static object poco;
    }

    public class when_a_poco_with_a_value_property_is_built : with_a_new_BasicPocoBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>()
                                                    {{new Property<DummyObject, string>(p => p.FirstName), "value"}});
        };

        Because of = () => poco = sut.Build(meta) as DummyObject;

        It should_have_its_property_set = () => poco.FirstName.ShouldEqual("value");

        static IPocoMeta meta;
        static DummyObject poco;
    }

    public class when_a_poco_with_a_poco_property_is_built : with_a_new_BasicPocoBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            childId = fake.an<IPocoId>();
            childPoco = fake.an<DummyObject>();

            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>()
                                                    {{new Property<DummyObject, DummyObject>(p => p.Child), childId}});

            A.CallTo(() => session.GetPoco(childId)).Returns(childPoco);
        };

        Because of = () => poco = sut.Build(meta) as DummyObject;

        It should_have_its_property_set = () => poco.Child.ShouldEqual(childPoco);

        static IPocoMeta meta;
        static DummyObject poco;
        static IPocoId childId;
        static DummyObject childPoco;
    }

    public class when_a_poco_is_already_being_tracked : with_a_new_BasicPocoBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();
            trackedPoco = fake.an<DummyObject>();

            A.CallTo(() => meta.Id).Returns(id);
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>());
            A.CallTo(() => session.TrackedPocos).Returns(new Dictionary<IPocoId, object>() {{id, trackedPoco}});
        };

        Because of = () => poco = sut.Build(meta);

        It should_return_the_original_poco = () => poco.ShouldEqual(trackedPoco);

        static IPocoMeta meta;
        static IPocoId id;
        static DummyObject trackedPoco;
        static object poco;
    }
}