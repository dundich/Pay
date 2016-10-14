using Ait.Auth.Api.Migrations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Ait.Auth.Api.Controllers
{

    [RoutePrefix("api/Setup")]
    public class SetupController : ApiController
    {
        public IHttpActionResult Get(string id)
        {
            var rep = Request.GetOwinContext().GetShell();

            var child = rep.CreateShell(id);

            var provider = child.Provider;


            //new MigrateDatabaseToLatestVersion<AuthContext>( true, new Configuration( )

            //var migr = new MigrateDatabaseToLatestVersion<AuthContext, Ait.Auth.Api.Migrations.Configuration>(child.ConnectionString);

            //new ArchiveMigratorDb().Update(targetMigration);
            //Database.SetInitializer(migr);


            var database = this.dataContext.Database;
            var migrationConfiguration = new Configuration();            

            migrationConfiguration.TargetDatabase = new DbConnectionInfo(database.Connection.ConnectionString, "System.Data.SqlClient");

            new MigrateDatabaseToLatestVersion<AuthContext, Configuration>(true, migrationConfiguration);

            var migrator = new DbMigrator(migrationConfiguration);
            migrator.Update();


            //System.Data.Entity.Infrastructure.LocalDbConnectionFactory

            //new AddClientsAndRefreshTokenTables().Up();
            //child.CreateAuthContext().Database.Connection.

            child.CreateAuthContext().Database.Initialize(true);


            //migr.InitializeDatabase()
            var cl = child.AuthRepository.FindClient("admin");


            var d = provider.LoadSettings();

            //provider.SaveSettings()
            //sett.SaveSettings("dd".PairWith("!!!"), "dd".PairWith("@!!"));            

            return Json(d);
        }
    }
}