using Maybe2.Classes;
using Maybe2.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.Web
{
    public interface IPayConfig
    {
        Task<IDictionary<string, string>> GetSettings();
        void Reset();
    }



    public class PayConfig : IPayConfig
    {
        readonly LazyCache<Task<IDictionary<string, string>>> cache;

        public PayConfig(IShellSettings settings)
        {
            cache = new LazyCache<Task<IDictionary<string, string>>>(() =>
                settings.LoadSettings());
        }

        public async Task<IDictionary<string, string>> GetSettings()
        {
            return await cache.Value;
        }


        public void Reset()
        {
            cache.Reset();
        }
    }

}