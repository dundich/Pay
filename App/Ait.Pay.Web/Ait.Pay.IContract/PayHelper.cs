using System;

namespace Ait.Pay.IContract
{



    public static class PayHelper
    {
        public static PayIdValue SetPropValue(this PayIdValue p, string key, string value)
        {
            if (p == null) return null;

            if (p._extProps == null)
                p._extProps = new ExtProps { };
            p._extProps[key] = value;
            return p;
        }

        public static string GetPropValue(this PayIdValue p, string key)
        {
            if (p == null || p._extProps == null) return null;
            string value;
            p._extProps.TryGetValue(key, out value);
            return value;
        }

        public static PayIdValue PayValue(this string key, string value)
        {
            return new PayIdValue { Id = key, Value = value };
        }




        public static IPayDoctor GetFakePayDoctorService()
        {
            return new FakePayDoctor();
        }


        public static IPayResearch GetFakePayResearchService()
        {
            return new FakePayResearch();
        }

        public static IPayIdent GetFakePayIdentService()
        {
            return new FakeIdent();
        }

    }
}
