using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PocoDb.Meta;

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

        public static IEnumerable<IProperty> PublicVirtualProperties(this Type type) {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite && p.GetGetMethod().IsVirtual)
                .Select(p => Property.Create(p));
        }
    }
}