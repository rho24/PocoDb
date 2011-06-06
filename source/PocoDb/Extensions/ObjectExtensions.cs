using System;

namespace PocoDb.Extensions
{
    public static class ObjectExtensions
    {
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

        public static bool IsPocoType(this object value) {
            return IsPocoType(value.GetType());
        }
    }
}