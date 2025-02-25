using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommandControlServer.Api.Migrations
{
    /// <inheritdoc />
    public partial class BotResponseUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "BotResponses",
                newName: "FilePath");

            migrationBuilder.AddColumn<bool>(
                name: "Success",
                table: "BotResponses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Success",
                table: "BotResponses");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "BotResponses",
                newName: "Data");
        }
    }
}
