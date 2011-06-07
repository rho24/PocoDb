using System;

namespace PocoDb.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsPocoType(this object value) {
            if (value == null)
                return false;

            return value.GetType().IsPocoType();
        }

        public static bool IsCollectionType(this object value) {
            if (value == null)
                return false;

            return value.GetType().IsCollectionType();
        }
    }
}