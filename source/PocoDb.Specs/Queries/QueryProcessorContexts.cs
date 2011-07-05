using System;
using FakeItEasy;
using Machine.Specifications;
using PocoDb.Indexing;
using PocoDb.Persistence;
using PocoDb.Queries;
using PocoDb.Server;
using developwithpassion.specifications.fakeiteasy;

namespace PocoDb.Specs.Queries
{
    [Subject(typeof (QueryProcessor))]
    public class with_a_new_QueryProcessor : Observes<QueryProcessor>
    {
        Establish c = () => {
            server = fake.an<IPocoDbServer>();
            metaStore = fake.an<IMetaStore>();
            indexManager = depends.on<IIndexManager>();

            A.CallTo(() => server.MetaStore).Returns(metaStore);
            A.CallTo(() => server.IndexManager).Returns(indexManager);

            sut_setup.run(sut => sut.Initialise(server));
        };

        protected static IIndexManager indexManager;
        protected static IPocoDbServer server;
        protected static IMetaStore metaStore;
    }
}