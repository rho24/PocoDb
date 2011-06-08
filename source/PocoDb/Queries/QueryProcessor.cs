using System;
using PocoDb.Indexing;
using PocoDb.Server;

namespace PocoDb.Queries
{
    public class QueryProcessor : IQueryProcessor
    {
        public IPocoDbServer Server { get; set; }
        public IIndexManager IndexManager { get; private set; }

        public QueryProcessor(IIndexManager indexManager) {
            IndexManager = indexManager;
        }

        public void Initialise(IPocoDbServer server) {
            Server = server;
        }

        public IPocoQueryResult Process(IPocoQuery query) {
            var index = IndexManager.RetrieveIndex(query.Expression);

            var result = new PocoQueryResult();
            result.Ids = index.GetIds();

            result.Metas = Server.MetaStore.Get(result.Ids);

            return result;
        }
    }
}