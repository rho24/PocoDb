using System;
using System.Collections.Generic;

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

        public static bool IsPocoType(this Type type) {
            if (type == typeof (string) ||
                type == typeof (int) ||
                type == typeof (float) ||
                type == typeof (long) ||
                type == typeof (double) ||
                type == typeof (DateTime))
                return false;

            return true;
        }

        public static bool IsCollectionType(this Type type) {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof (ICollection<>);
        }
    }
}