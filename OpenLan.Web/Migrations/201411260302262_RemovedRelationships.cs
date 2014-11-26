using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace OpenLan.Web.Migrations
{
    public partial class RemovedRelationships : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("AspNetUsers", "FK_AspNetUsers_Teams_TeamId");
            
            migrationBuilder.DropForeignKey("CartItems", "FK_CartItems_Products_ProductId");
            
            migrationBuilder.DropForeignKey("OrderDetails", "FK_OrderDetails_Orders_OrderId");
            
            migrationBuilder.DropForeignKey("OrderDetails", "FK_OrderDetails_Products_ProductId");
            
            migrationBuilder.DropForeignKey("Seats", "FK_Seats_Tickets_Id");
            
            migrationBuilder.DropForeignKey("TeamTournaments", "FK_TeamTournaments_Teams_TeamId");
            
            migrationBuilder.DropForeignKey("TeamTournaments", "FK_TeamTournaments_Tournaments_TournamentId");
            
            migrationBuilder.DropForeignKey("Tickets", "FK_Tickets_Orders_OrderId");
            
            migrationBuilder.AlterColumn("AspNetUsers", "TeamId", c => c.Int());
            
            migrationBuilder.AlterColumn("Tickets", "SeatId", c => c.Int());
            
            migrationBuilder.AddColumn("Teams", "OwnerUserId", c => c.Int());
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Teams", "OwnerUserId");
            
            migrationBuilder.AlterColumn("AspNetUsers", "TeamId", c => c.Int(nullable: false));
            
            migrationBuilder.AlterColumn("Tickets", "SeatId", c => c.Int(nullable: false));
            
            migrationBuilder.AddForeignKey("AspNetUsers", "FK_AspNetUsers_Teams_TeamId", new[] { "TeamId" }, "Teams", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("CartItems", "FK_CartItems_Products_ProductId", new[] { "ProductId" }, "Products", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("OrderDetails", "FK_OrderDetails_Orders_OrderId", new[] { "OrderId" }, "Orders", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("OrderDetails", "FK_OrderDetails_Products_ProductId", new[] { "ProductId" }, "Products", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("Seats", "FK_Seats_Tickets_Id", new[] { "Id" }, "Tickets", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("TeamTournaments", "FK_TeamTournaments_Teams_TeamId", new[] { "TeamId" }, "Teams", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("TeamTournaments", "FK_TeamTournaments_Tournaments_TournamentId", new[] { "TournamentId" }, "Tournaments", new[] { "Id" }, cascadeDelete: false);
            
            migrationBuilder.AddForeignKey("Tickets", "FK_Tickets_Orders_OrderId", new[] { "OrderId" }, "Orders", new[] { "Id" }, cascadeDelete: false);
        }
    }
}