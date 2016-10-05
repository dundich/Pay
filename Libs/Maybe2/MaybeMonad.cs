using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Maybe2
{
    [DebuggerStepThrough]
    public static class MaybeMonad
    {
        public static TResult NoNull<TObject, TResult>(this TObject obj, Func<TObject, TResult> accessor, TResult defaultingTo = default(TResult))
        {
            if (ReferenceEquals(obj, null))
                return defaultingTo;
            return accessor(obj);
        }

        public static TResult NoNull<TObject, TResult>(this TObject obj, Func<TObject, TResult> accessor, Func<TResult> defaultingTo)
        {
            if (ReferenceEquals(obj, null))
            {
                if (defaultingTo == null)
                    return default(TResult);
                return defaultingTo();
            }

            return accessor(obj);
        }

        public static IEnumerable<TResult> DefaultForNull<TObject, TResult>(this TObject obj, Func<TObject, IEnumerable<TResult>> accessor, IEnumerable<TResult> defaultingTo = null)
        {
            if (ReferenceEquals(obj, null))
                return defaultingTo ?? Enumerable.Empty<TResult>();
            return accessor(obj);
        }

        public static TResult ExecuteIfNotNull<TType, TResult>(this Func<TType, TResult> func, TType param)
        {
            if (func != null)
            {
                return func(param);
            }
            return default(TResult);
        }


        public static TResult ExecuteIfNotNull<TType1, TType2, TResult>(this Func<TType1, TType2, TResult> func,
                                                                        TType1 param1, TType2 param2)
        {
            if (func != null)
            {
                return func(param1, param2);
            }
            return default(TResult);
        }

        public static void RunUsing<T>(this T disposable, Action<T> runActionThenDispose)
            where T : IDisposable
        {
            using (disposable)
            {
                runActionThenDispose(disposable);
            }
        }


        public static bool IsNullOrEmpty(this Guid? val)
        {
            return val == null || val.Value == Guid.Empty;
        }

        public static bool IsEmpty(this Guid val)
        {
            return val == Guid.Empty;
        }
    }
}
