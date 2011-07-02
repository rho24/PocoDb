using System;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Extensions;
using PocoDb.Indexing;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Server;

namespace PocoDb.Queries
{
    public class QueryProcessor : IQueryProcessor
    {
        public IPocoDbServer Server { get; private set; }
        protected IQueryableToEnumerableConverter QueryableToEnumerableConverter { get; private set; }

        public QueryProcessor(IQueryableToEnumerableConverter queryableToEnumerableConverter) {
            QueryableToEnumerableConverter = queryableToEnumerableConverter;
        }

        public void Initialise(IPocoDbServer server) {
            Server = server;
        }

        public IQueryResult Process(IQuery query) {
            if (query.Expression.IsFirstCall() || query.Expression.IsFirstOrDefaultCall()) {
                var queryExpression = query.Expression.GetInnerQuery();
                var indexMatch = Server.IndexManager.RetrieveIndex(queryExpression);
                IPocoId id;
                if (indexMatch.IsPartial)
                    id =
                        (IPocoId)
                        GenericHelper.InvokeGeneric(() => ProcessWithPartialIndex<object>(indexMatch.Index, query),
                                                    query.Expression.Type);
                else
                    id = ProcessWithExactIndex(indexMatch.Index);

                return BuildSingleResult(id);
            }
            else {
                var indexMatch = Server.IndexManager.RetrieveIndex(query.Expression);

                var result = new EnumerablePocoQueryResult();
                result.Ids = indexMatch.Index.GetIds();
                result.Metas = Server.MetaStore.Get(result.Ids);

                return result;
            }
        }

        IPocoId ProcessWithPartialIndex<T>(IIndex index, IQuery query) {
            var pocoGetter = new ServerPocoGetter(Server);
            var ids = index.GetIds();
            var pocos = pocoGetter.GetPocos(ids).Cast<T>();

            var newQuery = QueryableToEnumerableConverter.Convert(query.Expression, index.IndexExpression, pocos);

            var result = Expression.Lambda<Func<T>>(newQuery).Compile().Invoke();
            return result == null ? null : pocoGetter.IdsMetasAndProxies.Ids[result];
        }

        IPocoId ProcessWithExactIndex(IIndex index) {
            return index.GetIds().FirstOrDefault();
        }

        IQueryResult BuildSingleResult(IPocoId id) {
            var result = new SinglePocoQueryResult();
            result.Id = id;
            result.Metas = id == null ? new IPocoMeta[] {} : new[] {Server.MetaStore.Get(id)};

            return result;
        }
    }
}