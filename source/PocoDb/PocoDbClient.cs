using System;
using PocoDb.Commits;
using PocoDb.Indexing;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Persistence;
using PocoDb.Persistence.SqlServer;
using PocoDb.Pocos;
using PocoDb.Pocos.Proxies;
using PocoDb.Queries;
using PocoDb.Serialisation;
using PocoDb.Server;
using PocoDb.Session;

namespace PocoDb
{
    public class PocoDbClient : IPocoDbClient
    {
        IPocoDbServer Server { get; set; }

        public PocoDbClient() : this(new InMemoryCommitStore()) {}

        public PocoDbClient(IDbConnectionFactory connectionFactory)
            : this(new SqlServerCommitStore(connectionFactory, new JsonSerializer())) {}

        PocoDbClient(ICommitStore commitStore) {
            if (commitStore == null) throw new ArgumentNullException("commitStore");

            var partialQueryReplacer = new QueryableToEnumerableConverter();
            var queryProcessor = new QueryProcessor(partialQueryReplacer);
            var commitProcessor = new CommitProcessor();
            var indexManager = new IndexManager();

            Server = new PocoDbServer(new InMemoryMetaStore(), commitStore, queryProcessor,
                                      commitProcessor, indexManager);

            queryProcessor.Initialise(Server);
            commitProcessor.Initialise(Server);

            foreach (var commit in commitStore.GetAll()) {
                commitProcessor.Apply(commit);
            }
        }

        public IPocoSession BeginSession() {
            var pocoProxyBuilder = new ReadOnlyPocoProxyBuilder();
            var collectionProxyBuilder = new ReadOnlyCollectionProxyBuilder();
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);
            var expressionProcessor = new ExpressionProcessor();
            var session = new PocoSession(Server, pocoFactory, expressionProcessor);

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
            var expressionProcessor = new ExpressionProcessor();
            var session = new WritablePocoSession(Server, pocoFactory, commitBuilder, expressionProcessor);

            pocoProxyBuilder.Initialise(session, session.ChangeTracker);
            collectionProxyBuilder.Initialise(session, session.ChangeTracker);

            return session;
        }
    }
}