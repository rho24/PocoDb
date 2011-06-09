using System;
using PocoDb.Commits;
using PocoDb.Indexing;
using PocoDb.Meta;
using PocoDb.Persistence;
using PocoDb.Pocos;
using PocoDb.Queries;
using PocoDb.Server;
using PocoDb.Session;

namespace PocoDb
{
    public class PocoDbClient : IPocoDbClient
    {
        IPocoDbServer Server { get; set; }

        public PocoDbClient() {
            var partialQueryReplacer = new QueryableToEnumerableConverter();
            var queryProcessor = new QueryProcessor(partialQueryReplacer);
            var commitProcessor = new CommitProcessor();
            var indexManager = new IndexManager();

            Server = new PocoDbServer(new InMemoryMetaStore(), new InMemoryCommitStore(), queryProcessor,
                                      commitProcessor, indexManager);

            queryProcessor.Initialise(Server);
            commitProcessor.Initialise(Server);
        }

        public IPocoSession BeginSession() {
            var pocoProxyBuilder = new ReadOnlyPocoProxyBuilder();
            var collectionProxyBuilder = new ReadOnlyCollectionProxyBuilder();
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);
            var session = new PocoSession(Server, pocoFactory);

            pocoProxyBuilder.Initialise(session);
            collectionProxyBuilder.Initialise(session);

            return session;
        }

        public IWritablePocoSession BeginWritableSession() {
            var pocoProxyBuilder = new WriteablePocoProxyBuilder();
            var collectionProxyBuilder = new WritableCollectionProxyBuilder();
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);
            var idGenerator = new CommitIdGenerator();
            var pocoIdBuilder = new PocoIdBuilder();
            var pocoMetaBuilder = new PocoMetaBuilder(pocoIdBuilder);
            var commitBuilder = new CommitBuilder(idGenerator, pocoMetaBuilder);
            var session = new WritablePocoSession(Server, pocoFactory, commitBuilder);

            pocoProxyBuilder.Initialise(session, session.ChangeTracker);
            collectionProxyBuilder.Initialise(session, session.ChangeTracker);

            return session;
        }
    }
}