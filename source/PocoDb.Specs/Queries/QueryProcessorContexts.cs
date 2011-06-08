using System;
using developwithpassion.specifications.fakeiteasy;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Persistence;
using PocoDb.Queries;
using PocoDb.Server;

namespace PocoDb.Specs.Queries
{
    [Subject(typeof (QueryProcessor))]
    public class with_a_new_QueryProcessor : Observes<QueryProcessor>
    {
        Establish c = () => {
            indexManager = depends.on<IIndexManager>();

            server = fake.an<IPocoDbServer>();
            metaStore = fake.an<IMetaStore>();

            A.CallTo(() => server.MetaStore).Returns(metaStore);

            sut_setup.run(sut => sut.Initialise(server));
        };

        protected static IIndexManager indexManager;
        protected static IPocoDbServer server;
        protected static IMetaStore metaStore;
    }
}