using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


        public static bool ImplementsCollectionType(this Type type) {
            return type.GetInterface("ICollection`1") != null;
        }

        public static Type GetCollectionType(this Type type) {
            if (!type.ImplementsCollectionType())
                throw new ArgumentException("value is not an ICollection<>");

            return type.GetInterface("ICollection`1");
        }

        public static Type GetCollectionInnerType(this Type type) {
            if (!type.ImplementsCollectionType())
                throw new ArgumentException("value is not an ICollection<>");

            return type.GetInterface("ICollection`1").GetGenericArguments()[0];
        }


        public static IEnumerable<IProperty> PublicVirtualProperties(this Type type) {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite && p.GetGetMethod().IsVirtual && p.GetIndexParameters().Count() == 0)
                .Select(p => Property.Create(p));
        }

        public static Type ConvertToEnumerable(this Type type) {
            if (type == typeof (IQueryable<>))
                return typeof (IEnumerable<>);

            if (type.IsGenericType) {
                var genericArguements = type.GetGenericArguments().Select(a => ConvertToEnumerable(a)).ToArray();

                if (type.GetGenericTypeDefinition() == typeof (IQueryable<>))
                    return typeof (IEnumerable<>).MakeGenericType(genericArguements);

                return type.GetGenericTypeDefinition().MakeGenericType(genericArguements);
            }

            return type;
        }

        public static Type StripExpressionFromFunc(this Type type) {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Expression<>)) {
                var genericArgument = type.GetGenericArguments()[0];
                if (genericArgument.Name.StartsWith("Func`"))
                    return genericArgument;
            }
            return type;
        }
    }
}