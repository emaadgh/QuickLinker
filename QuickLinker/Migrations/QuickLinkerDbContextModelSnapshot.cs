﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuickLinker.API.DbContexts;

#nullable disable

namespace QuickLinker.API.Migrations
{
    [DbContext(typeof(QuickLinkerDbContext))]
    partial class QuickLinkerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuickLinker.API.Entities.ShortenedURL", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTimeOffset>("CreationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("OriginalURL")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("ShortCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("ID");

                    b.HasIndex("ShortCode")
                        .IsUnique();

                    b.ToTable("ShortenedURLs");

                    b.HasData(
                        new
                        {
                            ID = -1,
                            CreationDate = new DateTimeOffset(new DateTime(2024, 4, 10, 18, 17, 53, 618, DateTimeKind.Unspecified).AddTicks(3326), new TimeSpan(0, 0, 0, 0, 0)),
                            OriginalURL = "www.google.com",
                            ShortCode = "12345abcde"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}