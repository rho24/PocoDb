using System;
using PocoDb.Commits;

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