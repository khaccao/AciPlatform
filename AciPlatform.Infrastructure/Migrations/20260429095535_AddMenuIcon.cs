using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Menus",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Menus");
        }
    }
}
