using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Ait.Auth.Api.Modules
{
    public class RepModule
    {

        private readonly AppFunc _next;
        private readonly string _prefix;

        public RepModule(AppFunc next, string prefix)
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

            AuthRepository _rep = null;
            try
            {
                try
                {
                    _rep = new AuthRepository(new AuthContext(Shell.GetConnectionString()));

                    env.Add(OwinConsts.AuthRepository, _rep as IAuthRepository);

                    var cl = _rep.FindClient("admin");

                    //var req = new Microsoft.Owin.OwinRequest(env);

                    //var s = req.Query.Get("tenat") ?? "";

                    //req.Set("ait:tenat", s);

                    //Debug.WriteLine("{0} tenat: {1}", this._prefix, s);
                }
                catch (Exception ex)
                {
                    var tcs = new TaskCompletionSource<object>();
                    tcs.SetException(ex);
                    return tcs.Task;
                }

                return this._next(env);
            }
            finally
            {
                //if (_rep != null)
                //    _rep.Dispose();
            }
        }
    }
}