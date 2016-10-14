using Maybe2;
using Maybe2.Classes;
using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    public class Shell : IShell
    {

        public Shell(string tenat = null)
        {
            Tenat = Normalize(tenat);
            _rep = new LazyCache<IAuthRepository>(() => new AuthRepository(CreateAuthContext));
            Provider = CreateProvider(tenat);
            Settings = new ShellSettings(Provider);
        }


        public Shell CreateShell(string tenat)
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
        public string ConnectionString => Settings.GetSettings().GetOrDefault(DB_KEY) ?? DB_KEY;


        internal AuthContext CreateAuthContext()
        {
            return new AuthContext(ConnectionString);
        }

        private LazyCache<IAuthRepository> _rep = null;

        public IAuthRepository AuthRepository => _rep.Value;

        public ISettingsProvider Provider { get; private set; }

        public IShellSettings Settings { get; private set; }

        public virtual void Reset()
        {
            Settings.Reset();
            _rep.Reset();
        }

    }
}