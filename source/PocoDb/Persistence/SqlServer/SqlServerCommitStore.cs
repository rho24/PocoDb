using System;
using System.Collections.Generic;
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

        public SqlServerCommitStore(string nameOrConnectionString) {
            NameOrConnectionString = nameOrConnectionString;
        }

        public void Save(ICommit commit) {
            if (commit == null)
                throw new ArgumentNullException("commit");

            var sqlCommit = commit as SqlCommit;
            if (sqlCommit == null)
                throw new ArgumentException("commit is not a SqlCommit");

            using (var context = new Context(NameOrConnectionString)) {
                context.Commits.Add(sqlCommit);
                context.SaveChanges();
            }
        }

        public ICommit Get(ICommitId id) {
            if (id == null)
                throw new ArgumentNullException("id");

            var guidId = id as CommitId;
            if (guidId == null)
                throw new ArgumentException("id is not a CommitId");

            using (var context = new Context(NameOrConnectionString)) {
                var commit = context.Commits.FirstOrDefault(c => c.Id == guidId.Id);

                return commit;
            }
        }

        public class Context : DbContext
        {
            public DbSet<SqlCommit> Commits { get; set; }

            public Context(string nameOrConnectionString) : base(nameOrConnectionString) {}

            protected override void OnModelCreating(ModelBuilder modelBuilder) {
                modelBuilder.Entity<SqlCommit>().HasKey(c => c.Id);

                modelBuilder.Entity<SqlCommit>().Property(c => c.AddedPocosValue).IsRequired();
                base.OnModelCreating(modelBuilder);
            }
        }
    }

    public class SqlCommit : ICommit
    {
        public Guid Id { get; set; }
        public string AddedPocosValue { get { return JsonConvert.SerializeObject(AddedPocos); } set { AddedPocos = JsonConvert.DeserializeObject<ICollection<AddedPoco>>(value); } }

        ICollection<AddedPoco> AddedPocos { get; set; }
        ICollection<SetProperty> SetProperties { get; set; }
        ICollection<CollectionAddition> CollectionAdditions { get; set; }
        ICollection<CollectionRemoval> CollectionRemovals { get; set; }

        public SqlCommit() {
            AddedPocos = new List<AddedPoco>();
            SetProperties = new List<SetProperty>();
            CollectionAdditions = new List<CollectionAddition>();
            CollectionRemovals = new List<CollectionRemoval>();
        }

        ICommitId _id;

        ICommitId ICommit.Id {
            get {
                if (Id == Guid.Empty)
                    return null;
                return _id ?? (_id = new CommitId(Id));
            }
        }

        IEnumerable<AddedPoco> ICommit.AddedPocos { get { return AddedPocos; } }
        IEnumerable<SetProperty> ICommit.SetProperties { get { return SetProperties; } }
        IEnumerable<CollectionAddition> ICommit.CollectionAdditions { get { return CollectionAdditions; } }
        IEnumerable<CollectionRemoval> ICommit.CollectionRemovals { get { return CollectionRemovals; } }
    }
}