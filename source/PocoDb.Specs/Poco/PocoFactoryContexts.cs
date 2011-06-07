﻿using System;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Pocos;
using PocoDb.Session;

namespace PocoDb.Specs.Poco
{
    [Subject(typeof (IPocoFactory))]
    public class with_a_new_PocoFactory : Observes<PocoFactory>
    {
        Establish c = () => {
            session = fake.an<IInternalPocoSession>();
            sut_setup.run(sut => sut.Initialise(session));

            proxyBuilder = depends.on<IPocoProxyBuilder>();
            collectionProxyBuilder = depends.on<ICollectionProxyBuilder>();
        };

        protected static IInternalPocoSession session;
        protected static IPocoProxyBuilder proxyBuilder;
        protected static ICollectionProxyBuilder collectionProxyBuilder;
    }
}