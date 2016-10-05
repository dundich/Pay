using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maybe2
{
    //IPAddressExtensions

    public static class FuncUtils
    {
        public static Func<T> One<T>(this Func<T> f)
        {
            var lz = new Lazy<T>(f);
            return () => lz.Value;
        }

        //public static Func<R> One<T, R>(this Func<T, R> f, T input)
        //{
        //    var lz = new Lazy<T>(f);
        //    return () => lz.Value;
        //}

        public static Func<T> Wrap<T>(this Func<T> f, Func<Func<T>, T> wrap)
        {
            return () => wrap(f);
        }

        public static Func<R> Next<T, R>(this Func<T> input, Func<T, R> f)
        {
            return () => f(input());
        }

        public static Func<T> If<T>(this Func<bool> fcond, Func<T> fthen, Func<T> felse)
        {
            return () => fcond() ? fthen() : felse();
        }

        public static bool TryExec(Action action, Action<Exception> logerror = null)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                logerror?.Invoke(ex);//Log.Error(ex.Message, ex);
            }
            return false;
        }

        public static T TryExec<T>(Func<T> func)
        {
            return TryExec(func, default(T));
        }

        public static T TryExec<T>(Func<T> func, T defaultValue, Action<Exception> logerror = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                logerror?.Invoke(ex);// Log.Error(ex.Message, ex);
            }
            return default(T);
        }

        public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
        {
            var last = 0;
            return arg =>
            {
                var current = Interlocked.Increment(ref last);
                Task.Delay(milliseconds).ContinueWith(task =>
                {
                    if (current == last) func(arg);
                    task.Dispose();
                });
            };
        }
    }
}
