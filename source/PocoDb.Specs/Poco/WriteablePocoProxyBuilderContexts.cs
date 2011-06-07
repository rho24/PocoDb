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
            session = fake.an<IInternalWriteablePocoSession>();
            changes = fake.an<IChangeTracker>();

            A.CallTo(() => session.ChangeTracker).Returns(changes);

            sut_setup.run(sut => sut.Initialise(session));
        };

        protected static IInternalWriteablePocoSession session;
        protected static IChangeTracker changes;
    }

    [Subject(typeof (WriteablePocoProxyBuilder))]
    public class with_a_writable_poco_proxy : with_a_new_WriteablePocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(
                new Dictionary<IProperty, object>() {
                                                        {new Property<DummyObject, string>(d => d.FirstName), null},
                                                        {new Property<DummyObject, string>(d => d.LastName), null},
                                                        {new Property<DummyObject, DummyObject>(d => d.Child), null}
                                                    });

            sut_setup.run(sut => proxy = (DummyObject) sut.BuildProxy(meta));
        };

        protected static IPocoMeta meta;
        protected static DummyObject proxy;
    }

    [Subject(typeof (WriteablePocoProxyBuilder))]
    public class with_a_writable_poco_proxy_with_values : with_a_new_WriteablePocoProxyBuilder
    {
        Establish c = () => {
            meta = fake.an<IPocoMeta>();
            A.CallTo(() => meta.Type).Returns(typeof (DummyObject));
            A.CallTo(() => meta.Properties).Returns(
                new Dictionary<IProperty, object>() {
                                                        {new Property<DummyObject, string>(d => d.FirstName), "value"},
                                                        {new Property<DummyObject, string>(d => d.LastName), "value"},
                                                        {new Property<DummyObject, DummyObject>(d => d.Child), childId}
                                                    });

            sut_setup.run(sut => proxy = (DummyObject) sut.BuildProxy(meta));
        };

        protected static IPocoMeta meta;
        protected static DummyObject proxy;
        protected static IPocoId childId;
    }
}