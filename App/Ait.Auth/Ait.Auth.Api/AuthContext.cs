using Ait.Infrastructure.Api.Entities;
using Maybe2;
using Maybe2.Classes;
using Maybe2.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using IDict = System.Collections.Generic.IDictionary<string, string>;

namespace Ait.Infrastructure.Api
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base(Shell.GetConnectionString())
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }

    /// <summary>
    /// Коннекшен считываем из настроек App_Data/Settings.txt
    /// </summary>
    class Shell
    {
        const string DB_KEY = "Db";

        static LazyCache<IDict> c = new LazyCache<IDict>(() =>
        {
            return SettingsProvider.CreateWebSettings().LoadSettings();
        });

        public static IDict GetSettings()
        {
            return c.Value;
        }

        public static string GetConnectionString()
        {
            return c.Value.GetOrDefault(DB_KEY) ?? DB_KEY;
            //"Server=.;Database=AitAuth2;Trusted_Connection=true;Integrated Security=True";
        }

        /// <summary>
        /// Перечитать коннекшен
        /// </summary>
        public void Reset()
        {
            c.Reset();
        }
    }



}