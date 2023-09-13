using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UniqueKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserItems_ItemId",
                table: "UserItems");

            migrationBuilder.DropIndex(
                name: "IX_SeasonHistory_UserId",
                table: "SeasonHistory");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ItemId_UserId",
                table: "UserItems",
                columns: new[] { "ItemId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeasonHistory_Type_Rank",
                table: "SeasonHistory",
                columns: new[] { "Type", "Rank" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeasonHistory_UserId_SeasonId_Type",
                table: "SeasonHistory",
                columns: new[] { "UserId", "SeasonId", "Type" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Type",
                table: "Items",
                column: "Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserItems_ItemId_UserId",
                table: "UserItems");

            migrationBuilder.DropIndex(
                name: "IX_SeasonHistory_Type_Rank",
                table: "SeasonHistory");

            migrationBuilder.DropIndex(
                name: "IX_SeasonHistory_UserId_SeasonId_Type",
                table: "SeasonHistory");

            migrationBuilder.DropIndex(
                name: "IX_Items_Type",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ItemId",
                table: "UserItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonHistory_UserId",
                table: "SeasonHistory",
                column: "UserId");
        }
    }
}
