using System;
using PocoDb.Commits;

namespace PocoDb.Persistence
{
    public class InMemoryCommitStore : ICommitStore
    {
        public void Save(ICommit commit) {
            throw new NotImplementedException();
        }
    }
}