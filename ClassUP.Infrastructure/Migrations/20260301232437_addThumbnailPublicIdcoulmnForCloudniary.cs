using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addThumbnailPublicIdcoulmnForCloudniary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailPublicId",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailPublicId",
                table: "Courses");
        }
    }
}
