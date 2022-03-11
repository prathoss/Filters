﻿// <auto-generated />
using Filters.Demo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Filters.Demo.Migrations
{
    [DbContext(typeof(BikeContext))]
    [Migration("20220309202021_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Filters.Demo.Data.Models.Bike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Bikes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Manufacturer = "YT",
                            Model = "Izzo",
                            Type = 3
                        },
                        new
                        {
                            Id = 2,
                            Manufacturer = "YT",
                            Model = "Jeffsy",
                            Type = 3
                        },
                        new
                        {
                            Id = 3,
                            Manufacturer = "YT",
                            Model = "Capra",
                            Type = 4
                        },
                        new
                        {
                            Id = 4,
                            Manufacturer = "YT",
                            Model = "Tues",
                            Type = 5
                        },
                        new
                        {
                            Id = 5,
                            Manufacturer = "Propain",
                            Model = "Hugene",
                            Type = 3
                        },
                        new
                        {
                            Id = 6,
                            Manufacturer = "Propain",
                            Model = "Tyee",
                            Type = 4
                        },
                        new
                        {
                            Id = 7,
                            Manufacturer = "Propain",
                            Model = "Spindrift",
                            Type = 4
                        },
                        new
                        {
                            Id = 8,
                            Manufacturer = "Propain",
                            Model = "Rage",
                            Type = 5
                        },
                        new
                        {
                            Id = 9,
                            Manufacturer = "Canyon",
                            Model = "Aero",
                            Type = 0
                        },
                        new
                        {
                            Id = 10,
                            Manufacturer = "Canyon",
                            Model = "Endurance",
                            Type = 0
                        },
                        new
                        {
                            Id = 11,
                            Manufacturer = "Canyon",
                            Model = "Race",
                            Type = 0
                        },
                        new
                        {
                            Id = 12,
                            Manufacturer = "Canyon",
                            Model = "Grizl",
                            Type = 1
                        },
                        new
                        {
                            Id = 13,
                            Manufacturer = "Canyon",
                            Model = "Grail",
                            Type = 1
                        },
                        new
                        {
                            Id = 14,
                            Manufacturer = "Canyon",
                            Model = "Exceed",
                            Type = 2
                        },
                        new
                        {
                            Id = 15,
                            Manufacturer = "Canyon",
                            Model = "Lux",
                            Type = 2
                        });
                });
#pragma warning restore 612, 618
        }
    }
}