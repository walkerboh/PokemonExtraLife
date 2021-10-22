using Microsoft.EntityFrameworkCore.Migrations;

namespace PokemonExtraLifeApiCore.Migrations
{
    public partial class Claim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Claimed",
                schema: "public",
                table: "TargetPrizes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Claimed",
                schema: "public",
                table: "TargetPrizes");
        }
    }
}
