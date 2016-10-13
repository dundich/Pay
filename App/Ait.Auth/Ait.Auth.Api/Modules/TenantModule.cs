using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Ait.Auth.Api.Modules
{

    public class TenantModule
    {
        private readonly AppFunc _next;
        private readonly string _prefix;

        public TenantModule(AppFunc next, string prefix)
        {
            if (next == null)
                throw new ArgumentNullException("next");

            if (string.IsNullOrEmpty(prefix))
                throw new ArgumentException("prefix can't be null or empty");

            this._next = next;
            this._prefix = prefix;
        }

        public Task Invoke(IDictionary<string, object> env)
        {
            try
            {
                var req = new Microsoft.Owin.OwinRequest(env);

                var s = req.Query.Get("tenat") ?? "";

                req.Set(OwinConsts.TENAT, s);

                Debug.WriteLine("{0} tenat: {1}", this._prefix, s);
            }
            catch (Exception ex)
            {
                var tcs = new TaskCompletionSource<object>();
                tcs.SetException(ex);
                return tcs.Task;
            }

            return this._next(env);
        }
    }
}