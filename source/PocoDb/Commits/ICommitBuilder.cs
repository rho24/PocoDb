using System;
using PocoDb.ChangeTracking;
using PocoDb.Pocos;

namespace PocoDb.Commits
{
    public interface ICommitBuilder
    {
        ICommit Build(ITrackedChanges trackedChanges, IIdsMetasAndProxies idsMetasAndProxies);
    }
}