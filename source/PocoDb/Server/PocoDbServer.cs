using System;
using PocoDb.Commits;

namespace PocoDb.Server
{
    public class PocoDbServer : IPocoDbServer
    {
        public ICommitIdGenerator IdGenerator { get; private set; }
        protected ICommitStore CommitStore { get; private set; }
        protected ICommitProcessor CommitProcessor { get; set; }

        public PocoDbServer(ICommitIdGenerator idGenerator, ICommitStore commitStore, ICommitProcessor commitProcessor) {
            IdGenerator = idGenerator;
            CommitStore = commitStore;
            CommitProcessor = commitProcessor;
        }

        public void Commit(ICommit commit) {
            var id = IdGenerator.New();

            CommitStore.Save(commit);

            CommitProcessor.Apply(commit);
        }
    }
}