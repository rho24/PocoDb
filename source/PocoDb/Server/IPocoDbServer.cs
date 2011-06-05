using System;
using PocoDb.Commits;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public interface IPocoDbServer
    {
        PocoQueryResult Query(PocoQuery query);
        void Commit(ICommit commit);
    }
}