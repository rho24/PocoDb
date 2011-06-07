using System;
using PocoDb.Commits;

namespace PocoDb.Persistence
{
    public interface ICommitStore
    {
        void Save(ICommit commit);
        ICommit Get(ICommitId id);
    }
}