using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFleetTransportationModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarFields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarFieldSetups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    CarFieldId = table.Column<int>(type: "int", nullable: false),
                    ValueNumber = table.Column<double>(type: "float", nullable: true),
                    FromAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WarningAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserIdString = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFieldSetups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarLocationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarLocationId = table.Column<int>(type: "int", nullable: false),
                    LicensePlates = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DriverName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PlanInprogress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PlanExpected = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FileStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarLocationDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProcedureNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicensePlates = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MileageAllowance = table.Column<double>(type: "float", nullable: false),
                    FuelAmount = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverRouterDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverRouterId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FileStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoliceCheckPointId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverRouterDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DriverRouters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PetrolConsumptionId = table.Column<int>(type: "int", nullable: false),
                    AdvancePaymentAmount = table.Column<double>(type: "float", nullable: true),
                    FuelAmount = table.Column<double>(type: "float", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverRouters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodWarehouseExports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodWarehouseId = table.Column<int>(type: "int", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodWarehouseExports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodWarehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WarehouseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Detail1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DetailName1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Detail2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DetailName2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GoodsType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Image1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    QuantityInput = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PriceList = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    OrginalVoucherNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LedgerId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateManufacture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPrinted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodWarehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodWarehousesPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodWarehousesId = table.Column<int>(type: "int", nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WareHouseShelvesId = table.Column<int>(type: "int", nullable: false),
                    WareHouseFloorId = table.Column<int>(type: "int", nullable: false),
                    WareHousePositionId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodWarehousesPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WarehouseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Detail1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DetailName1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Detail2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DetailName2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Image1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InputQuantity = table.Column<double>(type: "float", nullable: false),
                    OutputQuantity = table.Column<double>(type: "float", nullable: false),
                    CloseQuantity = table.Column<double>(type: "float", nullable: false),
                    CloseQuantityReal = table.Column<double>(type: "float", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isCheck = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetrolConsumptionPoliceCheckPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetrolConsumptionId = table.Column<int>(type: "int", nullable: false),
                    PoliceCheckPointName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsArise = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetrolConsumptionPoliceCheckPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetrolConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    PetroPrice = table.Column<double>(type: "float", nullable: false),
                    KmFrom = table.Column<double>(type: "float", nullable: false),
                    KmTo = table.Column<double>(type: "float", nullable: false),
                    LocationFrom = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LocationTo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdvanceAmount = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RoadRouteId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetrolConsumptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliceCheckPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliceCheckPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoadRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RoadRouteDetail = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PoliceCheckPointIdStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfTrips = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadRoutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseFloors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    UserUpdated = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseFloors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseFloorWithPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseFloorId = table.Column<int>(type: "int", nullable: false),
                    WareHousePositionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseFloorWithPositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHousePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    UserUpdated = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHousePositions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ManagerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsSyncChartOfAccount = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    UserUpdated = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseShelves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    OrderHorizontal = table.Column<int>(type: "int", nullable: false),
                    OrderVertical = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<int>(type: "int", nullable: true),
                    UserUpdated = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseShelves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseShelvesWithFloors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseShelvesId = table.Column<int>(type: "int", nullable: false),
                    WareHouseFloorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseShelvesWithFloors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WareHouseWithShelves",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WareHouseId = table.Column<int>(type: "int", nullable: false),
                    WareHouseShelveId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseWithShelves", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarFields");

            migrationBuilder.DropTable(
                name: "CarFieldSetups");

            migrationBuilder.DropTable(
                name: "CarLocationDetails");

            migrationBuilder.DropTable(
                name: "CarLocations");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "DriverRouterDetails");

            migrationBuilder.DropTable(
                name: "DriverRouters");

            migrationBuilder.DropTable(
                name: "GoodWarehouseExports");

            migrationBuilder.DropTable(
                name: "GoodWarehouses");

            migrationBuilder.DropTable(
                name: "GoodWarehousesPositions");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "PetrolConsumptionPoliceCheckPoints");

            migrationBuilder.DropTable(
                name: "PetrolConsumptions");

            migrationBuilder.DropTable(
                name: "PoliceCheckPoints");

            migrationBuilder.DropTable(
                name: "RoadRoutes");

            migrationBuilder.DropTable(
                name: "WareHouseFloors");

            migrationBuilder.DropTable(
                name: "WareHouseFloorWithPositions");

            migrationBuilder.DropTable(
                name: "WareHousePositions");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "WareHouseShelves");

            migrationBuilder.DropTable(
                name: "WareHouseShelvesWithFloors");

            migrationBuilder.DropTable(
                name: "WareHouseWithShelves");
        }
    }
}
