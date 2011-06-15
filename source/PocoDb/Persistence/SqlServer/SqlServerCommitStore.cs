using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Newtonsoft.Json;
using PocoDb.Commits;

namespace PocoDb.Persistence.SqlServer
{
    public class SqlServerCommitStore : ICommitStore
    {
        public string NameOrConnectionString { get; private set; }
        public JsonSerializerSettings JsonSettings { get; private set; }

        public SqlServerCommitStore(string nameOrConnectionString) {
            NameOrConnectionString = nameOrConnectionString;
            JsonSettings = new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.Objects};
            JsonSettings.Converters.Add(new PropertyConverter());
        }

        public void Save(ICommit commit) {
            if (commit == null)
                throw new ArgumentNullException("commit");

            var id = JsonConvert.SerializeObject(commit.Id, Formatting.None, JsonSettings);
            var value = JsonConvert.SerializeObject(commit, Formatting.None, JsonSettings);

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

            var serialisedId = JsonConvert.SerializeObject(id, Formatting.None, JsonSettings);

            using (var context = new Context(NameOrConnectionString)) {
                var sqlCommit = context.Commits.FirstOrDefault(c => c.Id == serialisedId);

                if (sqlCommit == null)
                    return null;

                var commit = JsonConvert.DeserializeObject(sqlCommit.Value, JsonSettings);
                return commit as ICommit;
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