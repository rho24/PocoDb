﻿using System;
using PocoDb.Commits;
using PocoDb.Indexing;
using PocoDb.Meta;
using PocoDb.Persistence;
using PocoDb.Pocos;
using PocoDb.Queries;

namespace PocoDb.Server
{
    public class PocoDbServer : IPocoDbServer
    {
        public IMetaStore MetaStore { get; private set; }
        public ICommitStore CommitStore { get; private set; }
        public IQueryProcessor QueryProcessor { get; private set; }
        public ICommitProcessor CommitProcessor { get; private set; }
        public IIndexManager IndexManager { get; private set; }
        public IPocoFactory PocoFactory { get; private set; }

        public PocoDbServer(IMetaStore metaStore, ICommitStore commitStore, IQueryProcessor queryProcessor,
                            ICommitProcessor commitProcessor, IIndexManager indexManager, IPocoFactory pocoFactory) {
            MetaStore = metaStore;
            CommitStore = commitStore;
            QueryProcessor = queryProcessor;
            CommitProcessor = commitProcessor;
            IndexManager = indexManager;
            PocoFactory = pocoFactory;
        }

        public IQueryResult Query(IQuery query) {
            return QueryProcessor.Process(query);
        }

        public IPocoMeta GetMeta(IPocoId id) {
            return MetaStore.Get(id);
        }

        public void Commit(ICommit commit) {
            CommitStore.Save(commit);

            CommitProcessor.Apply(commit);
        }
    }
}