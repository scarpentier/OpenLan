using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.SqlServer;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;

namespace OpenLan.Web.Models
{
    public class SampleData
    {
        const string defaultAdminUserNameConfigKey = "DefaultAdminUserName";
        const string defaultAdminPasswordConfigKey = "DefaultAdminPassword";

        public static async Task InitializeOpenLanDatabaseAsync(IServiceProvider serviceProvider, bool createUsers = true)
        {
            using (var db = serviceProvider.GetService<OpenLanContext>())
            {
                var sqlServerDatabase = db.Database as SqlServerDatabase;
                if (sqlServerDatabase != null)
                {
                    if (await sqlServerDatabase.EnsureCreatedAsync())
                    {
                        await InsertTestData(serviceProvider);
                        if (createUsers)
                        {
                            await CreateAdminUser(serviceProvider);
                        }
                    }
                }
                else
                {
                    await InsertTestData(serviceProvider);
                    if (createUsers)
                    {
                        await CreateAdminUser(serviceProvider);
                    }
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
                    Id = "lanbyoc",
                    Name = "Serious Lan BYOC",
                    Price = 80,
                    Category = "Ticket",
                    PictureUrl = "http://i.imgur.com/nX4zxx1.png",
                    Description = "Secure your place at the upcomming Serious Lan by buying this ticket"
                });
                db.Products.Add(new Product
                {
                    Id = "lanbyocvip",
                    Name = "Serious Lan BYOC VIP",
                    Price = 130,
                    Category = "Ticket",
                    PictureUrl = "http://i.imgur.com/ixZkjT4.png",
                    Description = "Secure your place at the upcomming Serious Lan by buying this ticket. You'll have the VIP treatment which includes extra space for your dual monitor setup and a backrub."
                });
                db.Products.Add(new Product
                {
                    Id = "lanconsole",
                    Name = "Serious Lan Console",
                    Price = 20,
                    Category = "Ticket",
                    PictureUrl = "http://i.imgur.com/nX4zxx1.png",
                    Description = "Not part of the #pcmasterrace? It's ok, nobody is perfect"
                });
                db.Products.Add(new Product
                {
                    Id = "lanvisitor",
                    Name = "Serious Lan Visitor",
                    Price = 10,
                    Category = "Ticket",
                    PictureUrl = "http://i.imgur.com/nX4zxx1.png",
                    Description = "Be part of something without commitment! You'll be able to stay on-site for the whole duration of the event. Don't forget to sleep and shower!"
                });
                db.Products.Add(new Product
                {
                    Id = "serioustshirt",
                    Name = "Serious Groupie T-Shirt",
                    Price = 20,
                    Category = "Product",
                    PictureUrl = "http://i.imgur.com/khDSMd3.jpg",
                    Description = "Sam himself wore this t-shirt. Now, we can't all be killing hordes of rabid aliens with a giant cannonball but at least you can wear this t-shirt"
                });
                db.Products.Add(new Product
                {
                    Id = "seriousmousepad",
                    Name = "Serious Mousepad",
                    Price = 15,
                    Category = "Product",
                    PictureUrl = "http://i.imgur.com/OnnN2rd.jpg",
                    Description = "Like lube for your mouse, it's going to be an easier spread."
                });
                db.Products.Add(new Product
                {
                    Id = "seriousmousepadplacebo",
                    Name = "Serious Mousepad Special Turbo Placebo Edition",
                    Price = 80,
                    Category = "Product",
                    PictureUrl = "http://i.imgur.com/OnnN2rd.jpg",
                    Description = "At more than 5 times the price of the standard one, you can be sure there's something special about it to justify this outrageous price!"
                });

                await db.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Creates a store manager user who can manage the inventory.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var configuration = new Configuration()
                        .AddJsonFile("config.json")
                        .AddEnvironmentVariables();

            //const string adminRole = "Administrator";

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            // TODO: Identity SQL does not support roles yet
            //var roleManager = serviceProvider.GetService<ApplicationRoleManager>();
            //if (!await roleManager.RoleExistsAsync(adminRole))
            //{
            //    await roleManager.CreateAsync(new IdentityRole(adminRole));
            //}

            var user = await userManager.FindByNameAsync(configuration.Get<string>("defaultAdminUserName"));
            if (user == null)
            {
                user = new ApplicationUser { UserName = configuration.Get<string>("defaultAdminUserName") };
                await userManager.CreateAsync(user, configuration.Get<string>("defaultAdminPassword"));
                
                // TODO: Wait for EF to support this
                ////await userManager.AddToRoleAsync(user, adminRole);

                await userManager.AddClaimAsync(user, new Claim("ManageProducts", "Allowed"));
                await userManager.AddClaimAsync(user, new Claim("ManageTournaments", "Allowed"));
                await userManager.AddClaimAsync(user, new Claim("ManageTeams", "Allowed"));
            }
        }

        // TODO [EF] This may be replaced by a first class mechanism in EF
        private static async Task AddOrUpdateAsync<TEntity>(
            IServiceProvider serviceProvider,
            Func<TEntity, object> propertyToMatch, IEnumerable<TEntity> entities)
            where TEntity : class
        {
            // Query in a separate context so that we can attach existing entities as modified
            List<TEntity> existingData;
            using (var db = serviceProvider.GetService<OpenLanContext>())
            {
                existingData = db.Set<TEntity>().ToList();
            }

            using (var db = serviceProvider.GetService<OpenLanContext>())
            {
                foreach (var item in entities)
                {
                    db.Entry(item).SetState(existingData.Any(g => propertyToMatch(g).Equals(propertyToMatch(item)))
                        ? EntityState.Modified
                        : EntityState.Added);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}