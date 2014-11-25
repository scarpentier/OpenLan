using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace openlan.web.Migrations
{
    public partial class Initial : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.CreateTable("CartItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ProductId = c.Int(nullable: false)
                    })
                .PrimaryKey("PK_CartItems", t => t.Id);
            
            migrationBuilder.CreateTable("Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false)
                    })
                .PrimaryKey("PK_Orders", t => t.Id);
            
            migrationBuilder.CreateTable("OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false)
                    })
                .PrimaryKey("PK_OrderDetails", t => t.Id);
            
            migrationBuilder.CreateTable("Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTime(nullable: false),
                        Description = c.String(),
                        Name = c.String(),
                        PictureUrl = c.String(),
                        Price = c.Decimal(nullable: false)
                    })
                .PrimaryKey("PK_Products", t => t.Id);
            
            migrationBuilder.CreateTable("Seats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PosX = c.Int(nullable: false),
                        PosY = c.Int(nullable: false),
                        Status = c.Int(nullable: false)
                    })
                .PrimaryKey("PK_Seats", t => t.Id);
            
            migrationBuilder.CreateTable("Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PictureUrl = c.String(),
                        Tagline = c.String(),
                        Token = c.String(),
                        Url = c.String()
                    })
                .PrimaryKey("PK_Teams", t => t.TeamId);
            
            migrationBuilder.CreateTable("Tickets",
                c => new
                    {
                        TicketId = c.Int(nullable: false, identity: true)
                    })
                .PrimaryKey("PK_Tickets", t => t.TicketId);
            
            migrationBuilder.CreateTable("Tournament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Name = c.String(),
                        Picture = c.String(),
                        Url = c.String()
                    })
                .PrimaryKey("PK_Tournament", t => t.Id);
                       
            migrationBuilder.AddForeignKey("OrderDetails", "FK_OrderDetails_Orders_OrderId", new[] { "OrderId" }, "Orders", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("OrderDetails", "FK_OrderDetails_Products_ProductId", new[] { "ProductId" }, "Products", new[] { "Id" }, cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.DropTable("CartItems");
            
            migrationBuilder.DropTable("Orders");
            
            migrationBuilder.DropTable("OrderDetails");
            
            migrationBuilder.DropTable("Products");
            
            migrationBuilder.DropTable("Seats");
            
            migrationBuilder.DropTable("Teams");
            
            migrationBuilder.DropTable("Tickets");
            
            migrationBuilder.DropTable("Tournament");
        }
    }
}