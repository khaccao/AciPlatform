using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarFleets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarFleetId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarFleets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFleets", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarFleetId",
                table: "Cars",
                column: "CarFleetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_CarFleets_CarFleetId",
                table: "Cars",
                column: "CarFleetId",
                principalTable: "CarFleets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_CarFleets_CarFleetId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "CarFleets");

            migrationBuilder.DropIndex(
                name: "IX_Cars_CarFleetId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarFleetId",
                table: "Cars");
        }
    }
}
