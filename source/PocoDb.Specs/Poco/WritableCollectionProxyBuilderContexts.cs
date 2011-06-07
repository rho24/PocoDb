using System;
using System.Collections.Generic;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.ChangeTracking;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Session;

namespace PocoDb.Specs.Poco
{
    [Subject(typeof (WritableCollectionProxyBuilder))]
    public class with_a_new_WritableCollectionProxyBuilder : Observes<WritableCollectionProxyBuilder>
    {
        Establish c = () => {
            session = fake.an<IInternalWriteablePocoSession>();
            trackedChanges = fake.an<ITrackedChanges>();

            A.CallTo(() => session.Changes).Returns(trackedChanges);

            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IInternalWriteablePocoSession session;
        protected static ITrackedChanges trackedChanges;
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