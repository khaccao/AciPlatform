using AciPlatform.Domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Persistence;

/// <summary>
/// Dedicated DbContext for AciPlatform_Hotel database.
/// Connection string: "HotelConnection" in appsettings.json
/// </summary>
public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    // ── Property / Structure ──────────────────────────────────
    public DbSet<HotelProperty> HotelProperties { get; set; }
    public DbSet<HotelAreaType> HotelAreaTypes { get; set; }
    public DbSet<HotelArea> HotelAreas { get; set; }
    public DbSet<HotelElement> HotelElements { get; set; }
    public DbSet<PmsRoomType> PmsRoomTypes { get; set; }
    public DbSet<HotelSetting> HotelSettings { get; set; }

    // ── Rooms & Beds ──────────────────────────────────────────
    public DbSet<PmsRoom> PmsRooms { get; set; }
    public DbSet<HotelBed> HotelBeds { get; set; }

    // ── Guests ────────────────────────────────────────────────
    public DbSet<HotelGuest> HotelGuests { get; set; }

    // ── Booking ───────────────────────────────────────────────
    public DbSet<HotelBooking> HotelBookings { get; set; }
    public DbSet<HotelBookingRoom> HotelBookingRooms { get; set; }
    public DbSet<AciPlatform.Domain.Entities.Hotel.HotelBookingService> HotelBookingServices { get; set; }
    public DbSet<HotelGroupMember> HotelGroupMembers { get; set; }

    // ── Room Forecast ─────────────────────────────────────────
    public DbSet<HotelRoomForecast> HotelRoomForecasts { get; set; }

    // ── Vehicles ──────────────────────────────────────────────
    public DbSet<HotelVehicle> HotelVehicles { get; set; }
    public DbSet<HotelVehicleRental> HotelVehicleRentals { get; set; }

    // ── Services Catalog ──────────────────────────────────────
    public DbSet<HotelService> HotelServices { get; set; }

    // ── Tours ─────────────────────────────────────────────────
    public DbSet<HotelTour> HotelTours { get; set; }
    public DbSet<HotelTourGuide> HotelTourGuides { get; set; }
    public DbSet<HotelTourSchedule> HotelTourSchedules { get; set; }

    // ── Guide Management (HR Integration) ─────────────────────

    public DbSet<PmsTourGuideContract> PmsTourGuideContracts { get; set; }
    public DbSet<PmsTourGuideSalary> PmsTourGuideSalaries { get; set; }

    // ── Finance ───────────────────────────────────────────────
    public DbSet<HotelInvoice> HotelInvoices { get; set; }

    // ── Pricing ───────────────────────────────────────────────
    public DbSet<HotelSeasonalPricing> HotelSeasonalPricings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Property
        modelBuilder.Entity<HotelProperty>(e => {
            e.HasIndex(x => x.Code).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Area tree (self-referencing)
        modelBuilder.Entity<HotelArea>(e => {
            e.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.Elements).WithOne(x => x.Area).HasForeignKey(x => x.AreaId);
        });

        // AreaType unique per hotel
        modelBuilder.Entity<HotelAreaType>(e =>
            e.HasIndex(x => new { x.Code, x.HotelGuid }).IsUnique());

        // PmsRoomType
        modelBuilder.Entity<PmsRoomType>(e => {
            e.HasIndex(x => new { x.HotelCode, x.Ma });
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // HotelSettings
        modelBuilder.Entity<HotelSetting>(e =>
            e.HasIndex(x => new { x.HotelCode, x.SettingKey }).IsUnique());

        // PmsRoom
        modelBuilder.Entity<PmsRoom>(e => {
            e.HasIndex(x => new { x.HotelCode, x.So });
        });

        // HotelBeds
        modelBuilder.Entity<HotelBed>(e =>
            e.HasIndex(x => new { x.HotelCode, x.RoomNo, x.BedCode }).IsUnique());

        // HotelGuests
        modelBuilder.Entity<HotelGuest>(e => {
            e.HasIndex(x => new { x.HotelCode, x.Phone });
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // HotelBookings
        modelBuilder.Entity<HotelBooking>(e => {
            e.HasIndex(x => new { x.HotelCode, x.BookingCode }).IsUnique();
            e.HasIndex(x => new { x.HotelCode, x.Status });
            e.HasIndex(x => new { x.HotelCode, x.CheckIn });
            e.HasQueryFilter(x => !x.IsDeleted);
            e.HasMany(x => x.Rooms).WithOne().HasForeignKey(r => r.BookingId);
            e.HasMany(x => x.Services).WithOne().HasForeignKey(s => s.BookingId);
        });

        // HotelRoomForecast - composite unique (BedCode nullable needs special handling)
        modelBuilder.Entity<HotelRoomForecast>(e => {
            e.HasIndex(x => new { x.HotelCode, x.ForecastDate });
        });

        // Vehicles
        modelBuilder.Entity<HotelVehicle>(e => {
            e.HasIndex(x => new { x.HotelCode, x.VehicleCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });
        modelBuilder.Entity<HotelVehicleRental>(e => {
            e.HasIndex(x => new { x.HotelCode, x.RentalCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Services
        modelBuilder.Entity<HotelService>(e => {
            e.HasIndex(x => new { x.HotelCode, x.ServiceCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // Tours
        modelBuilder.Entity<HotelTour>(e => {
            e.HasIndex(x => new { x.HotelCode, x.TourCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
            e.HasMany(x => x.Schedules).WithOne().HasForeignKey(s => s.TourCode).HasPrincipalKey(t => t.TourCode).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<HotelTourGuide>(e => {
            e.HasQueryFilter(x => !x.IsDeleted);
        });
        modelBuilder.Entity<HotelTourSchedule>(e => {
            e.HasIndex(x => new { x.HotelCode, x.TourCode, x.TourDate }).IsUnique();
        });

        // GroupMembers
        modelBuilder.Entity<HotelGroupMember>(e =>
            e.HasIndex(x => new { x.BookingId, x.MemberNo }));

        // Finance
        modelBuilder.Entity<HotelInvoice>(e => {
            e.HasIndex(x => new { x.HotelCode, x.InvoiceCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });
    }
}
