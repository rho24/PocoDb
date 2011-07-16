using System;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Linq;

namespace PocoDb.Extensions
{
    public static class ExpressionExtensions
    {
        public static bool IsSingleQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.IsSingleCall() || expression.IsSingleOrDefaultCall();
        }

        public static bool IsElementQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.IsSingleCall() || expression.IsSingleOrDefaultCall() || expression.IsFirstCall() ||
                   expression.IsFirstOrDefaultCall() || expression.IsLastCall() || expression.IsLastOrDefaultCall();
        }

        public static bool IsFirstQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.IsFirstCall() || expression.IsFirstOrDefaultCall();
        }

        public static bool IsLastQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.IsLastCall() || expression.IsLastOrDefaultCall();
        }

        public static bool IsOrDefaultQuery(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            return expression.IsSingleOrDefaultCall() || expression.IsFirstOrDefaultCall() ||
                   expression.IsLastOrDefaultCall();
        }

        public static bool IsSingleCall(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                return false;

            return callExpression.Method.DeclaringType == typeof (Queryable) && callExpression.Method.Name == "Single";
        }

        public static bool IsSingleOrDefaultCall(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                return false;

            return callExpression.Method.DeclaringType == typeof (Queryable) &&
                   callExpression.Method.Name == "SingleOrDefault";
        }

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

        public static bool IsLastCall(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                return false;

            return callExpression.Method.DeclaringType == typeof (Queryable) && callExpression.Method.Name == "Last";
        }

        public static bool IsLastOrDefaultCall(this Expression expression) {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var callExpression = expression as MethodCallExpression;

            if (callExpression == null)
                return false;

            return callExpression.Method.DeclaringType == typeof (Queryable) &&
                   callExpression.Method.Name == "LastOrDefault";
        }

        public static Expression GetInnerQuery(this Expression expression) {
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

            var callExpression = expression as MethodCallExpression;
            if (callExpression != null) {
                if (callExpression.Arguments.Count == 0)
                    return false;

                return callExpression.Arguments[0].IsQuery();
            }

            return expression.IsQueryBase();
        }

        public static bool IsQueryBase(this Expression query) {
            var constantExpression = query as ConstantExpression;
            if (constantExpression == null)
                return false;

            return constantExpression.Type.GetGenericTypeDefinition() == typeof (PocoQueryable<>);
        }


        public static Type GetQueryPocoType(this Expression query) {
            if (!query.IsQueryBase())
                return GetQueryPocoType(query.GetInnerQuery());

            return query.Type.GetGenericArguments()[0];
        }
    }
}