using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using PocoDb.Commits;
using PocoDb.Serialisation;

namespace PocoDb.Persistence.SqlServer
{
    public class SqlServerCommitStore : ICommitStore
    {
        public string NameOrConnectionString { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public SqlServerCommitStore(string nameOrConnectionString, ISerializer serializer) {
            NameOrConnectionString = nameOrConnectionString;
            Serializer = serializer;
        }

        public void Save(ICommit commit) {
            if (commit == null)
                throw new ArgumentNullException("commit");

            var id = Serializer.Serialize(commit.Id);
            var value = Serializer.Serialize(commit);

            using (var context = new Context(NameOrConnectionString)) {
                if (context.Commits.Any(c => c.Id == id))
                    throw new InvalidOperationException("Store already contains id");

                context.Commits.Add(new SqlCommit() {Id = id, Value = value});
                context.SaveChanges();
            }
        }

        public ICommit Get(ICommitId id) {
            if (id == null)
                throw new ArgumentNullException("id");

            var serialisedId = Serializer.Serialize(id);

            using (var context = new Context(NameOrConnectionString)) {
                var sqlCommit = context.Commits.FirstOrDefault(c => c.Id == serialisedId);

                if (sqlCommit == null)
                    return null;

                var commit = Serializer.Deserialize<ICommit>(sqlCommit.Value);
                return commit;
            }
        }

        public class Context : DbContext
        {
            public DbSet<SqlCommit> Commits { get; set; }

            public Context(string nameOrConnectionString) : base(nameOrConnectionString) {}

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
                modelBuilder.Entity<SqlCommit>()
                    .HasKey(c => c.Id)
                    .Property(c => c.Value).IsRequired();

                base.OnModelCreating(modelBuilder);
            }
        }
    }
}