using System;
using System.Collections.Generic;
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

        public SingleQueryResult ProcessSingle(IQuery query) {
            var ids = GetIds(query.Expression.GetInnerQuery());

            return BuildSingleResult(ids);
        }

        public ElementQueryResult ProcessElement(IQuery query) {
            var ids = GetIds(query.Expression.GetInnerQuery());

            var id = query.Expression.IsLastQuery() ? ids.LastOrDefault() : ids.FirstOrDefault();

            return BuildElementResult(id);
        }

        public EnumerableQueryResult ProcessEnumerable(IQuery query) {
            var ids = GetIds(query.Expression);

            return BuildEnumerableResult(ids);
        }

        IEnumerable<IPocoId> GetIds(Expression expression) {
            var indexMatch = Server.IndexManager.RetrieveIndex(expression);
            IEnumerable<IPocoId> ids;
            if (indexMatch.IsExact)
                ids = indexMatch.Index.GetIds();
            else
                ids = (IEnumerable<IPocoId>)
                      GenericHelper.InvokeGeneric(
                          () => GetIdsFromPartialIndex<object>(indexMatch.Index, expression),
                          expression.Type.QueryableInnerType());
            return ids;
        }

        IEnumerable<IPocoId> GetIdsFromPartialIndex<T>(IIndex index, Expression expression) {
            var pocoGetter = new ServerPocoGetter(Server);
            var ids = index.GetIds();
            var pocos = pocoGetter.GetPocos(ids).Cast<T>();

            var newExpression = QueryableToEnumerableConverter.Convert(expression, index.IndexExpression, pocos);

            var result = Expression.Lambda<Func<IEnumerable<T>>>(newExpression).Compile().Invoke();
            return result.Select(p => pocoGetter.IdsMetasAndProxies.Ids[p]);
        }

        SingleQueryResult BuildSingleResult(IEnumerable<IPocoId> ids) {
            var first = ids.FirstWithStats();

            if (first.HasMany)
                return new SingleQueryResult {HasMany = true};

            var result = new SingleQueryResult {ElementId = first.Element};

            if (result.ElementId != null)
                result.Metas = new[] {Server.MetaStore.Get(result.ElementId)};

            return result;
        }

        ElementQueryResult BuildElementResult(IPocoId id) {
            var result = new ElementQueryResult {ElementId = id};

            if (result.ElementId != null)
                result.Metas = new[] {Server.MetaStore.Get(result.ElementId)};

            return result;
        }

        EnumerableQueryResult BuildEnumerableResult(IEnumerable<IPocoId> ids) {
            var result = new EnumerableQueryResult {
                                                       ElementIds = ids,
                                                       Metas = Server.MetaStore.Get(ids)
                                                   };

            return result;
        }
    }
}