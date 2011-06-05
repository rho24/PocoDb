using System;

namespace PocoDb.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsEnumerable(this Type type) {
            return type.Name == "IEnumerable`1";
        }

        public static Type EnumerableInnerType(this Type type) {
            if (!type.IsEnumerable())
                throw new ArgumentException("type is not IEnumerable");

            return type.GetGenericArguments()[0];
        }
    }
}