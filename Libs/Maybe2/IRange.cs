using System;
using System.Collections.Generic;

namespace Maybe2
{
    public interface IRange<T>
        where T : IComparable<T>
    {
        T start { get; set; }
        T end { get; set; }
        /// <summary>
        /// вкл-выкл в рассмотрение -> end
        /// те (по - включая) или (до невключая)
        /// </summary>
        bool isIncludeEnding { get; set; }
    }

    public class Range<T> : IRange<T>
        where T : IComparable<T>
    {
        public T start { get; set; }
        public T end { get; set; }

        public bool isIncludeEnding { get; set; }

        public Range() { }

        public override string ToString()
        {
            return start + " - " + end;
        }
    }


    public static class RangeT
    {
        /// <summary>
        /// (1,4,true).rangeTo=(1,2,3,4)        
        /// |
        /// (1,4,false).rangeTo=(1,2,3)        
        /// </summary>
        public static Range<T> rangeTo<T>(this T start, T end, bool isIncludeEnding)
            where T : IComparable<T>
        {
            return new Range<T> { start = start, end = end, isIncludeEnding = isIncludeEnding };
        }

        /// <summary>
        /// Точка как период где (start == end)
        /// </summary>
        public static Range<T> rangeFromPoint<T>(this T point)
            where T : IComparable<T>
        {
            return new Range<T> { start = point, end = point, isIncludeEnding = true };
        }

        /// <summary>
        /// (1-3).Swap()=(3-1)        
        /// </summary>
        public static IRange<T> Swap<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            var p = a.start;
            a.start = a.end;
            a.end = p;
            return a;
        }

        /// <summary>
        /// (1-5).IsPoint = FALSE
        /// |
        /// (5-5).IsPoint = TRUE
        /// </summary>
        public static bool IsPoint<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            return a.start.CompareTo(a.end) == 0;
        }

        /// <summary>
        /// (1-5).IsPositive = TRUE
        /// </summary>
        public static bool IsPositive<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            return a.end.CompareTo(a.start) > 0;
        }

        /// <summary>
        /// (5-1).IsPositive = FALSE
        /// </summary>
        public static bool IsNegative<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            return a.start.CompareTo(a.end) > 0;
        }

        /// <summary>
        /// (5-1).Normalize = (1-5)
        /// |
        /// (1-5).Normalize = (1-5)
        /// </summary>
        public static IRange<T> Normalize<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            return a.IsNegative() ? a.Swap() : a;
        }
        /// <summary>
        /// Перекрывает
        /// 
        ///  ------------ a
        ///    ---- b
        ///  ============
        ///  TRUE
        ///  
        /// ------
        ///   -------
        /// =========
        ///  FALSE
        /// </summary>
        public static bool IsOverlapped<T>(this IRange<T> a, IRange<T> b)
            where T : IComparable<T>
        {
            return a.IsIntersection(b) && a.start.CompareTo(b.start) <= 0 && a.end.CompareTo(b.end) >= 0;
        }

        /// <summary>
        /// (1-4)==(1-4)
        /// |
        /// (1-4, true)!=(1-4, false)
        /// </summary>
        public static bool IsEqual<T>(this IRange<T> a, IRange<T> b, bool checkEnding = true)
            where T : IComparable<T>
        {
            var eq = a.start.CompareTo(b.start) == 0 && b.end.CompareTo(a.end) == 0;
            if (eq && checkEnding)
                return a.isIncludeEnding == b.isIncludeEnding;
            return eq;
        }

        /// <summary>
        /// пересекает
        /// 
        ///   ---- a
        ///     ---- b
        ///   ======
        ///   TRUE
        /// </summary>
        public static bool IsIntersection<T>(this IRange<T> a, IRange<T> b)
            where T : IComparable<T>
        {
            return a.start.CompareTo(b.end) < 0 && b.start.CompareTo(a.end) < 0;
        }

        /// <summary>
        /// Пересечение
        /// (1-6).GetIntersection((2-8)) == (2-6)
        /// --------
        ///      -------
        /// =============
        ///      ---                      
        /// </summary>
        public static Range<T> GetIntersection<T>(this IRange<T> range1, IRange<T> range2)
            where T : IComparable<T>
        {
            var greatestStart = range1.start.CompareTo(range2.start) > 0 ? range1.start : range2.start;
            var smallestEnd = range1.end.CompareTo(range2.end) < 0 ? range1.end : range2.end;

            var r = greatestStart.CompareTo(smallestEnd);
            //no intersection
            if (r > 0) return null;

            if (range1.isIncludeEnding && r == 0) return null;

            return greatestStart.rangeTo(smallestEnd, range1.isIncludeEnding);
        }

        /// <summary>
        /// Ислючить период
        /// (1-6).Subtract((2-4)) == ((1-2),(4-6))
        ///  -------------------           
        ///      ------
        /// =====================
        ///  ----      ---------        
        /// </summary>
        public static IEnumerable<Range<T>> Subtract<T>(this IRange<T> a, IRange<T> exclude)
            where T : IComparable<T>
        {
            var sub = GetIntersection(a, exclude);
            if (sub != null)
            {
                if (sub.start.CompareTo(a.start) != 0)
                    yield return a.start.rangeTo(sub.start, a.isIncludeEnding);

                if (sub.end.CompareTo(a.end) != 0)
                    yield return sub.end.rangeTo(a.end, a.isIncludeEnding);
            }
        }

        /// <summary>
        /// Входит в диапазон
        /// (1-5).inRange(2) = TRUE
        /// |
        /// (1-5).inRange(12) = FALSE        
        /// </summary>
        public static bool InRange<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (point.CompareTo(a.start) < 0) return false;
            return a.isIncludeEnding ? point.CompareTo(a.end) <= 0 : point.CompareTo(a.end) < 0;
        }

        public static IEnumerable<T> Iterate<T>(this IRange<T> a, Func<T, T> nextPoint)
            where T : IComparable<T>
        {
            var p = a.start;
            while (a.InRange(p))
            {
                yield return p;
                p = nextPoint(p);
            }
        }

        /// <summary>
        /// (1-6).ChangeEnd(3) == (1-3)
        /// </summary>
        public static IRange<T> ChangeEnd<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            a.end = point;
            return a;
        }

        /// <summary>
        /// (1-6).ChangeStart(3) == (3-6)
        /// </summary>
        public static IRange<T> ChangeStart<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            a.start = point;
            return a;
        }

        /// <summary>
        /// new equal Range
        /// </summary>
        public static Range<T> Clone<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            if (a == null) return null;
            return new Range<T> { start = a.start, end = a.end, isIncludeEnding = a.isIncludeEnding };
        }

        public static IRange<T> Assign<T>(this IRange<T> self, IRange<T> from)
            where T : IComparable<T>
        {
            self.start = from.start;
            self.end = from.end;
            self.isIncludeEnding = from.isIncludeEnding;
            return self;
        }

        /// <summary>
        /// (1-6).TrimStart(3) == (3-6)
        /// |
        /// (1-6).TrimStart(0) == (1-6)
        /// </summary>
        public static IRange<T> TrimStart<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (a.InRange(point))
                return a.ChangeStart(point);
            return a;
        }

        /// <summary>
        /// (1-6).TrimEnd(3) == (1-3)
        /// |
        /// (1-2).TrimEnd(7) == (1-2)
        /// </summary>
        public static IRange<T> TrimEnd<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (a.InRange(point))
                return a.ChangeEnd(point);
            return a;
        }

        /// <summary>
        /// (1-6).MaxStart(3) == (3-6)
        /// |
        /// (1-2).MaxStart(7) == (7-2)
        /// |
        /// (2-4).MaxStart(1) == (2-4)
        /// </summary>
        public static IRange<T> MaxStart<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (a.start.CompareTo(point) < 0)
                return a.ChangeStart(point);
            return a;
        }

        public static IRange<T> MinStart<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (a.start.CompareTo(point) > 0)
                return a.ChangeStart(point);
            return a;
        }

        public static IRange<T> MinEnd<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (a.end.CompareTo(point) > 0)
                return a.ChangeEnd(point);
            return a;
        }

        public static IRange<T> MaxEnd<T>(this IRange<T> a, T point)
            where T : IComparable<T>
        {
            if (a.end.CompareTo(point) < 0)
                return a.ChangeEnd(point);
            return a;
        }

        /// <summary>
        /// (1-6).MinMax(3,4) == (3-4)
        /// |
        /// (2-5).MinMax(1,8) == (2-5)
        /// |
        /// (2-5).MinMax(1, 3) == (2-3)
        /// </summary>
        public static IRange<T> MinMax<T>(this IRange<T> a, T min, T max)
            where T : IComparable<T>
        {
            return a
                .MaxStart(min).MinStart(max)
                .MinEnd(max).MaxEnd(min);
        }


        public static Nullable<T> getStartNullable<T>(this IRange<T> a)
            where T : struct, IComparable<T>
        {
            if (a == null) return null;
            return a.start;
        }

        public static Nullable<T> getEndNullable<T>(this IRange<T> a)
            where T : struct, IComparable<T>
        {
            if (a == null) return null;
            return a.end;
        }

        public static bool IsNullOrEmpty<T>(this IRange<T> a)
            where T : IComparable<T>
        {
            if (a == null) return true;
            return IsPoint(a) && a.start.CompareTo(default(T)) == 0;
        }
    }

}
