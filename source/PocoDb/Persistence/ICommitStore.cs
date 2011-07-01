using System;
using System.Collections.Generic;
using PocoDb.Commits;

namespace PocoDb.Persistence
{
    public interface ICommitStore
    {
        void Save(ICommit commit);
        ICommit Get(ICommitId id);
        IEnumerable<ICommit> GetAll();
    }
}