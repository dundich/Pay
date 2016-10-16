using Maybe2;
using Maybe2.Classes;
using Maybe2.Configuration;
using System.Collections.Generic;

namespace Ait.Auth.Api
{
    public class Shell : IShell
    {

        public Shell(string tenat = null)
        {
            Tenat = Normalize(tenat);
            _rep = new LazyCache<IAuthRepository>(() => new AuthRepository(CreateAuthContext));
            Provider = CreateProvider(tenat);
            _settings = new ShellSettings(Provider);
        }


        public IShell CreateShell(string tenat)
        {
            return new Shell(tenat);
        }

        static ISettingsProvider CreateProvider(string tenat)
        {
            return SettingsProvider.CreateProvider(Normalize(tenat));
        }

        public static string Normalize(string tenat)
        {
            return (tenat ?? "").ToUpper();
        }

        public virtual string DB_KEY => "Db";

        public string Tenat { get; private set; }

        /// <summary>
        /// Коннекшен считываем из настроек App_Data/Settings.txt
        /// </summary>
        public string ConnectionString => this[DB_KEY] ?? DB_KEY;


        internal AuthContext CreateAuthContext()
        {
            return new AuthContext(ConnectionString);
        }

        private LazyCache<IAuthRepository> _rep = null;

        public IAuthRepository AuthRepository => _rep.Value;

        public ISettingsProvider Provider { get; private set; }

        private readonly IShellSettings _settings;

        public virtual void Reset()
        {
            _settings.Reset();
            _rep.Reset();
        }

        public IDictionary<string, string> GetSettings()
        {
            return _settings.GetSettings();
        }

        public string this[string key]
        {
            get
            {
                return GetSettings().GetOrDefault(key);
            }
        }
    }
}