using System;
using System.Collections.Generic;
using System.Linq;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Meta;
using PocoDb.Pocos;
using PocoDb.Server;

namespace PocoDb.Session
{
    public class WritablePocoSession : IWritablePocoSession, IInternalWriteablePocoSession
    {
        public IPocoDbServer PocoDbServer { get; private set; }
        public IPocoFactory PocoFactory { get; private set; }
        public ICommitBuilder CommitBuilder { get; set; }
        public ITrackedChanges Changes { get; private set; }


        public WritablePocoSession(IPocoDbServer pocoDbServer, IPocoFactory pocoFactory, ICommitBuilder commitBuilder) {
            PocoDbServer = pocoDbServer;
            PocoFactory = pocoFactory;
            CommitBuilder = commitBuilder;

            Changes = new TrackedChanges();
        }

        public IQueryable<T> Get<T>() {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetWritable<T>() {
            throw new NotImplementedException();
        }

        public void Add<T>(T poco) {
            throw new NotImplementedException();
        }

        public void SaveChanges() {
            var commit = CommitBuilder.Build(Changes);

            PocoDbServer.Commit(commit);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }

        public IPocoDbServer Server { get { throw new NotImplementedException(); } }

        public IDictionary<IPocoId, IPocoMeta> Metas { get { throw new NotImplementedException(); } }

        public IDictionary<IPocoId, object> TrackedPocos { get { throw new NotImplementedException(); } }

        public object GetPoco(IPocoId id) {
            throw new NotImplementedException();
        }
    }
}