using Microsoft.EntityFrameworkCore.Migrations;

namespace PokemonExtraLifeApiCore.Migrations
{
    public partial class donationid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DonationId",
                schema: "public",
                table: "Prizes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonationId",
                schema: "public",
                table: "Prizes");
        }
    }
}
