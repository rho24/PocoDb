using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using PocoDb.Extensions;

namespace PocoDb.Queries
{
    public class QueryableToEnumerableConverter : IQueryableToEnumerableConverter
    {
        public Expression Convert<T>(Expression expression, Expression replace, IEnumerable<T> with) {
            var rewriter = new Rewriter(replace, Expression.Constant(with));

            return rewriter.Visit(expression);
        }

        class Rewriter : ExpressionVisitor
        {
            public Expression Replace { get; private set; }
            public Expression With { get; private set; }

            public Rewriter(Expression replace, Expression with) {
                Replace = replace;
                With = with;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node) {
                if (node.Method.DeclaringType == typeof (Queryable)) {
                    var genericTypes =
                        node.Method.GetGenericArguments().Select(t => t.ConvertToEnumerable().StripExpressionFromFunc())
                            .ToArray();

                    var requiredMethodParameterTypes =
                        node.Method.GetParameters().Select(
                            p => p.ParameterType.ConvertToEnumerable().StripExpressionFromFunc()).ToArray();

                    var methodsWithSameName =
                        typeof (Enumerable).GetMethods().Where(m => m.Name == node.Method.Name).Select(
                            m => m.MakeGenericMethod(genericTypes));

                    var methodsWithSameParameters = methodsWithSameName.Where(m => {
                        var parameterTypes = m.GetParameters().Select(p => p.ParameterType).ToArray();

                        return requiredMethodParameterTypes.All(t => parameterTypes.Contains(t));
                    });

                    var enumerableMethod = methodsWithSameParameters.FirstOrDefault();

                    if (enumerableMethod == null)
                        throw new NotSupportedException();

                    return Expression.Call(null, enumerableMethod, node.Arguments.Select(a => Visit(a)));
                }
                return base.VisitMethodCall(node);
            }

            protected override Expression VisitUnary(UnaryExpression node) {
                if (node.Type.IsGenericType && node.Type.GetGenericTypeDefinition() == typeof (Expression<>)) {
                    return base.Visit(node.Operand);
                }
                return base.VisitUnary(node);
            }

            public override Expression Visit(Expression node) {
                if (node != null && node.Type == Replace.Type)
                    return With;

                return base.Visit(node);
            }
        }
    }
}