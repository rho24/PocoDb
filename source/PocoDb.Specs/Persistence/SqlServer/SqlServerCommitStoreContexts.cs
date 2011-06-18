using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using developwithpassion.specifications.fakeiteasy;
using Machine.Specifications;
using PocoDb.Commits;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Persistence.SqlServer;
using PocoDb.Serialisation;

namespace PocoDb.Specs.Persistence.SqlServer
{
    [Subject(typeof (SqlServerCommitStore))]
    [SetupForEachSpecification]
    public class with_a_new_SqlServerCommitStore : Observes<SqlServerCommitStore>
    {
        Establish c = () => {
            var dbFileName = "JsonDb.sdf";
            var connectionString = "Data Source={0};Persist Security Info=False;".Fmt(dbFileName);

            CreateDatabase(dbFileName, connectionString);

            depends.on<IDbConnectionFactory>(new CompactDbConnectionFactory(connectionString));
            depends.on<ISerializer>(new JsonSerializer());
        };

        static void CreateDatabase(string dbFileName, string connectionString) {
            var dbFile = new FileInfo(dbFileName);

            if (dbFile.Exists)
                dbFile.Delete();

            dbFile.Create().Close();

            var script = File.ReadAllText("PocoDbSqlSchema.sql");
            var splitScripts = script.Split(new[] {"GO"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Replace("\n", "").Replace("\r", "")).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            using (var connection = new SqlCeConnection(connectionString)) {
                connection.Open();
                using (var trans = connection.BeginTransaction(IsolationLevel.Serializable)) {
                    foreach (var commandText in splitScripts) {
                        using (var command = connection.CreateCommand()) {
                            command.CommandText = commandText;
                            if (command.ExecuteNonQuery() == 0)
                                throw new InvalidOperationException("Failed to build db");
                        }
                    }
                    trans.Commit();
                }
            }
        }
    }

    internal class CompactDbConnectionFactory : IDbConnectionFactory
    {
        readonly string _connectionString;

        public CompactDbConnectionFactory(string connectionString) {
            _connectionString = connectionString;
        }

        public DbConnection CreateOpenConnection() {
            var connection = new SqlCeConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }

    [Subject(typeof (SqlServerCommitStore))]
    public class with_a_populated_SqlServerCommitStore : with_a_new_SqlServerCommitStore
    {
        Establish c = () => {
            commit = new Commit(new CommitId(Guid.Empty));
            commit.AddedPocos.Add(new AddedPoco(new PocoMeta(new PocoId(Guid.Empty), typeof (DummyObject))));
            commit.SetProperties.Add(new SetProperty(new PocoId(Guid.Empty),
                                                     new Property<DummyObject, string>(d => d.FirstName), "value"));
            commit.CollectionAdditions.Add(new CollectionAddition(new PocoId(Guid.Empty), "value"));
            commit.CollectionRemovals.Add(new CollectionRemoval(new PocoId(Guid.Empty), "value"));

            sut_setup.run(sut => sut.Save(commit));
        };

        protected static Commit commit;
    }
}