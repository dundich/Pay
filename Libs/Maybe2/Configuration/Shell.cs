using Maybe2.Classes;
using System;
using System.Collections.Generic;

namespace Maybe2.Configuration
{
    public class Shell : IShell
    {

        private readonly LazyCache<DynamicDictionary<string>> _config;

        public Shell(string tenat = null)
        {
            Tenat = Normalize(tenat);
            Provider = CreateProvider(tenat);
            _settings = new ShellSettings(Provider);
            _config = new LazyCache<DynamicDictionary<string>>(() => new DynamicDictionary<string>(_settings.GetSettings()));
        }


        public virtual IShell CreateChild(string tenat)
        {
            return new Shell(tenat);
        }

        public static Func<string, ISettingsProvider> CreateProvider = (string tenat) => SettingsProvider.CreateProvider(Normalize(tenat));

        public static string Normalize(string tenat)
        {
            return (tenat ?? "").ToUpper();
        }

        public virtual string DB_KEY => ShellConsts.DB_KEY;

        public string Tenat { get; private set; }

        /// <summary>
        /// Коннекшен считываем из настроек App_Data/Settings.txt
        /// </summary>
        public string ConnectionString => GetSettings().GetOrDefault(DB_KEY) ?? DB_KEY;

        public ISettingsProvider Provider { get; private set; }

        private readonly IShellSettings _settings;

        public virtual void Reset()
        {
            _config.Reset();
            _settings.Reset();            
        }

        public IDictionary<string, string> GetSettings()
        {
            return _settings.GetSettings();
        }

        public DynamicDictionary<string> Config => _config.Value;
    }
}