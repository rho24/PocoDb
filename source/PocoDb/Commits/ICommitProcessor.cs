using System;

namespace PocoDb.Commits
{
    public interface ICommitProcessor
    {
        void Apply(ICommit commit);
    }
}