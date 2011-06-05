using System;
using PocoDb.Session;

namespace PocoDb
{
    public interface IPocoDbClient
    {
        IPocoSession BeginSession();
        IWritablePocoSession BeginWritableSession();
    }
}