using System;
using System.Collections.Generic;
using System.Data;
using PocoDb.Commits;
using PocoDb.Serialisation;

namespace PocoDb.Persistence.SqlServer
{
    public class SqlServerCommitStore : ICommitStore
    {
        public string NameOrConnectionString { get; private set; }
        public IDbConnectionFactory DbConnectionFactory { get; private set; }
        protected ISerializer Serializer { get; private set; }

        public SqlServerCommitStore(IDbConnectionFactory dbConnectionFactory, ISerializer serializer) {
            DbConnectionFactory = dbConnectionFactory;
            Serializer = serializer;
        }

        public void Save(ICommit commit) {
            if (commit == null)
                throw new ArgumentNullException("commit");

            var id = Serializer.Serialize(commit.Id);
            var value = Serializer.Serialize(commit);

            using (var connection = DbConnectionFactory.CreateOpenConnection())
            using (var trans = connection.BeginTransaction(IsolationLevel.Serializable))
            using (var command = connection.CreateCommand()) {
                command.Transaction = trans;
                command.CommandText = "INSERT INTO SqlCommits (Id, Value) VALUES (@Id, @Value)";
                command.Parameters.Add(command.CreateParameter());
                command.Parameters[0].ParameterName = "Id";
                command.Parameters[0].Value = id;
                command.Parameters.Add(command.CreateParameter());
                command.Parameters[1].ParameterName = "Value";
                command.Parameters[1].Value = value;

                if (command.ExecuteNonQuery() == 0)
                    throw new InvalidOperationException("Failed to insert a row");

                trans.Commit();
            }
        }

        public ICommit Get(ICommitId id) {
            if (id == null)
                throw new ArgumentNullException("id");

            var serialisedId = Serializer.Serialize(id);

            using (var connection = DbConnectionFactory.CreateOpenConnection())
            using (var command = connection.CreateCommand()) {
                command.CommandText = "SELECT Value FROM SqlCommits WHERE Id = @Id";
                command.Parameters.Add(command.CreateParameter());
                command.Parameters[0].ParameterName = "Id";
                command.Parameters[0].Value = serialisedId;

                var result = command.ExecuteReader();

                if (!result.Read())
                    return null;

                var value = result["Value"].ToString();

                return Serializer.Deserialize<ICommit>(value);
            }
        }

        public IEnumerable<ICommit> GetAll() {
            using (var connection = DbConnectionFactory.CreateOpenConnection())
            using (var command = connection.CreateCommand()) {
                command.CommandText = "SELECT Value FROM SqlCommits ORDER BY Id";

                var result = command.ExecuteReader();

                while (result.Read()) {
                    var value = result["Value"].ToString();

                    yield return Serializer.Deserialize<ICommit>(value);
                }
            }
        }
    }
}