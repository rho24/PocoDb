using System;

namespace PocoDb.Commits
{
    public interface ICommitStore
    {
        void Save(ICommit commit);
        ICommit GetCommit(ICommitId id);
    }
}