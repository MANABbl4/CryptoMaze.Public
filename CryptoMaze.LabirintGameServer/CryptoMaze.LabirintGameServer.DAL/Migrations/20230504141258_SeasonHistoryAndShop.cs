using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeasonHistoryAndShop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeasonHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeasonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonHistory_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonHistory_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ShopProposals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyItemId = table.Column<int>(type: "int", nullable: false),
                    BuyItemAmount = table.Column<int>(type: "int", nullable: false),
                    SellItemId = table.Column<int>(type: "int", nullable: false),
                    SellItemAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopProposals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShopProposals_Items_BuyItemId",
                        column: x => x.BuyItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShopProposals_Items_SellItemId",
                        column: x => x.SellItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeasonHistory_SeasonId",
                table: "SeasonHistory",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonHistory_UserId",
                table: "SeasonHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProposals_BuyItemId",
                table: "ShopProposals",
                column: "BuyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopProposals_SellItemId",
                table: "ShopProposals",
                column: "SellItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeasonHistory");

            migrationBuilder.DropTable(
                name: "ShopProposals");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
