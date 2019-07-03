using System;
using System.Collections.Generic;
using System.Linq;

namespace deadrat22
{
    internal static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> ienumerable, Action<T> action)
        {
            foreach(var elem in ienumerable)
            {
                action(elem);
            }
        }

        public static int IndexOfOrDefault<T>(this IEnumerable<T> ienumerable, T value)
        {
            try
            {
                return ienumerable
                    .GetIndexedIEnumerable()
                    .First(indexed => indexed.Value.Equals(value)).Index;
            }
            catch
            {
                return -1;
            }
        }

        public static IEnumerable<IndexedValue<T>> GetIndexedIEnumerable<T>(this IEnumerable<T> ienumerable)
        {
            return ienumerable
                .Select((elem, index) => new IndexedValue<T>
                {
                    Index = index,
                    Value = elem
                });
        }
    }

    internal class IndexedValue<T>
    {
        public int Index { get; set; }
        public T Value { get; set; }
    }
}
