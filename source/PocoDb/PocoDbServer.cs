using System;
using PocoDb.Commits;

namespace PocoDb
{
    public class PocoDbServer : IPocoDbServer
    {
        public void Commit(ICommit commit) {}
    }
}