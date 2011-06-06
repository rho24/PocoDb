using System;
using System.Collections.Generic;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Session;

namespace PocoDb.Specs.Poco
{
    [Subject(typeof (ReadOnlyCollectionProxyBuilder))]
    public class with_a_new_ReadOnlyCollectionProxyBuilder : Observes<ReadOnlyCollectionProxyBuilder>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();
            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IInternalPocoSession session;
    }

    [Subject(typeof (ReadOnlyCollectionProxyBuilder))]
    public class with_a_ReadOnlyCollectionProxy_with_value : with_a_new_ReadOnlyCollectionProxyBuilder
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