using System;
using PocoDb.Commits;
using PocoDb.Meta;
using PocoDb.Persistence;
using PocoDb.Pocos;
using PocoDb.Server;
using PocoDb.Session;

namespace PocoDb
{
    public class PocoDbClient : IPocoDbClient
    {
        IPocoDbServer Server { get; set; }

        public PocoDbClient() {
            Server = new PocoDbServer(new CommitIdGenerator(), new InMemoryCommitStore(),
                                      new CommitProcessor(new InMemoryMetaStore()));
        }

        public IPocoSession BeginSession() {
            var pocoProxyBuilder = new ReadOnlyPocoProxyBuilder();
            var collectionProxyBuilder = new ReadOnlyCollectionProxyBuilder();
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);
            var session = new PocoSession(Server, pocoFactory);

            pocoProxyBuilder.Initialise(session);
            collectionProxyBuilder.Initialise(session);
            pocoFactory.Initialise(session);

            return session;
        }

        public IWritablePocoSession BeginWritableSession() {
            var pocoProxyBuilder = new WriteablePocoProxyBuilder();
            var collectionProxyBuilder = new WritableCollectionProxyBuilder();
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);
            var idGenerator = new CommitIdGenerator();
            var pocoMetaBuilder = new PocoMetaBuilder();
            var commitBuilder = new CommitBuilder(idGenerator, pocoMetaBuilder);
            var session = new WritablePocoSession(Server, pocoFactory, commitBuilder);

            pocoProxyBuilder.Initialise(session);
            collectionProxyBuilder.Initialise(session);
            pocoFactory.Initialise(session);

            return session;
        }
    }
}