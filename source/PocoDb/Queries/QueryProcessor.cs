using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Extensions;
using PocoDb.Indexing;
using PocoDb.Linq;
using PocoDb.Meta;
using PocoDb.Pocos;
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

                if (indexMatch.IsPartial)
                    return
                        (IQueryResult)
                        GenericHelper.InvokeGeneric(() => ProcessWithPartialIndex<object>(indexMatch.Index, query),
                                                    query.Expression.Type);

                return ProcessWithExactIndex(indexMatch.Index);
            }
            else {
                var indexMatch = Server.IndexManager.RetrieveIndex(query.Expression);

                var result = new EnumerablePocoQueryResult();
                result.Ids = indexMatch.Index.GetIds();
                result.Metas = Server.MetaStore.Get(result.Ids);

                return result;
            }
        }

        IQueryResult ProcessWithPartialIndex<T>(IIndex index, IQuery query) {
            var pocoGetter = new ServerPocoGetter(Server);
            var ids = index.GetIds();
            var pocos = ids.Select(i => (T)pocoGetter.GetPoco(i));

            var newQuery = QueryableToEnumerableConverter.Convert(query.Expression, index.IndexExpression, pocos);

            var result = Expression.Lambda<Func<T>>(newQuery).Compile().Invoke();
            var id = pocoGetter.IdsMetasAndProxies.Ids[result];
            var meta = Server.MetaStore.Get(id);

            return new SinglePocoQueryResult() {Id = id, Metas = new[] {meta}};
        }

        IQueryResult ProcessWithExactIndex(IIndex index) {
            var id = index.GetIds().FirstOrDefault();

            var result = new SinglePocoQueryResult();
            result.Id = id;
            result.Metas = id == null ? new IPocoMeta[] {} : new[] {Server.MetaStore.Get(id)};

            return result;
        }
    }

    public class ServerPocoGetter : ICanGetPocos
    {
        public IPocoDbServer Server { get; private set; }
        public IIdsMetasAndProxies IdsMetasAndProxies { get; private set; }

        public ServerPocoGetter(IPocoDbServer server) {
            Server = server;
            IdsMetasAndProxies = new IdsMetasAndProxies();
        }

        public object GetPoco(IPocoId id) {
            var meta = Server.MetaStore.Get(id);

            if (meta == null)
                throw new ArgumentException("id is not recognised");
            
            var pocoProxyBuilder = new ReadOnlyPocoProxyBuilder();
            var collectionProxyBuilder = new ReadOnlyCollectionProxyBuilder();
            pocoProxyBuilder.Initialise(this);
            collectionProxyBuilder.Initialise(this);
            var pocoFactory = new PocoFactory(pocoProxyBuilder, collectionProxyBuilder);

            return pocoFactory.Build(meta, IdsMetasAndProxies);
        }
    }

    internal class IndexQueryableExecutor : IPocoQueryableExecutor
    {
        public IIndex Index { get; private set; }

        public IndexQueryableExecutor(IIndex index) {
            Index = index;
        }

        public T Execute<T>(Expression expression) {
            throw new NotImplementedException();
        }
    }

    public interface IQueryableToEnumerableConverter
    {
        Expression Convert<T>(Expression expression, Expression replace, IEnumerable<T> with);
    }
}