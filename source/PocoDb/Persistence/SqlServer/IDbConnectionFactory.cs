using System;
using System.Data.Common;

public interface IDbConnectionFactory
{
    DbConnection CreateOpenConnection();
}