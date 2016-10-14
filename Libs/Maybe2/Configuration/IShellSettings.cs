using Maybe2.Classes;
using System.Collections.Generic;

namespace Maybe2.Configuration
{
    public interface IShellSettings
    {
        /// <summary>
        /// load configuration
        /// </summary>        
        IDictionary<string, string> GetSettings();
        /// <summary>
        /// reset cache for reload
        /// </summary>
        void Reset();
    }



    public class ShellSettings : IShellSettings
    {
        readonly LazyCache<IDictionary<string, string>> cache;

        protected readonly ISettingsProvider provider;

        public ShellSettings(ISettingsProvider provider)
        {
            this.provider = provider;

            cache = new LazyCache<IDictionary<string, string>>(() =>
                provider.LoadSettings());
        }

        public virtual IDictionary<string, string> GetSettings()
        {
            return cache.Value;
        }

        public virtual void Reset()
        {
            cache.Reset();
        }
    }
}
