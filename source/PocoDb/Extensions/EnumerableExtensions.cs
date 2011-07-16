using System;
using System.Collections.Generic;

namespace PocoDb.Extensions
{
    public static class EnumerableExtensions
    {
        public static FirstWithStatsResult<T> FirstWithStats<T>(this IEnumerable<T> sequence) {
            var firstLoop = true;
            var firstElement = default(T);
            foreach (var element in sequence) {
                if (firstLoop)
                    firstElement = element;
                else {
                    return new FirstWithStatsResult<T>(firstElement, true);
                }

                firstLoop = false;
            }

            return new FirstWithStatsResult<T>(firstElement, false);
        }
    }

    public class FirstWithStatsResult<T>
    {
        public T Element { get; private set; }
        public bool HasMany { get; private set; }

        public FirstWithStatsResult(T element, bool hasMany) {
            Element = element;
            HasMany = hasMany;
        }
    }
}