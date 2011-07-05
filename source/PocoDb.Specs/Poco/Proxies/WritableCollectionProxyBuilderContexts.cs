using System;
using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Pocos.Proxies;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Poco.Proxies
{
    [Subject(typeof (WritableCollectionProxyBuilder))]
    public class with_a_new_WritableCollectionProxyBuilder : Observes<WritableCollectionProxyBuilder>
    {
        Establish c = () => {
            pocoGetter = fake.an<ICanGetPocos>();
            changeTracker = fake.an<IChangeTracker>();

            sut_setup.run(sut => sut.Initialise(pocoGetter, changeTracker));
        };

        protected static ICanGetPocos pocoGetter;
        protected static IChangeTracker changeTracker;
    }

    [Subject(typeof (WritableCollectionProxyBuilder))]
    public class with_a_WritableCollectionProxy : with_a_new_WritableCollectionProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (ICollection<string>));
            A.CallTo(() => meta.Collection).Returns(new List<object> {});
            sut_setup.run(sut => proxy = (ICollection<string>) sut.BuildProxy(meta));
        };

        protected static IPocoMeta meta;
        protected static ICollection<string> proxy;
    }

    [Subject(typeof (WritableCollectionProxyBuilder))]
    public class with_a_WritableCollectionProxy_with_value : with_a_new_WritableCollectionProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (ICollection<string>));
            A.CallTo(() => meta.Collection).Returns(new List<object> {"value"});
            sut_setup.run(sut => proxy = (ICollection<string>) sut.BuildProxy(meta));
        };

        protected static IPocoMeta meta;
        protected static ICollection<string> proxy;
    }
}