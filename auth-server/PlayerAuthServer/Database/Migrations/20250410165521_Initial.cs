using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlayerAuthServer.Database.Migrations
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Losses = table.Column<int>(type: "integer", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "card_collection",
                columns: table => new
                {
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    PlayerGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_collection", x => new { x.PlayerId, x.CardId });
                    table.ForeignKey(
                        name: "FK_card_collection_players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card_collection");

            migrationBuilder.DropTable(
                name: "players");
        }
    }
}
