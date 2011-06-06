using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs
{
    public class when_a_proxy_with_a_value_property_is_built : with_a_new_ReadOnlyPocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>()
                                                    {{new Property<DummyObject, string>(p => p.FirstName), "value"}});
        };

        Because of = () => poco = sut.BuildProxy(meta) as DummyObject;

        It should_have_its_property_set = () => poco.FirstName.ShouldEqual("value");

        static IPocoMeta meta;
        static DummyObject poco;
    }

    public class when_a_proxy_with_a_poco_property_is_built : with_a_new_ReadOnlyPocoProxyBuilder
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

        Because of = () => poco = sut.BuildProxy(meta) as DummyObject;

        It should_have_its_property_set = () => poco.Child.ShouldEqual(childPoco);

        static IPocoMeta meta;
        static DummyObject poco;
        static IPocoId childId;
        static DummyObject childPoco;
    }
}