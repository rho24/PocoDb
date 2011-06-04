using System;

namespace PocoDb.Commits
{
    public interface ICommitIdGenerator
    {
        ICommitId New();
    }
}