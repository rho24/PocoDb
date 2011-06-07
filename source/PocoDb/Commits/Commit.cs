using System;
using System.Collections.Generic;

namespace PocoDb.Commits
{
    public class Commit : ICommit
    {
        public ICommitId Id { get; private set; }

        public List<AddedPoco> AddedPocos { get; private set; }
        public List<SetProperty> SetProperties { get; private set; }
        public List<CollectionAddition> CollectionAdditions { get; private set; }
        public List<CollectionRemoval> CollectionRemovals { get; private set; }

        IEnumerable<AddedPoco> ICommit.AddedPocos { get { return AddedPocos; } }
        IEnumerable<SetProperty> ICommit.SetProperties { get { return SetProperties; } }
        IEnumerable<CollectionAddition> ICommit.CollectionAdditions { get { return CollectionAdditions; } }
        IEnumerable<CollectionRemoval> ICommit.CollectionRemovals { get { return CollectionRemovals; } }

        public Commit(ICommitId id) {
            Id = id;

            AddedPocos = new List<AddedPoco>();
            SetProperties = new List<SetProperty>();
            CollectionAdditions = new List<CollectionAddition>();
            CollectionRemovals = new List<CollectionRemoval>();
        }
    }
}