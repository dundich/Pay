using Maybe2;
using Microsoft.Owin;
using System.Collections.Generic;

namespace Ait.Auth.Api
{
    public static class OwinConsts
    {
        public const string TENAT = "ait:tenat";
        public const string SHELL = "ait:shell";

        public static string GetTenat(this IDictionary<string, object> env)
        {
            return env.GetOrDefault(TENAT).NoNull(c => c.ToString()) ?? "";
        }

        public static IShell GetShell(this IOwinContext ctx)
        {
            return ctx.Get<IShell>(OwinConsts.SHELL);
        }

        public static IAuthRepository GetAuthRep(this IOwinContext ctx)
        {
            return ctx.Get<Shell>(OwinConsts.SHELL).AuthRepository;
        }

    }
}