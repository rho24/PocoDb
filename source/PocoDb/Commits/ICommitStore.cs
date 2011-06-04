using System;

namespace PocoDb.Commits
{
    public interface ICommitStore
    {
        void Save(ICommit commit);
    }
}