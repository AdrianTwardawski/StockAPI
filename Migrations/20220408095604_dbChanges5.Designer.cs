﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockAPI.Entities;

namespace StockAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220408095604_dbChanges5")]
    partial class dbChanges5
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StockAPI.Entities.Market", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Change")
                        .HasColumnType("real");

                    b.Property<float>("Price")
                        .HasMaxLength(25)
                        .HasColumnType("real");

                    b.Property<string>("Stock")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Time")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradesValue")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Market");
                });

            modelBuilder.Entity("StockAPI.Entities.Observed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MarketId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfActions")
                        .HasColumnType("int");

                    b.Property<float>("Profit")
                        .HasColumnType("real");

                    b.Property<float>("PurchasePrice")
                        .HasColumnType("real");

                    b.Property<string>("Stock")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MarketId");

                    b.ToTable("Observed");
                });

            modelBuilder.Entity("StockAPI.Entities.Observed", b =>
                {
                    b.HasOne("StockAPI.Entities.Market", "Market")
                        .WithMany()
                        .HasForeignKey("MarketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Market");
                });
#pragma warning restore 612, 618
        }
    }
}
