﻿using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Poco.Proxies
{
    public class when_a_writable_poco_proxy_is_built : with_a_new_WriteablePocoProxyBuilder
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

    public class when_a_writable_poco_proxy_with_a_value_property_is_built : with_a_new_WriteablePocoProxyBuilder
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

    public class when_a_writable_poco_proxy_with_a_poco_property_is_built : with_a_new_WriteablePocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            childId = fake.an<IPocoId>();
            childPoco = fake.an<ChildObject>();

            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(new Dictionary<IProperty, object>()
            {{new Property<DummyObject, ChildObject>(p => p.Child), childId}});

            A.CallTo(() => pocoGetter.GetPoco(childId)).Returns(childPoco);
        };

        Because of = () => poco = sut.BuildProxy(meta) as DummyObject;

        It should_have_its_property_set = () => poco.Child.ShouldEqual(childPoco);

        static IPocoMeta meta;
        static DummyObject poco;
        static IPocoId childId;
        static ChildObject childPoco;
    }

    public class when_setting_a_property_on_a_writable_poco_proxy : with_a_writable_poco_proxy
    {
        Establish c = () => { property = new Property<DummyObject, string>(d => d.FirstName); };

        Because of = () => proxy.FirstName = "value";

        It should_track_the_change =
            () => A.CallTo(() => changeTracker.TrackPropertySet(proxy, property, "value")).MustHaveHappened();

        It should_update_the_value = () => proxy.FirstName.ShouldEqual("value");

        static IProperty property;
    }

    public class when_setting_a_property_on_a_writable_poco_proxy_to_a_new_value :
        with_a_writable_poco_proxy_with_values
    {
        Establish c = () => { property = new Property<DummyObject, string>(d => d.FirstName); };

        Because of = () => proxy.FirstName = "new value";

        It should_not_track_the_change =
            () => A.CallTo(() => changeTracker.TrackPropertySet(proxy, property, "new value")).MustHaveHappened();

        It should_update_the_value = () => proxy.FirstName.ShouldEqual("new value");

        static IProperty property;
    }

    public class when_setting_a_property_on_a_writable_poco_proxy_to_the_same_value :
        with_a_writable_poco_proxy_with_values
    {
        Establish c = () => { property = new Property<DummyObject, string>(d => d.FirstName); };

        Because of = () => proxy.FirstName = "value";

        It should_not_track_the_change =
            () => A.CallTo(() => changeTracker.TrackPropertySet(proxy, property, "value")).MustNotHaveHappened();

        It should_should_have_the_same_value = () => proxy.FirstName.ShouldEqual("value");

        static IProperty property;
    }
}