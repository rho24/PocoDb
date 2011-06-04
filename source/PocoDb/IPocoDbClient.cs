using System;

namespace PocoDb
{
    public interface IPocoDbClient
    {
        IPocoSession BeginSession();
        IWritablePocoSession BeginWritableSession();
    }
}