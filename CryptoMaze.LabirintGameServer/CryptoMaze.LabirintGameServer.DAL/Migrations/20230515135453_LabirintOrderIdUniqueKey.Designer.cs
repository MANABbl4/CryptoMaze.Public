﻿// <auto-generated />
using System;
using CryptoMaze.LabirintGameServer.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CryptoMaze.LabirintGameServer.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230515135453_LabirintOrderIdUniqueKey")]
    partial class LabirintOrderIdUniqueKey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BtcBlocksCollected")
                        .HasColumnType("int");

                    b.Property<int>("CryptoKeyFragmentsCollected")
                        .HasColumnType("int");

                    b.Property<bool>("CryptoKeyUsed")
                        .HasColumnType("bit");

                    b.Property<int>("EnergyCollected")
                        .HasColumnType("int");

                    b.Property<int>("EthBlocksCollected")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FinishTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("TimeFreezeUsed")
                        .HasColumnType("bit");

                    b.Property<int>("TonBlocksCollected")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Labirint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("CryptoStorageOpened")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FinishTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("HasCryptoStorage")
                        .HasColumnType("bit");

                    b.Property<int>("LabirintOrderId")
                        .HasColumnType("int");

                    b.Property<bool>("SpeedRocketUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TimeToFinishSeconds")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId", "LabirintOrderId")
                        .IsUnique();

                    b.ToTable("Labirints");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.LabirintCryptoBlock", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<bool>("Found")
                        .HasColumnType("bit");

                    b.Property<int>("LabirintId")
                        .HasColumnType("int");

                    b.Property<bool>("Storage")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LabirintId");

                    b.ToTable("LabirintCryptoBlocks");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.LabirintCryptoKeyFragment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<bool>("Found")
                        .HasColumnType("bit");

                    b.Property<int>("LabirintId")
                        .HasColumnType("int");

                    b.Property<bool>("Storage")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("LabirintId");

                    b.ToTable("LabirintCryptoKeyFragments");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.LabirintEnergy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<bool>("Found")
                        .HasColumnType("bit");

                    b.Property<int>("LabirintId")
                        .HasColumnType("int");

                    b.Property<bool>("Storage")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("LabirintId");

                    b.ToTable("LabirintEnergies");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Season", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FinishDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Finished")
                        .HasColumnType("bit");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.SeasonHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.HasIndex("Type", "Rank")
                        .IsUnique();

                    b.HasIndex("UserId", "SeasonId", "Type")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("SeasonHistory");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.ShopProposal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BuyItemAmount")
                        .HasColumnType("int");

                    b.Property<int>("BuyItemId")
                        .HasColumnType("int");

                    b.Property<int>("SellItemAmount")
                        .HasColumnType("int");

                    b.Property<int>("SellItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BuyItemId");

                    b.HasIndex("SellItemId");

                    b.ToTable("ShopProposals");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("NameChanged")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.UserItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemAmount")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ItemId", "UserId")
                        .IsUnique();

                    b.ToTable("UserItems");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Game", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Labirint", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Game", "Game")
                        .WithMany("Labirints")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.LabirintCryptoBlock", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Labirint", "Labirint")
                        .WithMany("CryptoBlocks")
                        .HasForeignKey("LabirintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Labirint");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.LabirintCryptoKeyFragment", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Labirint", "Labirint")
                        .WithMany("CryptoKeyFragments")
                        .HasForeignKey("LabirintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Labirint");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.LabirintEnergy", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Labirint", "Labirint")
                        .WithMany("Energies")
                        .HasForeignKey("LabirintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Labirint");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.SeasonHistory", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Season", "Season")
                        .WithMany("History")
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.User", "User")
                        .WithMany("SeasonHistory")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Season");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.ShopProposal", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Item", "BuyItem")
                        .WithMany("BuyProposals")
                        .HasForeignKey("BuyItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Item", "SellItem")
                        .WithMany("SellProposals")
                        .HasForeignKey("SellItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BuyItem");

                    b.Navigation("SellItem");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.UserItem", b =>
                {
                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.Item", "Item")
                        .WithMany("UserItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CryptoMaze.LabirintGameServer.DAL.Entities.User", "User")
                        .WithMany("Items")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Game", b =>
                {
                    b.Navigation("Labirints");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Item", b =>
                {
                    b.Navigation("BuyProposals");

                    b.Navigation("SellProposals");

                    b.Navigation("UserItems");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Labirint", b =>
                {
                    b.Navigation("CryptoBlocks");

                    b.Navigation("CryptoKeyFragments");

                    b.Navigation("Energies");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.Season", b =>
                {
                    b.Navigation("History");
                });

            modelBuilder.Entity("CryptoMaze.LabirintGameServer.DAL.Entities.User", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Items");

                    b.Navigation("SeasonHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
