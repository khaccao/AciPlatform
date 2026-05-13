using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AciPlatform.Infrastructure.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class UpdateRoomStatusAndRentalPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    ParentGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HotelGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AreaName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AreaType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AreaTypeGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AreaAlias = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AreaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AreaAvatar = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PositionX = table.Column<int>(type: "int", nullable: true),
                    PositionY = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    DmsLockId = table.Column<long>(type: "bigint", nullable: true),
                    DmsHardwareId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelAreas_HotelAreas_ParentId",
                        column: x => x.ParentId,
                        principalTable: "HotelAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HotelAreaTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HotelGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descriptions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelAreaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelBeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoomNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BedCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BedType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBeds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    GuestId = table.Column<int>(type: "int", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GuestPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuestIdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NightCount = table.Column<int>(type: "int", nullable: false),
                    RoomPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServicePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehiclePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GroupSize = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SpecialRequests = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CheckInActual = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOutActual = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelGroupMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    MemberNo = table.Column<int>(type: "int", nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GuestPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuestIdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoomNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BedCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelGroupMembers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelGuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuestCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PreferRoomType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PreferVehicle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TotalVisits = table.Column<int>(type: "int", nullable: false),
                    TotalSpend = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastVisitDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsVIP = table.Column<bool>(type: "bit", nullable: false),
                    IsBlacklisted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelGuests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InvoiceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RoomAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehicleAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IssuedBy = table.Column<int>(type: "int", nullable: true),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelInvoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    HotelType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    CheckInTime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CheckOutTime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PmsConnectionString = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PmsDbName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PmsIpAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DmsAppId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DmsAppSecret = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsLinkedToAciCompany = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelRoomForecast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoomNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BedCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ForecastDate = table.Column<DateOnly>(type: "date", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    BlockType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BlockNote = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelRoomForecast", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelSeasonalPricing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SeasonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeasonType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PriceMultiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelSeasonalPricing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ServiceNameEN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TyLeSC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TyLeVAT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxQuantity = table.Column<int>(type: "int", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SettingKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelTourGuides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuideCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Languages = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Speciality = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsFreelance = table.Column<bool>(type: "bit", nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    HrEmployeeId = table.Column<int>(type: "int", nullable: true),
                    IdCard = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MonthlyBaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelTourGuides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelTours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TourCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TourName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TourNameEN = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TourType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    DurationNights = table.Column<int>(type: "int", nullable: false),
                    MaxPerson = table.Column<int>(type: "int", nullable: false),
                    MinPerson = table.Column<int>(type: "int", nullable: false),
                    PricePerPerson = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GroupPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GroupDiscountFrom = table.Column<int>(type: "int", nullable: false),
                    Highlights = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Itinerary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Inclusions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Exclusions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MeetingPoint = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Difficulty = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelTours", x => x.Id);
                    table.UniqueConstraint("AK_HotelTours_TourCode", x => x.TourCode);
                });

            migrationBuilder.CreateTable(
                name: "HotelVehicleRentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RentalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: true),
                    VehicleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GuestPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GuestIdCard = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RentFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RentTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositReturned = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DamageFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FuelLevelOut = table.Column<int>(type: "int", nullable: false),
                    FuelLevelIn = table.Column<int>(type: "int", nullable: true),
                    ConditionOut = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ConditionIn = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DamageNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelVehicleRentals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelVehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VehicleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BienSo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TenXe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearMade = table.Column<int>(type: "int", nullable: true),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositRequired = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FuelLevel = table.Column<int>(type: "int", nullable: false),
                    Condition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelVehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PMS_Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PmsRoomId = table.Column<int>(type: "int", nullable: true),
                    So = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Loai = table.Column<int>(type: "int", nullable: true),
                    Ma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    KhuVucCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BuildingID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AreaId = table.Column<int>(type: "int", nullable: true),
                    SachBan = table.Column<int>(type: "int", nullable: true),
                    CleanDirty = table.Column<int>(type: "int", nullable: true),
                    Inspected = table.Column<int>(type: "int", nullable: true),
                    TinhTrang = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaxPerson = table.Column<int>(type: "int", nullable: true),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMS_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PMS_RoomTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PmsItemId = table.Column<int>(type: "int", nullable: true),
                    Ma = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxPerson = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    FlagType = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Amenities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SyncDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMS_RoomTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HotelElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HotelGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaId = table.Column<int>(type: "int", nullable: false),
                    AreaGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PositionX = table.Column<int>(type: "int", nullable: false),
                    PositionY = table.Column<int>(type: "int", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Rotation = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelElements_HotelAreas_AreaId",
                        column: x => x.AreaId,
                        principalTable: "HotelAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    RoomNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BedCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoomType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NightCount = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookingRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookingRooms_HotelBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "HotelBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelBookingServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ServiceCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelBookingServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelBookingServices_HotelBookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "HotelBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PmsTourGuideContracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuideId = table.Column<int>(type: "int", nullable: false),
                    ContractCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ContractType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmsTourGuideContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmsTourGuideContracts_HotelTourGuides_GuideId",
                        column: x => x.GuideId,
                        principalTable: "HotelTourGuides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PmsTourGuideSalaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GuideId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TourCount = table.Column<int>(type: "int", nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TourIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deductions = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmsTourGuideSalaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmsTourGuideSalaries_HotelTourGuides_GuideId",
                        column: x => x.GuideId,
                        principalTable: "HotelTourGuides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HotelTourSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TourCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TourDate = table.Column<DateOnly>(type: "date", nullable: false),
                    GuideId = table.Column<int>(type: "int", nullable: true),
                    MaxSlots = table.Column<int>(type: "int", nullable: false),
                    BookedSlots = table.Column<int>(type: "int", nullable: false),
                    PriceOverride = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelTourSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelTourSchedules_HotelTours_TourCode",
                        column: x => x.TourCode,
                        principalTable: "HotelTours",
                        principalColumn: "TourCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelAreas_ParentId",
                table: "HotelAreas",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelAreaTypes_Code_HotelGuid",
                table: "HotelAreaTypes",
                columns: new[] { "Code", "HotelGuid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelBeds_HotelCode_RoomNo_BedCode",
                table: "HotelBeds",
                columns: new[] { "HotelCode", "RoomNo", "BedCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookingRooms_BookingId",
                table: "HotelBookingRooms",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_HotelCode_BookingCode",
                table: "HotelBookings",
                columns: new[] { "HotelCode", "BookingCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_HotelCode_CheckIn",
                table: "HotelBookings",
                columns: new[] { "HotelCode", "CheckIn" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookings_HotelCode_Status",
                table: "HotelBookings",
                columns: new[] { "HotelCode", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelBookingServices_BookingId",
                table: "HotelBookingServices",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelElements_AreaId",
                table: "HotelElements",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_HotelGroupMembers_BookingId_MemberNo",
                table: "HotelGroupMembers",
                columns: new[] { "BookingId", "MemberNo" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelGuests_HotelCode_Phone",
                table: "HotelGuests",
                columns: new[] { "HotelCode", "Phone" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelInvoices_HotelCode_InvoiceCode",
                table: "HotelInvoices",
                columns: new[] { "HotelCode", "InvoiceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelProperties_Code",
                table: "HotelProperties",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelRoomForecast_HotelCode_ForecastDate",
                table: "HotelRoomForecast",
                columns: new[] { "HotelCode", "ForecastDate" });

            migrationBuilder.CreateIndex(
                name: "IX_HotelServices_HotelCode_ServiceCode",
                table: "HotelServices",
                columns: new[] { "HotelCode", "ServiceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelSettings_HotelCode_SettingKey",
                table: "HotelSettings",
                columns: new[] { "HotelCode", "SettingKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelTours_HotelCode_TourCode",
                table: "HotelTours",
                columns: new[] { "HotelCode", "TourCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelTourSchedules_HotelCode_TourCode_TourDate",
                table: "HotelTourSchedules",
                columns: new[] { "HotelCode", "TourCode", "TourDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelTourSchedules_TourCode",
                table: "HotelTourSchedules",
                column: "TourCode");

            migrationBuilder.CreateIndex(
                name: "IX_HotelVehicleRentals_HotelCode_RentalCode",
                table: "HotelVehicleRentals",
                columns: new[] { "HotelCode", "RentalCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelVehicles_HotelCode_VehicleCode",
                table: "HotelVehicles",
                columns: new[] { "HotelCode", "VehicleCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PMS_Rooms_HotelCode_So",
                table: "PMS_Rooms",
                columns: new[] { "HotelCode", "So" });

            migrationBuilder.CreateIndex(
                name: "IX_PMS_RoomTypes_HotelCode_Ma",
                table: "PMS_RoomTypes",
                columns: new[] { "HotelCode", "Ma" });

            migrationBuilder.CreateIndex(
                name: "IX_PmsTourGuideContracts_GuideId",
                table: "PmsTourGuideContracts",
                column: "GuideId");

            migrationBuilder.CreateIndex(
                name: "IX_PmsTourGuideSalaries_GuideId",
                table: "PmsTourGuideSalaries",
                column: "GuideId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HotelAreaTypes");

            migrationBuilder.DropTable(
                name: "HotelBeds");

            migrationBuilder.DropTable(
                name: "HotelBookingRooms");

            migrationBuilder.DropTable(
                name: "HotelBookingServices");

            migrationBuilder.DropTable(
                name: "HotelElements");

            migrationBuilder.DropTable(
                name: "HotelGroupMembers");

            migrationBuilder.DropTable(
                name: "HotelGuests");

            migrationBuilder.DropTable(
                name: "HotelInvoices");

            migrationBuilder.DropTable(
                name: "HotelProperties");

            migrationBuilder.DropTable(
                name: "HotelRoomForecast");

            migrationBuilder.DropTable(
                name: "HotelSeasonalPricing");

            migrationBuilder.DropTable(
                name: "HotelServices");

            migrationBuilder.DropTable(
                name: "HotelSettings");

            migrationBuilder.DropTable(
                name: "HotelTourSchedules");

            migrationBuilder.DropTable(
                name: "HotelVehicleRentals");

            migrationBuilder.DropTable(
                name: "HotelVehicles");

            migrationBuilder.DropTable(
                name: "PMS_Rooms");

            migrationBuilder.DropTable(
                name: "PMS_RoomTypes");

            migrationBuilder.DropTable(
                name: "PmsTourGuideContracts");

            migrationBuilder.DropTable(
                name: "PmsTourGuideSalaries");

            migrationBuilder.DropTable(
                name: "HotelBookings");

            migrationBuilder.DropTable(
                name: "HotelAreas");

            migrationBuilder.DropTable(
                name: "HotelTours");

            migrationBuilder.DropTable(
                name: "HotelTourGuides");
        }
    }
}
