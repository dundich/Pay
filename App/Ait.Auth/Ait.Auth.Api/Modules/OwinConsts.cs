﻿using Maybe2;
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

        public static IAuthShell GetShell(this IOwinContext ctx)
        {
            return ctx.Get<IAuthShell>(OwinConsts.SHELL);
        }

        public static IAuthRepository GetAuthRep(this IOwinContext ctx)
        {
            return ctx.Get<AuthShell>(OwinConsts.SHELL).AuthRepository;
        }

    }
}