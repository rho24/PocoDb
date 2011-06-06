using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;

namespace PocoDb.Specs.Poco
{
    public class when_a_poco_is_built : with_a_new_PocoFactory
    {
        Establish c = () => {
            trackedPocos = fake.an<IDictionary<IPocoId, object>>();
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();
            proxy = fake.an<DummyObject>();

            A.CallTo(() => session.TrackedPocos).Returns(trackedPocos);
            A.CallTo(() => proxyBuilder.BuildProxy(meta)).Returns(proxy);

            A.CallTo(() => meta.Id).Returns(id);
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
        };

        Because of = () => poco = sut.Build(meta);

        It should_be_built_by_proxy_builder = () => poco.ShouldEqual(proxy);
        It should_be_tracked = () => A.CallTo(() => trackedPocos.Add(id, proxy)).MustHaveHappened();

        static IDictionary<IPocoId, object> trackedPocos;
        static IPocoMeta meta;
        static IPocoId id;
        static object poco;
        static object proxy;
    }

    public class when_a_poco_is_already_being_tracked : with_a_new_PocoFactory
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

    public class when_a_collection_is_built : with_a_new_PocoFactory
    {
        Establish c = () => {
            trackedPocos = fake.an<IDictionary<IPocoId, object>>();
            meta = fake.an<IPocoMeta>();
            id = fake.an<IPocoId>();
            proxy = fake.an<ICollection<string>>();

            A.CallTo(() => session.TrackedPocos).Returns(trackedPocos);
            A.CallTo(() => collectionProxyBuilder.BuildProxy(meta)).Returns(proxy);

            A.CallTo(() => meta.Id).Returns(id);
            A.CallTo(() => meta.Type).Returns(typeof (ICollection<string>));
        };

        Because of = () => poco = sut.Build(meta);

        It should_be_built_by_collection_proxy_builder = () => poco.ShouldEqual(proxy);
        It should_be_tracked = () => A.CallTo(() => trackedPocos.Add(id, proxy)).MustHaveHappened();

        static IDictionary<IPocoId, object> trackedPocos;
        static IPocoMeta meta;
        static IPocoId id;
        static object poco;
        static object proxy;
    }
}