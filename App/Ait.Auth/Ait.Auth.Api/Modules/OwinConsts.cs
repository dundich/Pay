using Maybe2;
using System.Collections.Generic;

namespace Ait.Auth.Api.Modules
{
    public static class OwinConsts
    {
        public const string TENAT = "ait:tenat";
        public const string AuthRepository = "ait:AuthRepository";


        public static string GetTenat(this IDictionary<string, object> env)
        {
            return env.GetOrDefault(TENAT).NoNull(c => c.ToString()) ?? "";
        }
    }
}