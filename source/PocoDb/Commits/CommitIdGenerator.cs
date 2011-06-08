using System;

namespace PocoDb.Commits
{
    public class CommitIdGenerator : ICommitIdGenerator
    {
        public ICommitId New() {
            return new CommitId();
        }
    }
}