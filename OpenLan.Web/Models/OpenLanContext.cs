using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using System;

namespace OpenLan.Web.Models
{
    public class OpenLanContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Team> Teams { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptions options)
        {
            options.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
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
            builder.Entity<Post>().ForRelational().Table("Posts");
            builder.Entity<ShoppingCart>().ForRelational().Table("ShoppingCarts");

            // Configure relations
            builder.Entity<Team>()
                .HasMany(x => x.Members)
                .WithOne(x => x.Team)
                .ForeignKey(x => x.TeamId)
                .Required(false);

            builder.Entity<Order>().HasMany<OrderDetail>(x => x.OrderDetails);
            builder.Entity<Product>().HasMany<OrderDetail>(x => x.OrderDetails);
            builder.Entity<Ticket>().HasOne<Seat>(x => x.Seat);
            builder.Entity<Tournament>().HasMany<TeamTournament>(x => x.Teams);
            builder.Entity<Team>().HasMany<TeamTournament>(x => x.Tournaments);
            builder.Entity<ApplicationUser>().HasMany<Ticket>(x => x.Tickets);
            builder.Entity<ApplicationUser>().HasMany<Order>(x => x.Orders);
            builder.Entity<CartItem>().HasOne<Product>(x => x.Product);

            // Configure delete cascade
            // TODO: Not yet implemented in EF7. See https://github.com/aspnet/EntityFramework/issues/333

            // Configure nullable
            ////builder.Entity<ApplicationUser>().Property(x => x.Team).Required(false);
            ////builder.Entity<Team>().Property(x => x.OwnerUser).Required(false);
            ////builder.Entity<Ticket>().Property(x => x.Seat).Required(false);

            base.OnModelCreating(builder);
        }
    }
}