using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class enrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAccessedAt",
                table: "Enrollments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastAccessedAt",
                table: "Enrollments",
                type: "datetime2",
                nullable: true);
        }
    }
}
