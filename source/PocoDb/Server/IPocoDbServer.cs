using System;
using PocoDb.Commits;
using PocoDb.Persistence;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public interface IPocoDbServer
    {
        IMetaStore MetaStore { get; }
        ICommitStore CommitStore { get; }

        IPocoQueryResult Query(IPocoQuery query);
        void Commit(ICommit commit);
    }
}