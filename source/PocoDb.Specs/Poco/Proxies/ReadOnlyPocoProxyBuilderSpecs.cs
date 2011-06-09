using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Poco.Proxies
{
    public class when_a_poco_proxy_is_built : with_a_new_ReadOnlyPocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>());
        };

        Because of = () => proxy = sut.BuildProxy(meta);

        It should_not_be_null = () => proxy.ShouldNotBeNull();
        It should_be_the_correct_type = () => proxy.ShouldBeOfType<DummyObject>();

        static IPocoMeta meta;
        static object proxy;
    }


    public class when_a_poco_proxy_with_a_value_property_is_built : with_a_new_ReadOnlyPocoProxyBuilder
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

    public class when_a_poco_proxy_with_a_poco_property_is_built : with_a_new_ReadOnlyPocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            childId = fake.an<IPocoId>();
            childPoco = fake.an<ChildObject>();

            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>()
            {{new Property<DummyObject, ChildObject>(p => p.Child), childId}});

            A.CallTo(() => session.GetPoco(childId)).Returns(childPoco);
        };

        Because of = () => poco = sut.BuildProxy(meta) as DummyObject;

        It should_have_its_property_set = () => poco.Child.ShouldEqual(childPoco);

        static IPocoMeta meta;
        static DummyObject poco;
        static IPocoId childId;
        static ChildObject childPoco;
    }
}