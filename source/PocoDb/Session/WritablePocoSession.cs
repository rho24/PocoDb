using System;
using System.Linq;
using PocoDb.ChangeTracking;
using PocoDb.Commits;
using PocoDb.Server;

namespace PocoDb.Session
{
    public class WritablePocoSession : IWritablePocoSession
    {
        public IPocoDbServer PocoDbServer { get; private set; }
        public ICommitBuilder CommitBuilder { get; set; }
        public ITrackedChanges Changes { get; private set; }


        public WritablePocoSession(IPocoDbServer pocoDbServer, ICommitBuilder commitBuilder) {
            PocoDbServer = pocoDbServer;
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
    }
}