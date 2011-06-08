using System;
using PocoDb.Commits;
using PocoDb.Indexing;
using PocoDb.Meta;
using PocoDb.Persistence;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public interface IPocoDbServer
    {
        IMetaStore MetaStore { get; }
        ICommitStore CommitStore { get; }
        IIndexManager IndexManager { get; }

        IPocoQueryResult Query(IPocoQuery query);
        void Commit(ICommit commit);
        IPocoMeta GetMeta(IPocoId id);
    }
}