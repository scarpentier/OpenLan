using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Model;
using System;

namespace openlan.web.Migrations
{
    public partial class birthdate : Migration
    {
        public override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn("AspNetUsers", "BirthDate", c => c.DateTime());
        }
        
        public override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("AspNetUsers", "BirthDate");
        }
    }
}