using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using OpenLan.Web.Models;
using System;

namespace openlan.web.Migrations
{
    [ContextType(typeof(OpenLanContext))]
    public partial class Initial : IMigrationMetadata
    {
        string IMigrationMetadata.MigrationId
        {
            get
            {
                return "201411251546581_Initial";
            }
        }
        
        string IMigrationMetadata.ProductVersion
        {
            get
            {
                return "7.0.0-beta1-11518";
            }
        }
        
        IModel IMigrationMetadata.TargetModel
        {
            get
            {
                var builder = new BasicModelBuilder();
                               
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