using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickLinker.API.Migrations
{
    /// <inheritdoc />
    public partial class updatedummydatatobeavalidURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShortenedURLs",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "CreationDate", "OriginalURL", "ShortCode" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 12, 14, 8, 56, 313, DateTimeKind.Unspecified).AddTicks(2546), new TimeSpan(0, 0, 0, 0, 0)), "https://www.google.com", "rGu2aeQORK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShortenedURLs",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "CreationDate", "OriginalURL", "ShortCode" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 11, 13, 39, 29, 742, DateTimeKind.Unspecified).AddTicks(9495), new TimeSpan(0, 0, 0, 0, 0)), "www.google.com", "GRNHv-VdDK" });
        }
    }
}
