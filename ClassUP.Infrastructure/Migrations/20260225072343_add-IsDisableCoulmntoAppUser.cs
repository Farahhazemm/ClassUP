using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassUP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsDisableCoulmntoAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "Users");
        }
    }
}
