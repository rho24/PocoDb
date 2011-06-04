using System;
using System.Linq;
using PocoDb.ChangeTracking;
using PocoDb.Commits;

namespace PocoDb
{
    public class WritablePocoSession : IWritablePocoSession
    {
        public IInternalServer Server { get; private set; }
        public ICommitBuilder CommitBuilder { get; set; }
        public ITrackedChanges Changes { get; private set; }


        public WritablePocoSession(IInternalServer server, ICommitBuilder commitBuilder) {
            Server = server;
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

            Server.Commit(commit);
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}