using System;
using System.Linq.Expressions;

namespace PocoDb.Extensions
{
    public static class ExpressionExtensions
    {
        public static bool IsQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (!(expression is ConstantExpression))
                return false;

            var constantExpression = (ConstantExpression) expression;

            return constantExpression.Type.GetGenericTypeDefinition() == typeof (PocoDb.Linq.PocoQueryable<>);
        }

        public static bool MatchesQuery(this Expression query, Expression otherQuery) {
            if (!query.IsQuery())
                throw new ArgumentException("query is not a PocoQuery");

            if (!otherQuery.IsQuery())
                throw new ArgumentException("otherQuery is not a PocoQuery");

            return query.Type == otherQuery.Type;

            //TODO: this won't work for anything other than constants.
        }


        public static Type QueryPocoType(this Expression query) {
            if (!query.IsQuery())
                throw new ArgumentException("query is not a PocoQuery");

            return query.Type.GetGenericArguments()[0];
        }
    }
}