using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using OpenLan.Web.Models;
using System;

namespace openlan.web.Migrations
{
    [ContextType(typeof(OpenLanContext))]
    public class OpenLanContextModelSnapshot : ModelSnapshot
    {
        public override IModel Model
        {
            get
            {
                var builder = new BasicModelBuilder();
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityRole", b =>
                    {
                        b.Property<string>("Id");
                        b.Property<string>("Name");
                        b.Key("Id");
                        b.ForRelational().Table("AspNetRoles");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityRoleClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.Property<string>("ClaimType");
                        b.Property<string>("ClaimValue");
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<string>("RoleId");
                        b.Key("Id");
                        b.ForRelational().Table("AspNetRoleClaims");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityUserClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.Property<string>("ClaimType");
                        b.Property<string>("ClaimValue");
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<string>("UserId");
                        b.Key("Id");
                        b.ForRelational().Table("AspNetUserClaims");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityUserLogin`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.Property<string>("LoginProvider");
                        b.Property<string>("ProviderDisplayName");
                        b.Property<string>("ProviderKey");
                        b.Property<string>("UserId");
                        b.Key("LoginProvider", "ProviderKey");
                        b.ForRelational().Table("AspNetUserLogins");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityUserRole`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.Property<string>("RoleId");
                        b.Property<string>("UserId");
                        b.Key("UserId", "RoleId");
                        b.ForRelational().Table("AspNetUserRoles");
                    });
                
                builder.Entity("OpenLan.Web.Models.ApplicationUser", b =>
                    {
                        b.Property<int>("AccessFailedCount");
                        b.Property<string>("Email");
                        b.Property<bool>("EmailConfirmed");
                        b.Property<string>("Id");
                        b.Property<bool>("LockoutEnabled");
                        b.Property<DateTimeOffset>("LockoutEnd");
                        b.Property<string>("NormalizedUserName");
                        b.Property<string>("PasswordHash");
                        b.Property<string>("PhoneNumber");
                        b.Property<bool>("PhoneNumberConfirmed");
                        b.Property<string>("SecurityStamp");
                        b.Property<bool>("TwoFactorEnabled");
                        b.Property<string>("UserName");
                        b.Key("Id");
                        b.ForRelational().Table("AspNetUsers");
                    });
                
                builder.Entity("OpenLan.Web.Models.CartItem", b =>
                    {
                        b.Property<string>("CartId");
                        b.Property<int>("Count");
                        b.Property<DateTime>("DateCreated");
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<int>("ProductId");
                        b.Key("Id");
                        b.ForRelational().Table("CartItems");
                    });
                
                builder.Entity("OpenLan.Web.Models.Order", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<DateTime>("OrderDate");
                        b.Property<decimal>("Total");
                        b.Key("Id");
                        b.ForRelational().Table("Orders");
                    });
                
                builder.Entity("OpenLan.Web.Models.OrderDetail", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<int>("OrderId");
                        b.Property<int>("ProductId");
                        b.Property<int>("Quantity");
                        b.Property<decimal>("UnitPrice");
                        b.Key("Id");
                        b.ForRelational().Table("OrderDetails");
                    });
                
                builder.Entity("OpenLan.Web.Models.Product", b =>
                    {
                        b.Property<DateTime>("Created");
                        b.Property<string>("Description");
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<string>("Name");
                        b.Property<string>("PictureUrl");
                        b.Property<decimal>("Price");
                        b.Key("Id");
                        b.ForRelational().Table("Products");
                    });
                
                builder.Entity("OpenLan.Web.Models.Seat", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<string>("Name");
                        b.Property<int>("PosX");
                        b.Property<int>("PosY");
                        b.Property<SeatStatus>("Status");
                        b.Key("Id");
                        b.ForRelational().Table("Seats");
                    });
                
                builder.Entity("OpenLan.Web.Models.Team", b =>
                    {
                        b.Property<string>("Name");
                        b.Property<string>("PictureUrl");
                        b.Property<string>("Tagline");
                        b.Property<int>("TeamId")
                            .GenerateValuesOnAdd();
                        b.Property<string>("Token");
                        b.Property<string>("Url");
                        b.Key("TeamId");
                        b.ForRelational().Table("Teams");
                    });
                
                builder.Entity("OpenLan.Web.Models.Ticket", b =>
                    {
                        b.Property<int>("TicketId")
                            .GenerateValuesOnAdd();
                        b.Key("TicketId");
                        b.ForRelational().Table("Tickets");
                    });
                
                builder.Entity("OpenLan.Web.Models.Tournament", b =>
                    {
                        b.Property<string>("Description");
                        b.Property<int>("Id")
                            .GenerateValuesOnAdd();
                        b.Property<string>("Name");
                        b.Property<string>("Picture");
                        b.Property<string>("Url");
                        b.Key("Id");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityRoleClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.ForeignKey("Microsoft.AspNet.Identity.IdentityRole", "RoleId");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityUserClaim`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.ForeignKey("OpenLan.Web.Models.ApplicationUser", "UserId");
                    });
                
                builder.Entity("Microsoft.AspNet.Identity.IdentityUserLogin`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]", b =>
                    {
                        b.ForeignKey("OpenLan.Web.Models.ApplicationUser", "UserId");
                    });
                
                builder.Entity("OpenLan.Web.Models.OrderDetail", b =>
                    {
                        b.ForeignKey("OpenLan.Web.Models.Order", "OrderId");
                        b.ForeignKey("OpenLan.Web.Models.Product", "ProductId");
                    });
                
                return builder.Model;
            }
        }
    }
}