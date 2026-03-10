using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserMenus_UserId_MenuId",
                table: "UserMenus");

            migrationBuilder.CreateIndex(
                name: "IX_UserMenus_UserId",
                table: "UserMenus",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserMenus_UserId",
                table: "UserMenus");

            migrationBuilder.CreateIndex(
                name: "IX_UserMenus_UserId_MenuId",
                table: "UserMenus",
                columns: new[] { "UserId", "MenuId" },
                unique: true);
        }
    }
}
