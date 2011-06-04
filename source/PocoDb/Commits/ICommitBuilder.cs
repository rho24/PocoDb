using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PocoDb.ChangeTracking;

namespace PocoDb.Commits
{
    public interface ICommitBuilder
    {
        ICommit Build(ITrackedChanges changes);
    }
}