using System;
using System.Collections.Generic;

namespace PocoDb.Commits
{
    public interface ICommit
    {
        ICommitId Id { get; }
        IEnumerable<AddedPoco> AddedPocos { get; }
        IEnumerable<SetProperty> SetProperties { get; }
        IEnumerable<CollectionAddition> CollectionAdditions { get; }
        IEnumerable<CollectionRemoval> CollectionRemovals { get; }
    }
}