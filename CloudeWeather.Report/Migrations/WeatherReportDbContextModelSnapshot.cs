﻿// <auto-generated />
using System;
using CloudeWeather.Report.DataAcess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CloudeWeather.Report.Migrations
{
    [DbContext(typeof(WeatherReportDbContext))]
    partial class WeatherReportDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CloudeWeather.Report.DataAcess.WeatherReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("RainfallTotalInches")
                        .HasColumnType("numeric");

                    b.Property<decimal>("SnowTotalInches")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TempHighF")
                        .HasColumnType("numeric");

                    b.Property<decimal>("TempLowF")
                        .HasColumnType("numeric");

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Weather_report", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}