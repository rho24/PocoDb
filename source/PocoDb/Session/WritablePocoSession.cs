﻿using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Server;

namespace PocoDb.Session
{
    public class WritablePocoSession : IWritablePocoSession, IInternalWritablePocoSession
    {
        public IPocoDbServer Server { get; private set; }
        public IPocoFactory PocoFactory { get; private set; }
        public ICommitBuilder CommitBuilder { get; set; }
        public IDictionary<IPocoId, IPocoMeta> Metas { get; private set; }
        public IDictionary<IPocoId, object> TrackedPocos { get; private set; }
        public IDictionary<object, IPocoId> TrackedIds { get; private set; }
        public IChangeTracker ChangeTracker { get; private set; }


        public WritablePocoSession(IPocoDbServer server, IPocoFactory pocoFactory, ICommitBuilder commitBuilder) {
            Server = server;
            PocoFactory = pocoFactory;
            CommitBuilder = commitBuilder;

            Metas = new Dictionary<IPocoId, IPocoMeta>();
            TrackedPocos = new Dictionary<IPocoId, object>();
            TrackedIds = new Dictionary<object, IPocoId>();
            ChangeTracker = new ChangeTracker();
        }


        //IPocoSession
        public IQueryable<T> Get<T>() {
            return new PocoQueryable<T>(new PocoQueryProvider(new PocoQueryableExecutor(this)));
        }

        //IWritablePocoSession
        ITrackedChanges IWritablePocoSession.Changes { get { return ChangeTracker.Changes; } }

        //IWritablePocoSession
        public IQueryable<T> GetWritable<T>() {
            throw new NotImplementedException();
        }

        //IWritablePocoSession
        public void Add<T>(T poco) {
            throw new NotImplementedException();
        }

        //IWritablePocoSession
        public void SaveChanges() {
            var commit = CommitBuilder.Build(ChangeTracker.Changes);

            Server.Commit(commit);
        }

        //IInternalPocoSession
        public object GetPoco(IPocoId id) {
            var meta = Metas[id];
            if (meta == null)
                throw new ArgumentException("id is not recognised");

            return PocoFactory.Build(meta);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}