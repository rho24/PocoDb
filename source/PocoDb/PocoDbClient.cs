using System;
using PocoDb.Session;

namespace PocoDb
{
    public class PocoDbClient : IPocoDbClient
    {
        public IPocoSession BeginSession() {
            throw new NotImplementedException();
        }

        public IWritablePocoSession BeginWritableSession() {
            throw new NotImplementedException();
        }
    }
}