using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommandControlServer.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedPorts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Port",
                table: "Bots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Port",
                table: "Bots");
        }
    }
}
