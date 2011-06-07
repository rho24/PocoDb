using System;
using PocoDb.Commits;
using PocoDb.Persistence;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public class PocoDbServer : IPocoDbServer
    {
        public ICommitIdGenerator IdGenerator { get; private set; }
        protected ICommitStore CommitStore { get; private set; }
        protected ICommitProcessor CommitProcessor { get; private set; }

        public PocoDbServer(ICommitIdGenerator idGenerator, ICommitStore commitStore, ICommitProcessor commitProcessor) {
            IdGenerator = idGenerator;
            CommitStore = commitStore;
            CommitProcessor = commitProcessor;
        }

        public PocoQueryResult Query(PocoQuery query) {
            throw new NotImplementedException();
        }

        public void Commit(ICommit commit) {
            var id = IdGenerator.New();

            CommitStore.Save(commit);

            CommitProcessor.Apply(commit);
        }
    }
}