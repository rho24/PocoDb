using System;

namespace PocoDb.Commits
{
    public class CommitIdGenerator : ICommitIdGenerator
    {
        public ICommitId New() {
            var id = Guid.NewGuid();
            return new CommitId(id, DateTime.Now);
        }
    }
}