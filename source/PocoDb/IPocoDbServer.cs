using System;
using PocoDb.Commits;

namespace PocoDb
{
    public interface IPocoDbServer
    {
        void Commit(ICommit commit);
    }
}