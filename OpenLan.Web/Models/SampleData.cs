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
        const string defaultAdminUserNameConfigKey = "DefaultAdminUserName";
        const string defaultAdminPasswordConfigKey = "DefaultAdminPassword";

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
                db.Tournaments.Add(new Tournament
                {
                    Name = "Serious Sam",
                    Tagline = "Nothing too serious about it",
                    PictureUrl = "http://cdn.akamai.steamstatic.com/steam/apps/41014/header.jpg",
                    Url = "http://binarybeast.com/",
                    Description = "Coop, Cathedral, serious difficulty, all guns, infinite ammo, extra monsters, monsters have extra health. Winner is the one who has the most points at the end. Bonus points for making it a drinking game"
                });
                db.Tournaments.Add(new Tournament
                {
                    Name = "Scavenger Hunt",
                    Tagline = "Fight for the second place!",
                    PictureUrl = "http://i.imgur.com/uJEKXh2.png",
                    Url = "https://serioushunt.azurewebsites.net"
                });
                db.Tournaments.Add(new Tournament
                {
                    Name = "Minecraft",
                    Tagline = "Punching wood!",
                    PictureUrl = "http://i.imgur.com/VB04jf2.jpg",
                    Url = "https://minecraft.net/"
                });

                db.Products.Add(new Product
                {
                    Name = "Serious Lan BYOC",
                    Price = 80
                });
                db.Products.Add(new Product
                {
                    Name = "SouthSec Early Bird",
                    Price = 250
                });
                db.Products.Add(new Product
                {
                    Name = "CS Games Participant",
                    Price = 100
                });

                await db.SaveChangesAsync();
            }
        }

        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByNameAsync(configuration.Get<string>(defaultAdminUserNameConfigKey));
            if (user == null)
            {
                user = new ApplicationUser { UserName = configuration.Get<string>(defaultAdminUserNameConfigKey) };
                await userManager.CreateAsync(user, configuration.Get<string>(defaultAdminPasswordConfigKey));
                await userManager.AddClaimAsync(user, new Claim("ManageTournaments", "Allowed"));
                await userManager.AddClaimAsync(user, new Claim("ManageProducts", "Allowed"));
            }
        }
    }
}