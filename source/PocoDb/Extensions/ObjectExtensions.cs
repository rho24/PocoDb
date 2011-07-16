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


        public static bool ImplementsCollectionType(this object value) {
            if (value == null)
                return false;

            return value.GetType().ImplementsCollectionType();
        }

        public static Type GetCollectionType(this object value) {
            return value.GetType().GetCollectionType();
        }

        public static Type GetCollectionInnerType(this object value) {
            return value.GetType().GetCollectionInnerType();
        }

        public static T As<T>(this object obj) where T : class {
            return obj as T;
        }
    }
}