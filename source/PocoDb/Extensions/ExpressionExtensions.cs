using System;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Linq;

namespace PocoDb.Extensions
{
    public static class ExpressionExtensions
    {
        public static bool IsFirstCall(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                return false;

            return callExpression.Method.DeclaringType == typeof (Queryable) && callExpression.Method.Name == "First";
        }

        public static bool IsFirstOrDefaultCall(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                return false;

            return callExpression.Method.DeclaringType == typeof (Queryable) &&
                   callExpression.Method.Name == "FirstOrDefault";
        }

        public static Expression GetQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                throw new ArgumentException("expression is not a CallExpression");

            var query = callExpression.Arguments[0];

            if (!query.IsQuery())
                throw new ArgumentException("expression does not contain a valid Query");

            return query;
        }

        public static bool IsQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var constantExpression = expression as ConstantExpression;

            if (constantExpression == null)
                return false;

            return constantExpression.Type.GetGenericTypeDefinition() == typeof (PocoQueryable<>);
        }

        public static bool MatchesQuery(this Expression query, Expression otherQuery) {
            if (!query.IsQuery())
                throw new ArgumentException("query is not a valid Query");

            if (!otherQuery.IsQuery())
                throw new ArgumentException("otherQuery is not a valid Query");

            return query.Type == otherQuery.Type;

            //TODO: this won't work for anything other than constants.
        }


        public static Type QueryPocoType(this Expression query) {
            if (!query.IsQuery())
                throw new ArgumentException("query is not a valid Query");

            return query.Type.GetGenericArguments()[0];
        }
    }
}