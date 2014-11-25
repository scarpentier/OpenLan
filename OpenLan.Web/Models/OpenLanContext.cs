using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using System;

namespace OpenLan.Web.Models
{
    public class OpenLanContext : IdentityDbContext<ApplicationUser>
    {
        ////private static bool _created = false;

        public OpenLanContext()
        {
            ////// Create the database and schema if it doesn't exist
            ////// This is a temporary workaround to create database until Entity Framework database migrations 
            ////// are supported in ASP.NET 5
            ////if (!_created)
            ////{
            ////    Database.AsRelational().ApplyMigrations();
            ////    _created = true;
            ////}
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptions options)
        {
            options.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // Configure pluralization
            builder.Entity<CartItem>().ForRelational().Table("CartItems");
            builder.Entity<Order>().ForRelational().Table("Orders");
            builder.Entity<OrderDetail>().ForRelational().Table("OrderDetails");
            builder.Entity<Product>().ForRelational().Table("Products");
            builder.Entity<Seat>().ForRelational().Table("Seats");
            builder.Entity<Team>().ForRelational().Table("Teams");
            builder.Entity<Ticket>().ForRelational().Table("Tickets");
            builder.Entity<Tournament>().ForRelational().Table("Tournaments");
            builder.Entity<TeamTournament>().ForRelational().Table("TeamTournaments");

            // TODO: Remove this once convention-based relations work again
            builder.Entity<Order>().OneToMany(o => o.OrderDetails, od => od.Order);
            builder.Entity<Product>().OneToMany(p => p.OrderDetails, od => od.Product);
            builder.Entity<Team>().OneToMany(t => t.Members, m => m.Team);
            builder.Entity<Ticket>().OneToOne(t => t.Seat, s => s.Ticket);
            builder.Entity<Order>().OneToMany(o => o.Tickets, t => t.Order);
            builder.Entity<Team>().OneToMany(t => t.Tournaments, t => t.Team);
            builder.Entity<Tournament>().OneToMany(t => t.Teams, t => t.Tournament);
            ////builder.Entity<ApplicationUser>().OneToMany(a => a.Tickets, t => t.User);
            ////builder.Entity<ApplicationUser>().OneToMany(a => a.Orders, o => o.User);
        }
    }
}