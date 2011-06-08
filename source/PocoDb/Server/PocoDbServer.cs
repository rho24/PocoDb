using System;
using PocoDb.Commits;
using PocoDb.Persistence;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public class PocoDbServer : IPocoDbServer
    {
        protected IQueryProcessor QueryProcessor { get; private set; }
        public ICommitProcessor CommitProcessor { get; private set; }

        public IMetaStore MetaStore { get; private set; }
        public ICommitStore CommitStore { get; private set; }

        public PocoDbServer(IMetaStore metaStore, ICommitStore commitStore, IQueryProcessor queryProcessor,
                            ICommitProcessor commitProcessor) {
            MetaStore = metaStore;
            CommitStore = commitStore;
            QueryProcessor = queryProcessor;
            CommitProcessor = commitProcessor;
        }

        public IPocoQueryResult Query(IPocoQuery query) {
            return QueryProcessor.Process(query);
        }

        public void Commit(ICommit commit) {
            CommitStore.Save(commit);

            CommitProcessor.Apply(commit);
        }
    }
}