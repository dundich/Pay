using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LinqEnumerable = System.Linq.Enumerable;

namespace Maybe2
{
    public static class EnumerableExtensions
    {
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<string> RemoveEmptyStrings(this IEnumerable<string> source)
        {
            if (source == null) return null;
            return source.Where(c => !c.IsNullOrWhiteSpace());
        }

        public static string JoinStrings(this IEnumerable<string> source, string Delimiter = "", bool removeEmptyStrings = true)
        {
            if (source == null) return null;
            if (removeEmptyStrings)
                source = source.RemoveEmptyStrings();
            if (source.IsNullOrEmpty()) return string.Empty;
            return source.Aggregate((s1, s2) => s1 + Delimiter + s2);
        }

        public static string CommaText(this IEnumerable<string> source, string Delimiter = ",", string Quote = "'", bool isUnique = true, bool removeEmptyEntries = true)
        {
            if (source == null) return null;
            if (isUnique)
                source = source.Distinct();
            if (removeEmptyEntries)
                source = source.Where(c => !c.IsNullOrEmpty());

            if (!source.Any()) return null;

            return source
                .Select(s => Quote + s + Quote)
                .Aggregate((s1, s2) => s1 + Delimiter + s2);
        }


        [DebuggerStepThrough]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null && action != null)
            {
                foreach (var item in source)
                    action(item);
            }
            return source;
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source != null && action != null)
            {
                var i = 0;
                foreach (var item in source)
                    action(item, i++);
            }
            return source;
        }


        public static IEnumerable<T> Concat<T>(this T head, IEnumerable<T> tail)
        {
            if (tail == null) throw new ArgumentNullException("tail");
            return LinqEnumerable.Concat(LinqEnumerable.Repeat(head, 1), tail);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> head, T tail)
        {
            if (head == null) throw new ArgumentNullException("head");
            return LinqEnumerable.Concat(head, LinqEnumerable.Repeat(tail, 1));
        }

        public static IEnumerable<TResult> Generate<TResult>(TResult initial, Func<TResult, TResult> generator)
        {
            if (generator == null) throw new ArgumentNullException("generator");
            var current = initial;
            while (true)
            {
                yield return current;
                current = generator(current);
            }
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable.IsNullOrEmpty()) return default(T);
            var r = new Random();
            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.ElementAt(r.Next(0, list.Count()));
        }

    }
}
