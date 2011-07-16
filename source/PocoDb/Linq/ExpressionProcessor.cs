using System;
using System.Linq.Expressions;

namespace PocoDb.Linq
{
    public class ExpressionProcessor : IExpressionProcessor
    {
        public Expression Process(Expression expression) {
            var visitor = new Visitor();
            return visitor.Visit(expression);
        }

        class Visitor : ExpressionVisitor
        {
            protected override Expression VisitMember(MemberExpression node) {
                if (node.Expression is ParameterExpression)
                    return base.VisitMember(node);

                var value = Expression.Lambda(node).Compile().DynamicInvoke();

                return base.VisitConstant(Expression.Constant(value));
            }
        }
    }
}