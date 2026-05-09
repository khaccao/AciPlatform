using AciPlatform.Domain.Entities.Hotel;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Persistence;

/// <summary>
/// Dedicated DbContext for AciPlatform_Hotel database.
/// Separate from ApplicationDbContext (main ERP DB).
/// Connection string: "HotelConnection" in appsettings.json
/// </summary>
public class HotelDbContext : DbContext
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

    // Core management
    public DbSet<HotelBed> HotelBeds { get; set; }
    public DbSet<HotelGuest> HotelGuests { get; set; }

    // Booking
    public DbSet<HotelBooking> HotelBookings { get; set; }
    public DbSet<HotelBookingRoom> HotelBookingRooms { get; set; }
    public DbSet<HotelBookingService> HotelBookingServices { get; set; }

    // Room Forecast (availability calendar)
    public DbSet<HotelRoomForecast> HotelRoomForecasts { get; set; }

    // Vehicles
    public DbSet<HotelVehicle> HotelVehicles { get; set; }
    public DbSet<HotelVehicleRental> HotelVehicleRentals { get; set; }

    // Services catalog
    public DbSet<HotelService> HotelServices { get; set; }

    // PMS Rooms (sync from PMS)
    public DbSet<PmsRoom> PmsRooms { get; set; }

    // Finance
    public DbSet<HotelInvoice> HotelInvoices { get; set; }

    // Pricing
    public DbSet<HotelSeasonalPricing> HotelSeasonalPricings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // HotelBeds
        modelBuilder.Entity<HotelBed>(e => {
            e.HasIndex(x => new { x.HotelCode, x.RoomNo, x.BedCode }).IsUnique();
        });

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

        // HotelRoomForecast - composite unique
        modelBuilder.Entity<HotelRoomForecast>(e => {
            e.HasIndex(x => new { x.HotelCode, x.RoomNo, x.BedCode, x.ForecastDate }).IsUnique();
            e.HasIndex(x => new { x.HotelCode, x.ForecastDate });
        });

        // HotelVehicles
        modelBuilder.Entity<HotelVehicle>(e => {
            e.HasIndex(x => new { x.HotelCode, x.VehicleCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // HotelVehicleRentals
        modelBuilder.Entity<HotelVehicleRental>(e => {
            e.HasIndex(x => new { x.HotelCode, x.RentalCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // HotelServices
        modelBuilder.Entity<HotelService>(e => {
            e.HasIndex(x => new { x.HotelCode, x.ServiceCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // HotelInvoices
        modelBuilder.Entity<HotelInvoice>(e => {
            e.HasIndex(x => new { x.HotelCode, x.InvoiceCode }).IsUnique();
            e.HasQueryFilter(x => !x.IsDeleted);
        });

        // PmsRoom — no soft delete filter (sync table)
        modelBuilder.Entity<PmsRoom>(e => {
            e.HasIndex(x => new { x.HotelCode, x.So });
        });
    }
}
