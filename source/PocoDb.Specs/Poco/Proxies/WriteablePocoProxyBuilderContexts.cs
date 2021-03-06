﻿using System;
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
    [Subject(typeof (WriteablePocoProxyBuilder))]
    public class with_a_new_WriteablePocoProxyBuilder : Observes<WriteablePocoProxyBuilder>
    {
        Establish c = () => {
            pocoGetter = fake.an<ICanGetPocos>();
            changeTracker = fake.an<IChangeTracker>();

            sut_setup.run(sut => sut.Initialise(pocoGetter, changeTracker));
        };

        protected static ICanGetPocos pocoGetter;
        protected static IChangeTracker changeTracker;
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
                                                        {new Property<DummyObject, ChildObject>(d => d.Child), null}
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
                                                        {new Property<DummyObject, ChildObject>(d => d.Child), childId}
                                                    });

            sut_setup.run(sut => proxy = (DummyObject) sut.BuildProxy(meta));
        };

        protected static IPocoMeta meta;
        protected static DummyObject proxy;
        protected static IPocoId childId;
    }
}