using System;
using PocoDb.Commits;

namespace PocoDb.Server
{
    public interface IPocoDbServer
    {
        void Commit(ICommit commit);
    }
}