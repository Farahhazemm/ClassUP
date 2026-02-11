using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_Progress_Busniss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastWatchedAt",
                table: "LectureProgresses");

            migrationBuilder.DropColumn(
                name: "WatchedDuration",
                table: "LectureProgresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastWatchedAt",
                table: "LectureProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "WatchedDuration",
                table: "LectureProgresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
