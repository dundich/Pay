namespace Ait.Auth.Api.Migrations
{
    using Ait.Auth.Api.Entities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;// false;
        }

        protected override void Seed(AuthContext context)
        {
            if (context.Clients.Count() > 0)
            {
                return;
            }

            context.Clients.AddRange(BuildClientsList());

            fillUsers(context);
            context.SaveChanges();
        }

        private static void fillUsers(AuthContext context)
        {
            if (!context.Users.Any())
            {
                // Add missing roles
                //var role = roleManager.FindByName("Admin");
                var role = context.Roles.FirstOrDefault(c => c.Name == "Admin");

                if (role == null)
                {
                    role = new IdentityRole("Admin");

                    context.Roles.Add(role);
                    //roleManager.Create(role);

                    //var newUser = new IdentityUser()
                    //{
                    //    UserName = "admin",
                    //    Email = "xxx@xxx.net",
                    //    PasswordHash = "master"
                    //};

                    //newUser.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = newUser.Id });

                    //context.Users.Add(newUser);
                }
            }
        }

        private static List<Client> BuildClientsList()
        {

            List<Client> ClientsList = new List<Client>
            {
                new Client
                {
                    Id = "ngAit",
                    Secret= Helper.GetHash("abc@123"),
                    Name="AngularJS front-end Application",
                    ApplicationType =  Models.ApplicationTypes.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200,
                    AllowedOrigin = "http://localhost"
                },
                new Client
                {
                    Id = "consoleApp",
                    Secret=Helper.GetHash("123@abc"),
                    Name="Console Application",
                    ApplicationType =Models.ApplicationTypes.NativeConfidential,
                    Active = true,
                    RefreshTokenLifeTime = 14400,
                    AllowedOrigin = "*"
                }
            };

            return ClientsList;
        }
    }
}
