using System;
using PocoDb.ChangeTracking;

namespace PocoDb.Commits
{
    public interface ICommitBuilder
    {
        ICommit Build(ITrackedChanges changes);
    }
}