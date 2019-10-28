using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PokemonExtraLifeApiCore.Migrations
{
    public partial class prizeEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                schema: "public",
                table: "Prizes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                schema: "public",
                table: "Prizes");
        }
    }
}
