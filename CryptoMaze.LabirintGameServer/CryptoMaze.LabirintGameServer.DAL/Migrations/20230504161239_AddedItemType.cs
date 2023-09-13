using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedItemType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Items");
        }
    }
}
