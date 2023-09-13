using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class LabirintOrderIdUniqueKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labirints_GameId",
                table: "Labirints");

            migrationBuilder.AlterColumn<int>(
                name: "LabirintOrderId",
                table: "Labirints",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Labirints_GameId_LabirintOrderId",
                table: "Labirints",
                columns: new[] { "GameId", "LabirintOrderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labirints_GameId_LabirintOrderId",
                table: "Labirints");

            migrationBuilder.AlterColumn<int>(
                name: "LabirintOrderId",
                table: "Labirints",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Labirints_GameId",
                table: "Labirints",
                column: "GameId");
        }
    }
}
