using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Extensions;
using PocoDb.Queries;
using PocoDb.Server;

namespace PocoDb.Linq
{
    public class PocoQueryProvider : IQueryProvider
    {
        public IPocoDbServer Server { get; private set; }

        public PocoQueryProvider(IPocoDbServer server) {
            Server = server;
        }

        public IQueryable CreateQuery(Expression expression) {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) {
            throw new NotImplementedException();
        }

        public object Execute(Expression expression) {
            throw new NotImplementedException();
        }

        public T Execute<T>(Expression expression) {
            var returnType = typeof (T);

            if (returnType.IsEnumerable())
                return
                    (T)
                    LambdaExtensions.InvokeGeneric(() => ExecuteEnumerable<object>(expression),
                                                   returnType.EnumerableInnerType());

            return default(T);
        }

        IEnumerable<T> ExecuteEnumerable<T>(Expression expression) {
            var result = Server.Query(new PocoQuery(expression));

            return Enumerable.Empty<T>();
        }
    }
}