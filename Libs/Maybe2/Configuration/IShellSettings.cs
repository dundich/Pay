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

        public ShellSettings(ISettingsProvider settings)
        {
            cache = new LazyCache<IDictionary<string, string>>(() =>
                settings.LoadSettings());
        }

        public IDictionary<string, string> GetSettings()
        {
            return cache.Value;
        }


        public void Reset()
        {
            cache.Reset();
        }
    }
}
