using Maybe2;
using Microsoft.Owin;
using System.Collections.Generic;

namespace Ait.Infrastructure.Web
{
    public static class OwinHelper
    {
        public const string TENAT = "ait:tenat";
        public const string SHELL = "ait:shell";

        public static string GetTenat(this IDictionary<string, object> env)
        {
            return env.GetOrDefault(TENAT).NoNull(c => c.ToString()) ?? "";
        }

        public static InfraShell GetShell(this IOwinContext ctx)
        {
            return ctx.Get<InfraShell>(SHELL);
        }
    }
}