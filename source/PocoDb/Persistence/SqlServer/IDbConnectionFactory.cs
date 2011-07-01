using System;
using System.Data.Common;

namespace PocoDb.Persistence.SqlServer
{
    public interface IDbConnectionFactory
    {
        DbConnection CreateOpenConnection();
    }
}