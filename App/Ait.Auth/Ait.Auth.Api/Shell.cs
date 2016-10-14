using Maybe2;
using Maybe2.Classes;
using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    public class Shell : ShellSettings
    {

        public Shell(string tenat = null)
            : base(SettingsProvider.CreateProvider(Normalize(tenat), null))
        {
            Tenat = Normalize(tenat);
            _rep = new LazyCache<IAuthRepository>(() => new AuthRepository(CreateAuthContext));
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
        public string ConnectionString => GetSettings().GetOrDefault(DB_KEY) ?? DB_KEY;


        internal AuthContext CreateAuthContext()
        {
            return new AuthContext(ConnectionString);
        }

        private LazyCache<IAuthRepository> _rep = null;

        public IAuthRepository AuthRepository
        {
            get
            {
                return _rep.Value;
            }
        }

        public override void Reset()
        {
            base.Reset();
            _rep.Reset();
        }

    }
}