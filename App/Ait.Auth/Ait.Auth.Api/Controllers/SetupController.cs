using System;
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
        private static object lck = new { };

        public IHttpActionResult Get(string id)
        {
            var rep = Request.GetOwinContext().GetShell();

            var child = rep.CreateShell(id);

            var connectionInfo = new DbConnectionInfo(child.ConnectionString, "System.Data.SqlClient");

            bool doMigration = true;
            if (Database.Exists(child.ConnectionString))
            {
                var contextInfo = new DbContextInfo(typeof(AuthContext), connectionInfo);
                var context = contextInfo.CreateInstance();
                context.Configuration.LazyLoadingEnabled = true;
                context.Configuration.ProxyCreationEnabled = true;
                //contextInfo.OnModelCreating
                try
                {
                    doMigration = !context.Database.CompatibleWithModel(true);
                }
                catch (NotSupportedException)
                {
                    //if there are no metadata for migration
                    doMigration = true;
                }
            }

            if (doMigration)
            {
                lock (lck)
                {
                    var migrationConfiguration = new Ait.Auth.Api.Migrations.Configuration();
                    migrationConfiguration.TargetDatabase = connectionInfo;
                    migrationConfiguration.AutomaticMigrationsEnabled = true;
                    var migrator = new DbMigrator(migrationConfiguration);
                    migrator.Update();
                }
            }

            //migr.InitializeDatabase()
            //var cl = child.AuthRepository.FindClient("admin");

            var provider = child.Provider;
            var d = provider.LoadSettings();

            return Json(d);
        }
    }
}