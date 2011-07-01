using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using PocoDb.Extensions;
using PocoDb.Persistence.SqlServer;

namespace PocoDb.Specs
{
    public static class CompactDbHelper
    {
        public static IDbConnectionFactory CreateFreshDb() {
            var dbFileName = "JsonDb.sdf";
            var connectionString = "Data Source={0};Persist Security Info=False;".Fmt(dbFileName);

            CreateDatabase(dbFileName, connectionString);

            return new CompactDbConnectionFactory(connectionString);
        }

        static void CreateDatabase(string dbFileName, string connectionString) {
            var dbFile = new FileInfo(dbFileName);

            if (dbFile.Exists)
                dbFile.Delete();

            dbFile.Create().Close();

            var script = File.ReadAllText("PocoDbSqlSchema.sql");
            var splitScripts = script.Split(new[] {"GO"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Replace("\n", "").Replace("\r", "")).Where(s => !String.IsNullOrWhiteSpace(s)).ToList();

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

        class CompactDbConnectionFactory : IDbConnectionFactory
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
    }
}