using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDegreeCertificateWithUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Degrees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Degrees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Certificates");
        }
    }
}
