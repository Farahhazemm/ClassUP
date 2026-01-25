using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_InstructorId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                table: "Courses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses",
                newName: "IX_Courses_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_UserId",
                table: "Courses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_UserId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Courses",
                newName: "InstructorId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_UserId",
                table: "Courses",
                newName: "IX_Courses_InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
