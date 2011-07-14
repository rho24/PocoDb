using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public class Commit : ICommit
    {
        public ICommitId Id { get; private set; }

        public ICollection<AddedPoco> AddedPocos { get; set; }
        public ICollection<Tuple<IPocoId, SetProperty>> UpdatedPocos { get; set; }
        public ICollection<CollectionAddition> CollectionAdditions { get; set; }
        public ICollection<CollectionRemoval> CollectionRemovals { get; set; }

        IEnumerable<AddedPoco> ICommit.AddedPocos { get { return AddedPocos; } }
        ILookup<IPocoId, SetProperty> ICommit.UpdatedPocos { get { return UpdatedPocos.ToLookup(u => u.Item1, u => u.Item2); } }
        IEnumerable<CollectionAddition> ICommit.CollectionAdditions { get { return CollectionAdditions; } }
        IEnumerable<CollectionRemoval> ICommit.CollectionRemovals { get { return CollectionRemovals; } }

        public Commit(ICommitId id) {
            Id = id;

            AddedPocos = new List<AddedPoco>();
            UpdatedPocos = new List<Tuple<IPocoId, SetProperty>>();
            CollectionAdditions = new List<CollectionAddition>();
            CollectionRemovals = new List<CollectionRemoval>();
        }
    }
}