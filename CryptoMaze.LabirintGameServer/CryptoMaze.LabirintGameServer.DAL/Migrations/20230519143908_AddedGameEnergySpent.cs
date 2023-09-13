using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameEnergySpent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnergySpent",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 11);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergySpent",
                table: "Games");
        }
    }
}
