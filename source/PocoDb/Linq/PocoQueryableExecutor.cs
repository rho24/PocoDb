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

            if (returnType.IsEnumerable())
                return (T) LambdaExtensions.InvokeGeneric(() => ExecuteEnumerable<object>(expression),
                                                          returnType.EnumerableInnerType());

            var result = GetQueryResult(expression);

            return result.Ids.Select(id => (T) Session.GetPoco(id)).FirstOrDefault();
        }

        IEnumerable<T> ExecuteEnumerable<T>(Expression expression) {
            var result = GetQueryResult(expression);

            return result.Ids.Select(id => (T) Session.GetPoco(id));
        }

        PocoQueryResult GetQueryResult(Expression expression) {
            var result = Session.Server.Query(new PocoQuery(expression));

            foreach (var meta in result.Metas) {
                Session.Metas.Add(meta);
            }
            return result;
        }
    }
}