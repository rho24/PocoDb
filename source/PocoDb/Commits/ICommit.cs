using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.Meta;

namespace PocoDb.Commits
{
    public interface ICommit
    {
        ICommitId Id { get; }
        IEnumerable<AddedPoco> AddedPocos { get; }
        ILookup<IPocoId, SetProperty> UpdatedPocos { get; }
        IEnumerable<CollectionAddition> CollectionAdditions { get; }
        IEnumerable<CollectionRemoval> CollectionRemovals { get; }
    }
}