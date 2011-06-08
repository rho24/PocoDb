using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PocoDb.Linq
{
    public class PocoQueryable<T> : IQueryable<T>
    {
        public IQueryProvider Provider { get; private set; }
        public Type ElementType { get; private set; }
        public Expression Expression { get; private set; }

        public PocoQueryable(PocoQueryProvider provider) {
            Provider = provider;
            ElementType = typeof (T);
            Expression = Expression.Constant(this);
        }

        public PocoQueryable(PocoQueryProvider provider, Expression expression) {
            Provider = provider;
            ElementType = typeof (T);
            Expression = expression;
        }

        public IEnumerator<T> GetEnumerator() {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}