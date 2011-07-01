using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace PocoDb.Persistence.SqlServer
{
    public class SqlServerConnectionFactory : IDbConnectionFactory
    {
        readonly string _connectionString;

        public SqlServerConnectionFactory(string connectionString) {
            _connectionString = connectionString;
        }

        public DbConnection CreateOpenConnection() {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}