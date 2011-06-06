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
    [Subject(typeof (WriteablePocoProxyBuilder))]
    public class with_a_new_WriteablePocoProxyBuilder : Observes<WriteablePocoProxyBuilder>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();
            trackedChanges = fake.an<ITrackedChanges>();

            A.CallTo(() => session.Changes).Returns(trackedChanges);

            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IInternalPocoSession session;
        protected static ITrackedChanges trackedChanges;
    }

    [Subject(typeof (WriteablePocoProxyBuilder))]
    public class with_a_writable_poco_proxy : with_a_new_WriteablePocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(
                new Dictionary<IProperty, object>() {
                                                        {new Property<DummyObject, string>(d => d.FirstName), null}
                                                    });

            sut_setup.run(sut => proxy = (DummyObject) sut.BuildProxy(meta));
        };

        protected static IPocoMeta meta;
        protected static DummyObject proxy;
    }
}