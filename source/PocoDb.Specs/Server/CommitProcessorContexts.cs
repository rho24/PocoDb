﻿using System;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Persistence;
using PocoDb.Server;

namespace PocoDb.Specs.Server
{
    [Subject(typeof (ICommitProcessor))]
    public class with_a_new_CommitProcessor : Observes<CommitProcessor>
    {
        Establish c = () => {
            server = fake.an<IPocoDbServer>();
            metaStore = fake.an<IMetaStore>();
            A.CallTo(() => server.MetaStore).Returns(metaStore);

            sut_setup.run(sut => sut.Initialise(server));

            commit = fake.an<ICommit>();
        };

        protected static IPocoDbServer server;
        protected static IMetaStore metaStore;
        protected static ICommit commit;
    }
}