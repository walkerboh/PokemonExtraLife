using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace PokemonExtraLifeApiCore.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "DisplayStatus",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CurrentHostId = table.Column<int>(nullable: false),
                    CurrentGame = table.Column<string>(nullable: true),
                    NextGame = table.Column<string>(nullable: true),
                    FollowingGame = table.Column<string>(nullable: true),
                    DonationGoal = table.Column<decimal>(nullable: false),
                    TrackDonations = table.Column<bool>(nullable: false),
                    HealthMultiplier = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Donor = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Gym = table.Column<int>(nullable: true),
                    PrizeId = table.Column<int>(nullable: true),
                    Processed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Giveaways",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PrizeName = table.Column<string>(nullable: true),
                    Contributor = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Gym = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giveaways", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Gym = table.Column<int>(nullable: false),
                    Started = table.Column<bool>(nullable: false),
                    ForceComplete = table.Column<bool>(nullable: false),
                    DurationMinutes = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hosts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Pokemon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pokemon",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    StartingHealth = table.Column<decimal>(nullable: false),
                    Damage = table.Column<decimal>(nullable: false),
                    HealthMultiplier = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prizes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Contributor = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    WiningDonor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Icon = table.Column<string>(nullable: true),
                    Gym = table.Column<int>(nullable: true),
                    Leader = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PokemonOrders",
                schema: "public",
                columns: table => new
                {
                    PokemonId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    TrainerId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    Activated = table.Column<bool>(nullable: false),
                    ForceDone = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonOrders", x => x.PokemonId);
                    table.ForeignKey(
                        name: "FK_PokemonOrders_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "public",
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PokemonOrders_Pokemon_PokemonId",
                        column: x => x.PokemonId,
                        principalSchema: "public",
                        principalTable: "Pokemon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonOrders_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalSchema: "public",
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonOrders_GroupId",
                schema: "public",
                table: "PokemonOrders",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PokemonOrders_TrainerId",
                schema: "public",
                table: "PokemonOrders",
                column: "TrainerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisplayStatus",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Donations",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Giveaways",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Hosts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PokemonOrders",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Prizes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Groups",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Pokemon",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Trainers",
                schema: "public");
        }
    }
}
