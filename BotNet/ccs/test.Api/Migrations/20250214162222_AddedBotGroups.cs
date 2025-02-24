using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommandControlServer.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedBotGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BotGroups",
                columns: table => new
                {
                    BotGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotGroups", x => x.BotGroupId);
                });

            migrationBuilder.CreateTable(
                name: "BotBotGroup",
                columns: table => new
                {
                    BotGroupsBotGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    BotsBotId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotBotGroup", x => new { x.BotGroupsBotGroupId, x.BotsBotId });
                    table.ForeignKey(
                        name: "FK_BotBotGroup_BotGroups_BotGroupsBotGroupId",
                        column: x => x.BotGroupsBotGroupId,
                        principalTable: "BotGroups",
                        principalColumn: "BotGroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BotBotGroup_Bots_BotsBotId",
                        column: x => x.BotsBotId,
                        principalTable: "Bots",
                        principalColumn: "BotId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotBotGroup_BotsBotId",
                table: "BotBotGroup",
                column: "BotsBotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotBotGroup");

            migrationBuilder.DropTable(
                name: "BotGroups");
        }
    }
}
