using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OpenLan.Web.Models
{
    public class OpenLanContext : IdentityDbContext<ApplicationUser>
    {
        public OpenLanContext()
            : base("OpenLanConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public static OpenLanContext Create()
        {
            return new OpenLanContext();
        }

        public new DbSet<ApplicationRole> Roles { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Rename Identity Tables
            ////modelBuilder.Entity<IdentityUser>().ToTable("IdentityUsers").Property(p => p.Id);
            ////modelBuilder.Entity<IdentityUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");

            modelBuilder.Entity<Team>()
                .HasMany(a => a.Members)
                .WithOptional(a => a.Team)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users")
                .Property(p => p.Id)
                .HasColumnName("UserId");

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);

            modelBuilder.Entity<ApplicationRole>()
                .ToTable("Roles")
                .Property(r => r.Id)
                .HasColumnName("RoleId");

            modelBuilder.Entity<ApplicationRole>()
                .HasMany<ApplicationUserRole>((ApplicationRole r) => r.RoleUsers);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId })
                .ToTable("UserRoles");
        }

        public bool Seed(OpenLanContext context)
        {
            bool success = false;

            ApplicationRoleManager roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(context));

            success = this.CreateRole(roleManager, "Admin", "Global Access");
            if (!success) return success;

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };
            IdentityResult result = userManager.Create(user, "admin123");

            success = this.AddUserToRole(userManager, user.Id, "Admin");
            if (!success) return success;

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
            return true;
        }

        public class DropCreateAlwaysInitializer : DropCreateDatabaseAlways<OpenLanContext>
        {
            protected override void Seed(OpenLanContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public bool RoleExists(ApplicationRoleManager roleManager, string name)
        {
            return roleManager.RoleExists(name);
        }

        public bool CreateRole(ApplicationRoleManager _roleManager, string name, string description = "")
        {
            var idResult = _roleManager.Create<ApplicationRole, string>(new ApplicationRole(name, description));
            return idResult.Succeeded;
        }

        public bool AddUserToRole(ApplicationUserManager _userManager, string userId, string roleName)
        {
            var idResult = _userManager.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }

        public void ClearUserRoles(ApplicationUserManager userManager, string userId)
        {
            var user = userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.UserRoles);
            foreach (ApplicationUserRole role in currentRoles)
            {
                userManager.RemoveFromRole(userId, role.Role.Name);
            }
        }


        public void RemoveFromRole(ApplicationUserManager userManager, string userId, string roleName)
        {
            userManager.RemoveFromRole(userId, roleName);
        }

        public void DeleteRole(OpenLanContext context, ApplicationUserManager userManager, string roleId)
        {
            var roleUsers = context.Users.Where(u => u.UserRoles.Any(r => r.RoleId == roleId));
            var role = context.Roles.Find(roleId);

            foreach (var user in roleUsers)
            {
                this.RemoveFromRole(userManager, user.Id, role.Name);
            }
            context.Roles.Remove(role);
            context.SaveChanges();
        }
    }
}