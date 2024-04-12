using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickLinker.API.Migrations
{
    /// <inheritdoc />
    public partial class updatesdummydata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShortenedURLs",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "CreationDate", "ShortCode" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 11, 13, 39, 29, 742, DateTimeKind.Unspecified).AddTicks(9495), new TimeSpan(0, 0, 0, 0, 0)), "GRNHv-VdDK" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ShortenedURLs",
                keyColumn: "ID",
                keyValue: -1,
                columns: new[] { "CreationDate", "ShortCode" },
                values: new object[] { new DateTimeOffset(new DateTime(2024, 4, 10, 18, 17, 53, 618, DateTimeKind.Unspecified).AddTicks(3326), new TimeSpan(0, 0, 0, 0, 0)), "12345abcde" });
        }
    }
}
