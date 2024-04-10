using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickLinker.API.Migrations
{
    /// <inheritdoc />
    public partial class addsdummydata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ShortenedURLs",
                columns: new[] { "ID", "CreationDate", "OriginalURL", "ShortCode" },
                values: new object[] { -1, new DateTimeOffset(new DateTime(2024, 4, 10, 18, 17, 53, 618, DateTimeKind.Unspecified).AddTicks(3326), new TimeSpan(0, 0, 0, 0, 0)), "www.google.com", "12345abcde" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ShortenedURLs",
                keyColumn: "ID",
                keyValue: -1);
        }
    }
}
