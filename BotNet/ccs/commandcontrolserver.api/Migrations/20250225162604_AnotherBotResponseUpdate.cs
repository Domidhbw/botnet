using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommandControlServer.Api.Migrations
{
    /// <inheritdoc />
    public partial class AnotherBotResponseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Command",
                table: "BotResponses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ResponseContent",
                table: "BotResponses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Command",
                table: "BotResponses");

            migrationBuilder.DropColumn(
                name: "ResponseContent",
                table: "BotResponses");
        }
    }
}
