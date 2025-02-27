using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommandControlServer.Api.Migrations
{
    /// <inheritdoc />
    public partial class PortToDockername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Bots");

            migrationBuilder.AddColumn<string>(
                name: "DockerName",
                table: "Bots",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DockerName",
                table: "Bots");

            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Bots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
