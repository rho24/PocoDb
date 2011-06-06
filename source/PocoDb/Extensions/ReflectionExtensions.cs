using System;
using System.Reflection;

namespace PocoDb.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool IsProperty(this MethodInfo method) {
            return method.IsSpecialName &&
                   (method.Name.StartsWith("get_", StringComparison.Ordinal) ||
                    method.Name.StartsWith("set_", StringComparison.Ordinal));
        }

        public static bool IsPropertyGetter(this MethodInfo method) {
            return method.IsSpecialName && method.Name.StartsWith("get_", StringComparison.Ordinal);
        }


        public static bool IsPropertySetter(this MethodInfo method) {
            return method.IsSpecialName && method.Name.StartsWith("set_", StringComparison.Ordinal);
        }

        public static PropertyInfo GetPropertyInfo(this MethodInfo method) {
            return method.DeclaringType.GetProperty(method.Name.Substring(4));
        }
    }
}