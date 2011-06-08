using System;
using System.Linq;
using PocoDb.Extensions;
using PocoDb.Server;

namespace PocoDb.Queries
{
    public class QueryProcessor : IQueryProcessor
    {
        public IPocoDbServer Server { get; private set; }

        public void Initialise(IPocoDbServer server) {
            Server = server;
        }

        public IPocoQueryResult Process(IPocoQuery query) {
            if (query.Expression.IsFirstCall() || query.Expression.IsFirstOrDefaultCall()) {
                var queryExpression = query.Expression.GetQuery();
                var indexMatch = Server.IndexManager.RetrieveIndex(queryExpression);

                var id = indexMatch.Index.GetIds().FirstOrDefault();

                var result = new PocoQueryResult();
                result.Ids = new[] {id};
                result.Metas = new[] {Server.MetaStore.Get(id)};

                return result;
            }
            else {
                var indexMatch = Server.IndexManager.RetrieveIndex(query.Expression);

                var result = new PocoQueryResult();
                result.Ids = indexMatch.Index.GetIds();
                result.Metas = Server.MetaStore.Get(result.Ids);

                return result;
            }
        }
    }
}