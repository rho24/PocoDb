using System;
using System.Linq.Expressions;

namespace PocoDb.Queries
{
    public class Query : IQuery
    {
        public Expression Expression { get; private set; }

        public Query(Expression expression) {
            Expression = expression;
        }
    }
}