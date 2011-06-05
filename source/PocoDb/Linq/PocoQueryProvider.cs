using System;
using System.Linq;
using System.Linq.Expressions;

namespace PocoDb.Linq
{
    public class PocoQueryProvider : IQueryProvider
    {
        protected IPocoQueryableExecutor Executor { get; private set; }

        public PocoQueryProvider(IPocoQueryableExecutor executor) {
            Executor = executor;
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
            return Executor.Execute<T>(expression);
        }
    }
}