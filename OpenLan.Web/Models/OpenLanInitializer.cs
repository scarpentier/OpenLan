using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OpenLan.Web.Models
{
    public class OpenLanInitializer : System.Data.Entity.DropCreateDatabaseAlways<OpenLanContext>
    {
        protected override void Seed(OpenLanContext context)
        {
            var idManager = new IdentityManager();

            // Create roles
            var adminRole = idManager.CreateRole("Admin");

            // Create users
            var userAdmin = new ApplicationUser() { UserName = "admin@admin.com", Email = "admin@admin.com" };
            idManager.AddUserToRole(userAdmin.Id, "Admin");

            // Create mock data
            var teams = new List<Team>
                            {
                                new Team() { Name = "Business Team" },
                                new Team() { Name = "LaNe-éTs" },
                                new Team() { Name = "LQGR" },
                                new Team() { Name = "CEH Groupies" },
                                new Team() { Name = "SouthSec" }
                            };
            context.Teams.AddRange(teams);

            context.SaveChanges();
        }
    }
}