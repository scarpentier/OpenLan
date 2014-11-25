using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.SqlServer;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.ConfigurationModel;

namespace OpenLan.Web.Models
{
    public class SampleData
    {
        const string defaultAdminUserName = "admin";
        const string defaultAdminPassword = "admin123";

        public static async Task InitializeOpenLanDatabaseAsync(IServiceProvider serviceProvider)
        {
            using (var db = serviceProvider.GetService<OpenLanContext>())
            {
                var sqlServerDataStore = db.Configuration.DataStore as SqlServerDataStore;
                if (sqlServerDataStore != null)
                {
                    if (await db.Database.EnsureCreatedAsync())
                    {
                        await InsertTestData(serviceProvider);
                        await CreateAdminUser(serviceProvider);
                    }
                }
                else
                {
                    await InsertTestData(serviceProvider);
                    await CreateAdminUser(serviceProvider);
                }
            }
        }

        private static async Task InsertTestData(IServiceProvider serviceProvider)
        {
            using (var db = serviceProvider.GetService<OpenLanContext>())
            {
                db.Tournaments.Add(new Tournament { Name = "Serious Sam" });
                db.Tournaments.Add(new Tournament { Name = "Scavenger Hunt" });
                await db.SaveChangesAsync();
            }
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(configuration.Get<string>(defaultAdminUserName));
            if (user == null)
            {
                user = new ApplicationUser { UserName = configuration.Get<string>(defaultAdminUserName) };
                await userManager.CreateAsync(user, configuration.Get<string>(defaultAdminPassword));
                await userManager.AddClaimAsync(user, new Claim("ManageUsers", "Allowed"));
            }
        }
    }
}