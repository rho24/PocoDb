using System;
using System.Linq;
using PocoDb.Extensions;
using PocoDb.Meta;
using PocoDb.Server;

namespace PocoDb.Queries
{
    public class QueryProcessor : IQueryProcessor
    {
        public IPocoDbServer Server { get; private set; }

        public void Initialise(IPocoDbServer server) {
            Server = server;
        }

        public IQueryResult Process(IQuery query) {
            if (query.Expression.IsFirstCall() || query.Expression.IsFirstOrDefaultCall()) {
                var queryExpression = query.Expression.GetInnerQuery();
                var indexMatch = Server.IndexManager.RetrieveIndex(queryExpression);

                var id = indexMatch.Index.GetIds().FirstOrDefault();

                var result = new SinglePocoQueryResult();
                result.Id = id;
                result.Metas = id == null ? new IPocoMeta[] {} : new[] {Server.MetaStore.Get(id)};

                return result;
            }
            else {
                var indexMatch = Server.IndexManager.RetrieveIndex(query.Expression);

                var result = new EnumerablePocoQueryResult();
                result.Ids = indexMatch.Index.GetIds();
                result.Metas = Server.MetaStore.Get(result.Ids);

                return result;
            }
        }
    }
}