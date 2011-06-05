using System;
using System.Linq.Expressions;

namespace PocoDb.Queries
{
    public class PocoQuery
    {
        public Expression Expression { get; private set; }

        public PocoQuery(Expression expression) {
            Expression = expression;
        }
    }
}