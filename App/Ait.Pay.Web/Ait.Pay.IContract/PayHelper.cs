namespace Ait.Pay.IContract
{
    public static class PayHelper
    {
        public static PayIdValue PayValue(this string key, string value)
        {
            return new PayIdValue { Id = key, Value = value };
        }
    }
}
