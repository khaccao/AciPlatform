using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSupplier",
                table: "Customers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChartOfAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpeningDebit = table.Column<double>(type: "float", nullable: true),
                    OpeningCredit = table.Column<double>(type: "float", nullable: true),
                    ArisingDebit = table.Column<double>(type: "float", nullable: true),
                    ArisingCredit = table.Column<double>(type: "float", nullable: true),
                    IsForeignCurrency = table.Column<bool>(type: "bit", nullable: false),
                    OpeningForeignDebit = table.Column<double>(type: "float", nullable: true),
                    OpeningForeignCredit = table.Column<double>(type: "float", nullable: true),
                    ArisingForeignDebit = table.Column<double>(type: "float", nullable: true),
                    ArisingForeignCredit = table.Column<double>(type: "float", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<double>(type: "float", nullable: true),
                    AccGroup = table.Column<int>(type: "int", nullable: false),
                    Classification = table.Column<int>(type: "int", nullable: false),
                    Protected = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HasChild = table.Column<bool>(type: "bit", nullable: false),
                    HasDetails = table.Column<bool>(type: "bit", nullable: false),
                    ParentRef = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayInsert = table.Column<bool>(type: "bit", nullable: false),
                    DisplayDelete = table.Column<bool>(type: "bit", nullable: false),
                    StockUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningStockQuantity = table.Column<double>(type: "float", nullable: true),
                    ArisingStockQuantity = table.Column<double>(type: "float", nullable: true),
                    StockUnitPrice = table.Column<double>(type: "float", nullable: true),
                    WarehouseCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OpeningDebitNB = table.Column<double>(type: "float", nullable: true),
                    OpeningCreditNB = table.Column<double>(type: "float", nullable: true),
                    ArisingDebitNB = table.Column<double>(type: "float", nullable: true),
                    ArisingCreditNB = table.Column<double>(type: "float", nullable: true),
                    OpeningForeignDebitNB = table.Column<double>(type: "float", nullable: true),
                    OpeningForeignCreditNB = table.Column<double>(type: "float", nullable: true),
                    ArisingForeignDebitNB = table.Column<double>(type: "float", nullable: true),
                    ArisingForeignCreditNB = table.Column<double>(type: "float", nullable: true),
                    OpeningStockQuantityNB = table.Column<double>(type: "float", nullable: true),
                    ArisingStockQuantityNB = table.Column<double>(type: "float", nullable: true),
                    StockUnitPriceNB = table.Column<double>(type: "float", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    IsInternal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartOfAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LedgerEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    BookDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVoucher = table.Column<bool>(type: "bit", nullable: false),
                    OrginalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrginalVoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrginalBookDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrginalFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrginalDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrginalDescriptionEN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrginalCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrginalAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachVoucher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceVoucherNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceBookDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferenceAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceAdditionalDeclarationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceTaxCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceSerial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceProductItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitWarehouse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitDetailCodeFirst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitDetailCodeSecond = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditWarehouse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditDetailCodeFirst = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditDetailCodeSecond = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepreciaMonth = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Group = table.Column<int>(type: "int", nullable: false),
                    DepreciaDuration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    OrginalCurrency = table.Column<double>(type: "float", nullable: false),
                    ExchangeRate = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsAriseMark = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserCreated = table.Column<long>(type: "bigint", nullable: false),
                    UserUpdated = table.Column<long>(type: "bigint", nullable: false),
                    UserDeleted = table.Column<long>(type: "bigint", nullable: false),
                    IsInternal = table.Column<int>(type: "int", nullable: false),
                    DebitCodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitDetailCodeFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitDetailCodeSecondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditDetailCodeFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditDetailCodeSecondName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitWarehouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditWarehouseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillId = table.Column<int>(type: "int", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Tab = table.Column<int>(type: "int", nullable: true),
                    PercentImportTax = table.Column<double>(type: "float", nullable: true),
                    PercentTransport = table.Column<double>(type: "float", nullable: true),
                    AmountTransport = table.Column<double>(type: "float", nullable: true),
                    AmountImportWarehouse = table.Column<double>(type: "float", nullable: true),
                    Classification = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Detail1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Detail2 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerEntries", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChartOfAccounts");

            migrationBuilder.DropTable(
                name: "LedgerEntries");

            migrationBuilder.DropColumn(
                name: "IsSupplier",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "Customers");
        }
    }
}
