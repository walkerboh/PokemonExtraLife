using Microsoft.EntityFrameworkCore.Migrations;

namespace PokemonExtraLifeApiCore.Migrations
{
    public partial class DonationPrize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrizeId",
                schema: "public",
                table: "Donations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrizeId",
                schema: "public",
                table: "Donations");
        }
    }
}
