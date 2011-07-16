using System;
using PocoDb.Commits;
using PocoDb.Indexing;
using PocoDb.Meta;
using PocoDb.Persistence;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public class PocoDbServer : IPocoDbServer
    {
        public IMetaStore MetaStore { get; private set; }
        public ICommitStore CommitStore { get; private set; }
        public IQueryProcessor QueryProcessor { get; private set; }
        public ICommitProcessor CommitProcessor { get; private set; }
        public IIndexManager IndexManager { get; private set; }

        public PocoDbServer(IMetaStore metaStore, ICommitStore commitStore, IQueryProcessor queryProcessor,
                            ICommitProcessor commitProcessor, IIndexManager indexManager) {
            MetaStore = metaStore;
            CommitStore = commitStore;
            QueryProcessor = queryProcessor;
            CommitProcessor = commitProcessor;
            IndexManager = indexManager;
        }

        public IPocoMeta GetMeta(IPocoId id) {
            return MetaStore.Get(id);
        }

        public SingleQueryResult QuerySingle(Query query) {
            return QueryProcessor.ProcessSingle(query);
        }

        public ElementQueryResult QueryElement(Query query) {
            return QueryProcessor.ProcessElement(query);
        }

        public EnumerableQueryResult QueryEnumerable(Query query) {
            return QueryProcessor.ProcessEnumerable(query);
        }

        public void Commit(ICommit commit) {
            CommitStore.Save(commit);

            CommitProcessor.Apply(commit);
        }
    }
}