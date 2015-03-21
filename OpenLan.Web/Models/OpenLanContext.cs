using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using System;

namespace OpenLan.Web.Models
{
    public class OpenLanContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Clan> Clans { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TeamTournament> TeamTournament { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptions options)
        {
            options.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure pluralization
            builder.Entity<CartItem>().ForRelational().Table("CartItems");
            builder.Entity<Order>().ForRelational().Table("Orders");
            builder.Entity<OrderDetail>().ForRelational().Table("OrderDetails");
            builder.Entity<Product>().ForRelational().Table("Products");
            builder.Entity<Seat>().ForRelational().Table("Seats");
            builder.Entity<Clan>().ForRelational().Table("Clans");
            builder.Entity<Ticket>().ForRelational().Table("Tickets");
            builder.Entity<Tournament>().ForRelational().Table("Tournaments");
            builder.Entity<TeamTournament>().ForRelational().Table("TeamTournaments");
            builder.Entity<Post>().ForRelational().Table("Posts");

            // Configure relations
            // Clan, team, tournaments and user
            builder.Entity<Clan>()
                .HasMany(x => x.Members)
                .WithOne(x => x.Clan)
                .ForeignKey(x => x.ClanId)
                .Required(false);

            builder.Entity<Tournament>()
                .HasMany<TeamTournament>(x => x.Teams)
                .WithOne(x => x.Tournament)
                .ForeignKey(x => x.TournamentId);

            builder.Entity<Clan>()
                .HasMany<TeamTournament>(x => x.Tournaments)
                .WithOne(x => x.Clan)
                .ForeignKey(x => x.ClanId);

            builder.Entity<TeamTournament>()
                .HasMany(x => x.AssignedMembers);

            // Cart, cart and order
            builder.Entity<Order>().HasMany<OrderDetail>(x => x.OrderDetails);
            builder.Entity<Product>().HasMany<OrderDetail>(x => x.OrderDetails);
            builder.Entity<ApplicationUser>().HasMany<Order>(x => x.Orders);
            builder.Entity<CartItem>().HasOne<Product>(x => x.Product);

            // User, ticket and seat
            builder.Entity<Ticket>()
                .HasOne<ApplicationUser>(x => x.UserOwner)
                .WithMany(x => x.TicketsOwned)
                .ForeignKey(x => x.UserOwnerId)
                .ReferencedKey(x => x.Id);

            builder.Entity<Ticket>()
                .HasOne<Seat>(x => x.Seat)
                .WithOne(x => x.Ticket)
                .ForeignKey<Ticket>(x => x.SeatId)
                .ReferencedKey<Seat>(x => x.Id)
                .Required(false);

            builder.Entity<Ticket>()
                .HasOne<ApplicationUser>(x => x.UserAssigned)
                .WithOne(x => x.TicketAssigned)
                .ForeignKey<Ticket>(x => x.UserAssignedId)
                .ReferencedKey<ApplicationUser>(x => x.Id)
                .Required(false);

            builder.Entity<Ticket>()
                .HasOne<Order>(x => x.Order)
                .WithMany(x => x.Tickets)
                .ForeignKey(x => x.OrderId)
                .ReferencedKey(x => x.Id);

            // Configure delete cascade
            // TODO: Not yet implemented in EF7. See https://github.com/aspnet/EntityFramework/issues/333

            base.OnModelCreating(builder);
        }
    }
}