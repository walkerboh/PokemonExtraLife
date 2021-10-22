using Microsoft.EntityFrameworkCore.Migrations;

namespace PokemonExtraLifeApiCore.Migrations
{
    public partial class DonorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DonorIdentifier",
                schema: "public",
                table: "Donations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DonorIdentifier",
                schema: "public",
                table: "Donations");
        }
    }
}
