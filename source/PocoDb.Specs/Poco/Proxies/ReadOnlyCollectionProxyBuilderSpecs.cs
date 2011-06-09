using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Poco.Proxies
{
    public class when_a_collection_proxy_is_built : with_a_new_ReadOnlyCollectionProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (ICollection<string>));
            A.CallTo(() => meta.Collection).Returns(new List<object>());
        };

        Because of = () => proxy = sut.BuildProxy(meta);

        It should_not_be_null = () => proxy.ShouldNotBeNull();
        It should_be_the_correct_type = () => proxy.ShouldBeOfType<ICollection<string>>();

        static IPocoMeta meta;
        static object proxy;
    }

    public class when_a_collection_proxy_with_a_value_is_built : with_a_new_ReadOnlyCollectionProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (ICollection<string>));
            A.CallTo(() => meta.Collection).Returns(new List<object> {"value"});
        };

        Because of = () => proxy = sut.BuildProxy(meta) as ICollection<string>;

        It should_contain_the_value = () => proxy.Contains("value").ShouldBeTrue();

        static IPocoMeta meta;
        static ICollection<string> proxy;
    }

    public class when_a_collection_proxy_with_a_value_has_the_same_value_added :
        with_a_ReadOnlyCollectionProxy_with_value
    {
        Establish c = () => { };

        Because of = () => proxy.Add("value");

        It should_contain_the_value_twice =
            () => ShouldExtensionMethods.ShouldEqual(proxy.Where(v => v == "value").Count(), 2);
    }
}