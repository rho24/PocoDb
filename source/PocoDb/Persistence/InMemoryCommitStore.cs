using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Commits;

namespace PocoDb.Persistence
{
    public class InMemoryCommitStore : ICommitStore
    {
        public List<ICommit> Commits { get; private set; }

        public InMemoryCommitStore() {
            Commits = new List<ICommit>();
        }

        public void Save(ICommit commit) {
            Commits.Add(commit);
        }

        public ICommit Get(ICommitId id) {
            return Commits.Where(c => c.Id == id).FirstOrDefault();
        }

        public IEnumerable<ICommit> GetAll() {
            return Commits.OrderBy(c => c.Id);
        }
    }
}