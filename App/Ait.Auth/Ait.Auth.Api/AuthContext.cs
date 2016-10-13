using Ait.Auth.Api.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Ait.Auth.Api
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        static AuthContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Ait.Auth.Api.Migrations.Configuration>());
        }


        public AuthContext() : base("AuthContext")
        { }

        public AuthContext(string connstr)
            : base(connstr)//Shell.GetConnectionString()
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}