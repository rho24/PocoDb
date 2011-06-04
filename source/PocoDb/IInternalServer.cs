using System;
using PocoDb.Commits;

namespace PocoDb
{
    public interface IInternalServer
    {
        void Commit(ICommit commit);
    }
}