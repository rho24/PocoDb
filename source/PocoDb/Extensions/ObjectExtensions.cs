using System;

namespace PocoDb.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsPocoType(this object value) {
            if (value is string ||
                value is int ||
                value is float ||
                value is long ||
                value is double ||
                value is DateTime)
                return false;

            return true;
        }
    }
}