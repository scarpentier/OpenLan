using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using System;

namespace OpenLan.Web.Models
{
    public class OpenLanContext : IdentityDbContext<ApplicationUser>
    {
        private static bool _created = false;

        public OpenLanContext()
        {
            // Create the database and schema if it doesn't exist
            // This is a temporary workaround to create database until Entity Framework database migrations 
            // are supported in ASP.NET 5
            if (!_created)
            {
                Database.AsRelational().ApplyMigrations();
                _created = true;
            }
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public Floorplan Floorplan { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
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

            builder.Entity<Product>().Key(p => p.ProductId);
            builder.Entity<Order>().Key(o => o.OrderId);
            builder.Entity<CartItem>().Key(c => c.CartItemId);
            builder.Entity<OrderDetail>().Key(o => o.OrderDetailId);
        }
    }
}