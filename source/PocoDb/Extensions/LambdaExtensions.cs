using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PocoDb.Extensions
{
    //TODO: need to clean this up alot.
    public static class LambdaExtensions
    {
        public static void InvokeGeneric(this Expression<Action> expression, Type genericType) {
            GetObjectAndMethodAndArguments(expression.Body, genericType);
        }

        public static object InvokeGeneric(this Expression<Func<object>> expression, Type genericType) {
            return GetObjectAndMethodAndArguments(expression.Body, genericType);
        }

        static object GetObjectAndMethodAndArguments(Expression expression, Type genericType) {
            if (expression is UnaryExpression) {
                //static method
                return GetObjectAndMethodAndArguments(((UnaryExpression) expression).Operand, genericType);
            }

            if (expression is MethodCallExpression) {
                var methodCall = (MethodCallExpression) expression;

                if (!methodCall.Method.IsGenericMethod)
                    throw new ArgumentException("expression is not a generic method");

                var obj = GetObjectValue(methodCall.Object);
                var method = methodCall.Method.GetGenericMethodDefinition().MakeGenericMethod(genericType);
                var args = methodCall.Arguments.Select(a => GetObjectValue(a)).ToArray();

                return method.Invoke(obj, args);
            }

            if (expression is NewExpression) {
                var constructorCall = (NewExpression) expression;

                if (!constructorCall.Type.IsGenericType)
                    throw new ArgumentException("expression is not a generic method");

                var constructor = constructorCall.Type.GetGenericTypeDefinition().MakeGenericType(genericType)
                    .GetConstructor(constructorCall.Constructor.GetParameters().Select(p => p.ParameterType).ToArray());
                var args = constructorCall.Arguments.Select(a => GetObjectValue(a)).ToArray();

                return constructor.Invoke(args);
            }

            throw new ArgumentException("expression is not a method call");
        }

        class ObjectAndMethodAndArguments
        {
            public object Object { get; set; }
            public MethodBase Method { get; set; }
            public object[] Arguments { get; set; }
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

            return null;
        }
    }
}