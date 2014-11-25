using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace openlan.web.Migrations
{
    public partial class TournamentTagline : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn("Tournaments", "Tagline", c => c.String());
            
            migrationBuilder.AddForeignKey("CartItems", "FK_CartItems_Products_ProductId", new[] { "ProductId" }, "Products", new[] { "Id" }, cascadeDelete: false);
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("CartItems", "FK_CartItems_Products_ProductId");
            
            migrationBuilder.DropColumn("Tournaments", "Tagline");
        }
    }
}