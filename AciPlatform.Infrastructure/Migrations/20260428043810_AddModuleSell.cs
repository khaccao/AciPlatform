using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModuleSell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProjectMembers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GoodCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodCustomers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodDetails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: true),
                    AccountParent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNameParent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail1Parent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailName1Parent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail2Parent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailName2Parent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WarehouseParent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    GoodID = table.Column<int>(type: "int", nullable: true),
                    GoodsQuotaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodDetails", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuType = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PriceList = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    GoodsType = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    SalePrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: false),
                    Inventory = table.Column<long>(type: "bigint", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Delivery = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MinStockLevel = table.Column<long>(type: "bigint", nullable: false),
                    MaxStockLevel = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Detail1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DetailName1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Detail2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DetailName2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Detail1English = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailName1English = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail1Korean = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetailName1Korean = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image1 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Image2 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Image3 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Image4 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Image5 = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    WebGoodNameVietNam = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WebPriceVietNam = table.Column<double>(type: "float", nullable: true),
                    WebDiscountVietNam = table.Column<double>(type: "float", nullable: true),
                    TitleVietNam = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ContentVietNam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebGoodNameKorea = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WebPriceKorea = table.Column<double>(type: "float", nullable: true),
                    WebDiscountKorea = table.Column<double>(type: "float", nullable: true),
                    TitleKorea = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentKorea = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebGoodNameEnglish = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WebPriceEnglish = table.Column<double>(type: "float", nullable: true),
                    WebDiscountEnglish = table.Column<double>(type: "float", nullable: true),
                    TitleEnglish = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ContentEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isPromotion = table.Column<bool>(type: "bit", nullable: true),
                    DateManufacture = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StockUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreated = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateApplicable = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Net = table.Column<double>(type: "float", nullable: true),
                    TaxRateId = table.Column<long>(type: "bigint", nullable: true),
                    OpeningStockQuantityNB = table.Column<double>(type: "float", nullable: true),
                    IsService = table.Column<bool>(type: "bit", nullable: false),
                    GoodsQuotaId = table.Column<int>(type: "int", nullable: true),
                    NumberItem = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsPriceLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsId = table.Column<int>(type: "int", nullable: false),
                    PriceList = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    SalePrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsPriceLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsPromotionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsPromotionId = table.Column<int>(type: "int", nullable: false),
                    Standard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<double>(type: "float", nullable: false),
                    Detail1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail2Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsPromotionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsPromotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<double>(type: "float", nullable: false),
                    FromAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsPromotions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsQuotaDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsQuotaId = table.Column<int>(type: "int", nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailName1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detail2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DetailName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsQuotaDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsQuotaRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoodsQuotaStepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsQuotaRecipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsQuotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GoodsQuotaRecipeId = table.Column<int>(type: "int", nullable: false),
                    GoodsQuotaCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GoodsQuotaStepId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsQuotas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GoodsQuotaSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIds = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsQuotaSteps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    GoodId = table.Column<int>(type: "int", nullable: false),
                    GoodDetailId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TaxVAT = table.Column<double>(type: "float", nullable: true),
                    AdultQuantity = table.Column<int>(type: "int", nullable: true),
                    ChildrenQuantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    TotalPriceDiscount = table.Column<double>(type: "float", nullable: false),
                    TotalPricePaid = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tell = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPayment = table.Column<bool>(type: "bit", nullable: false),
                    PaymentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Broker = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    BillId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Promotion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderSuccessfuls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSuccessfuls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BankNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Product = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PayerType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodCustomers");

            migrationBuilder.DropTable(
                name: "GoodDetails");

            migrationBuilder.DropTable(
                name: "Goods");

            migrationBuilder.DropTable(
                name: "GoodsPriceLists");

            migrationBuilder.DropTable(
                name: "GoodsPromotionDetails");

            migrationBuilder.DropTable(
                name: "GoodsPromotions");

            migrationBuilder.DropTable(
                name: "GoodsQuotaDetails");

            migrationBuilder.DropTable(
                name: "GoodsQuotaRecipes");

            migrationBuilder.DropTable(
                name: "GoodsQuotas");

            migrationBuilder.DropTable(
                name: "GoodsQuotaSteps");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderSuccessfuls");

            migrationBuilder.DropTable(
                name: "Payers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectMembers");
        }
    }
}
