using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    NameChanged = table.Column<bool>(type: "bit", nullable: false),
                    Energy = table.Column<int>(type: "int", nullable: false),
                    TonWalletBalance = table.Column<int>(type: "int", nullable: false),
                    BlcWalletBalance = table.Column<int>(type: "int", nullable: false),
                    TonCryptoBlocks = table.Column<int>(type: "int", nullable: false),
                    EthCryptoBlocks = table.Column<int>(type: "int", nullable: false),
                    BtcCryptoBlocks = table.Column<int>(type: "int", nullable: false),
                    CryptoKeyFragmentCount = table.Column<int>(type: "int", nullable: false),
                    CryptoKeyCount = table.Column<int>(type: "int", nullable: false),
                    TimeFreezeCount = table.Column<int>(type: "int", nullable: false),
                    SpeedRocketCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BtcBlocksCollected = table.Column<int>(type: "int", nullable: false),
                    EthBlocksCollected = table.Column<int>(type: "int", nullable: false),
                    TonBlocksCollected = table.Column<int>(type: "int", nullable: false),
                    EnergyCollected = table.Column<int>(type: "int", nullable: false),
                    CryptoKeyFragmentsCollected = table.Column<int>(type: "int", nullable: false),
                    TimeFreezeUsed = table.Column<bool>(type: "bit", nullable: false),
                    CryptoKeyUsed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Labirints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FinishTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeToFinishSeconds = table.Column<int>(type: "int", nullable: false),
                    HasCryptoStorage = table.Column<bool>(type: "bit", nullable: false),
                    CryptoStorageOpened = table.Column<bool>(type: "bit", nullable: false),
                    SpeedRocketUsed = table.Column<bool>(type: "bit", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Labirints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Labirints_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabirintCryptoBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Found = table.Column<bool>(type: "bit", nullable: false),
                    Storage = table.Column<bool>(type: "bit", nullable: false),
                    LabirintId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabirintCryptoBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabirintCryptoBlocks_Labirints_LabirintId",
                        column: x => x.LabirintId,
                        principalTable: "Labirints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabirintCryptoKeyFragments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Found = table.Column<bool>(type: "bit", nullable: false),
                    Storage = table.Column<bool>(type: "bit", nullable: false),
                    LabirintId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabirintCryptoKeyFragments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabirintCryptoKeyFragments_Labirints_LabirintId",
                        column: x => x.LabirintId,
                        principalTable: "Labirints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabirintEnergies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Found = table.Column<bool>(type: "bit", nullable: false),
                    Storage = table.Column<bool>(type: "bit", nullable: false),
                    LabirintId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabirintEnergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabirintEnergies_Labirints_LabirintId",
                        column: x => x.LabirintId,
                        principalTable: "Labirints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_UserId",
                table: "Games",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LabirintCryptoBlocks_LabirintId",
                table: "LabirintCryptoBlocks",
                column: "LabirintId");

            migrationBuilder.CreateIndex(
                name: "IX_LabirintCryptoKeyFragments_LabirintId",
                table: "LabirintCryptoKeyFragments",
                column: "LabirintId");

            migrationBuilder.CreateIndex(
                name: "IX_LabirintEnergies_LabirintId",
                table: "LabirintEnergies",
                column: "LabirintId");

            migrationBuilder.CreateIndex(
                name: "IX_Labirints_GameId",
                table: "Labirints",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabirintCryptoBlocks");

            migrationBuilder.DropTable(
                name: "LabirintCryptoKeyFragments");

            migrationBuilder.DropTable(
                name: "LabirintEnergies");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Labirints");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
