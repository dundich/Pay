namespace Ait.Pay.IContract
{
    public static class PayHelper
    {
        //public static T SetPropValue<T>(this T p, string key, string value)
        //    where T : PayIdValue
        //{
        //    if (p == null) return null;

        //    if (p._extProps == null)
        //        p._extProps = new ExtProps { };
        //    p._extProps[key] = value;
        //    return p;
        //}

        //public static string GetPropValue(this PayIdValue p, string key)
        //{
        //    if (p == null || p._extProps == null) return null;
        //    string value;
        //    p._extProps.TryGetValue(key, out value);
        //    return value;
        //}

        public static PayIdValue PayValue(this string key, string value)
        {
            return new PayIdValue { Id = key, Value = value };
        }

    }
}
