using System;
using System.Collections.Generic;

namespace PocoDb.Commits
{
    public class Commit : ICommit
    {
        public ICommitId Id { get; private set; }

        public ICollection<AddedPoco> AddedPocos { get; set; }
        public ICollection<SetProperty> SetProperties { get; set; }
        public ICollection<CollectionAddition> CollectionAdditions { get; set; }
        public ICollection<CollectionRemoval> CollectionRemovals { get; set; }

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