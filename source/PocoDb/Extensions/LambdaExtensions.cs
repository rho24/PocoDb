using System;
using System.Linq;
using System.Linq.Expressions;

namespace PocoDb.Extensions
{
    //TODO: need to clean this up alot.
    public static class LambdaExtensions
    {
        public static void InvokeGeneric(this Expression<Action> exp, Type genericType) {
            throw new NotImplementedException();
        }

        public static object InvokeGeneric(this Expression<Func<object>> expression, Type genericType) {
            var body = expression.Body as UnaryExpression;
            if (body != null) {
                //Static method.

                var operand = body.Operand as MethodCallExpression;
                if (operand == null)
                    throw new ArgumentException("expression is not a method call");

                var method = operand.Method;
                if (!method.IsGenericMethod)
                    throw new ArgumentException("expression is not a generic method");

                var arguments = operand.Arguments;

                return method.MakeGenericMethod(genericType).Invoke(null, arguments.ToArray());
            }
            else {
                var operand = expression.Body as MethodCallExpression;
                if (operand == null)
                    throw new ArgumentException("expression is not a method call");

                var method = operand.Method;
                if (!method.IsGenericMethod)
                    throw new ArgumentException("expression is not a generic method");

                var obj = GetObjectValue(operand.Object);

                var arguments = operand.Arguments;

                var newMethod = method.GetGenericMethodDefinition().MakeGenericMethod(genericType);

                return newMethod.Invoke(obj, arguments.Select(i => GetObjectValue(i)).ToArray());
            }
        }

        static object GetObjectValue(Expression expression) {
            if (expression is ConstantExpression) {
                return (expression as ConstantExpression).Value;
            }

            if (expression is MemberExpression) {
                var objectMember = Expression.Convert(expression, typeof (object));

                var getterLambda = Expression.Lambda<Func<object>>(objectMember);

                var getter = getterLambda.Compile();

                return getter();
            }

            throw new NotSupportedException("Can't resolve object for this case");
        }
    }
}