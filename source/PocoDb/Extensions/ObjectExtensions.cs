using System;

namespace PocoDb.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsPocoType(this object value) {
            return value.GetType().IsPocoType();
        }

        public static bool IsCollectionType(this object value) {
            return value.GetType().IsCollectionType();
        }
    }
}