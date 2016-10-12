using Ait.Auth.Api.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Ait.Auth.Api
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
}