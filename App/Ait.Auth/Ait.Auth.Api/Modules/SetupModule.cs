using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Ait.Auth.Api.Modules
{
    public class SetupModule
    {

        private readonly AppFunc _next;
        private readonly string _prefix;

        public SetupModule(AppFunc next, string prefix)
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
                var tenat = env.GetTenat();

                //var _rep = new AuthRepository(() => new AuthContext(Shell.GetConnectionString()));

                if (tenat == "MONIKI")
                {
                    var response = new OwinResponse(env);
                    response.Redirect("/api/setup/" + tenat);

                    //return response.WriteAsync("About page");
                }
                

                //var cl = _rep.FindClient("admin");
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