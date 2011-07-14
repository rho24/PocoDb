using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Extensions;
using PocoDb.Queries;
using PocoDb.Session;

namespace PocoDb.Linq
{
    public class PocoQueryableExecutor : IPocoQueryableExecutor
    {
        public IInternalPocoSession Session { get; private set; }

        public PocoQueryableExecutor(IInternalPocoSession session) {
            Session = session;
        }

        public T Execute<T>(Expression expression) {
            var returnType = typeof (T);

            //TODO: these need to be worked out the same on client and server...
            if (returnType.IsEnumerable())
                return (T) GenericHelper.InvokeGeneric(() => ExecuteEnumerable<object>(expression),
                                                       returnType.EnumerableInnerType());

            if (returnType.IsPocoType())
                return (T) GenericHelper.InvokeGeneric(() => ExecuteSingle<object>(expression), returnType);

            throw new NotImplementedException();
        }

        IQueryResult GetQueryResult(Expression expression) {
            var result = Session.Server.Query(new Query(expression));

            foreach (var meta in result.Metas) {
                Session.IdsMetasAndProxies.Metas.Add(meta.Id, meta);
            }
            return result;
        }

        IEnumerable<T> ExecuteEnumerable<T>(Expression expression) {
            var result = GetQueryResult(expression);

            var enumerableResult = result as EnumerablePocoQueryResult;
            if (enumerableResult == null)
                throw new IncorrectQueryResultType(typeof (EnumerablePocoQueryResult), result.GetType());

            return enumerableResult.Ids.Select(id => (T) Session.GetPoco(id));
        }

        T ExecuteSingle<T>(Expression expression) {
            var result = GetQueryResult(expression);

            var singleResult = result as SinglePocoQueryResult;
            if (singleResult == null)
                throw new IncorrectQueryResultType(typeof (SinglePocoQueryResult), result.GetType());

            if (singleResult.Id == null) {
                if (expression.IsFirstQuery() || expression.IsSingleQuery())
                    throw new InvalidOperationException("No items in sequence");

                if (expression.IsFirstOrDefaultQuery() || expression.IsSingleOrDefaultQuery())
                    return default(T);

                throw new NotSupportedException("Unknown expression type");
            }

            return (T) Session.GetPoco(singleResult.Id);
        }
    }
}