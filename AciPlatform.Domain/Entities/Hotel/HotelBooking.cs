using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelBookings")]
public class HotelBooking
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string BookingCode { get; set; } = string.Empty;
    [MaxLength(10)] public string BookingType { get; set; } = "FIT"; // FIT / GIT
    public int? GuestId { get; set; }
    [Required, MaxLength(200)] public string GuestName { get; set; } = string.Empty;
    [MaxLength(20)] public string? GuestPhone { get; set; }
    [MaxLength(50)] public string? GuestIdCard { get; set; }
    [MaxLength(100)] public string? Nationality { get; set; } = "Việt Nam";
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NightCount { get; set; } = 1;
    public decimal RoomPrice { get; set; } = 0;
    public decimal ServicePrice { get; set; } = 0;
    public decimal VehiclePrice { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public decimal PaidAmount { get; set; } = 0;
    public decimal DepositAmount { get; set; } = 0;
    [MaxLength(20)] public string Status { get; set; } = "CONFIRMED";
    [MaxLength(50)] public string Source { get; set; } = "DIRECT";
    [MaxLength(200)] public string? GroupName { get; set; }
    public int GroupSize { get; set; } = 1;
    [MaxLength(1000)] public string? Notes { get; set; }
    [MaxLength(500)] public string? SpecialRequests { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? CheckInActual { get; set; }
    public DateTime? CheckOutActual { get; set; }
    public DateTime? CancelledAt { get; set; }
    [MaxLength(500)] public string? CancelReason { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    // Navigation
    public ICollection<HotelBookingRoom> Rooms { get; set; } = new List<HotelBookingRoom>();
    public ICollection<HotelBookingService> Services { get; set; } = new List<HotelBookingService>();
}

[Table("HotelBookingRooms")]
public class HotelBookingRoom
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public int BookingId { get; set; }
    [MaxLength(20)] public string RoomNo { get; set; } = string.Empty;
    [MaxLength(20)] public string? BedCode { get; set; }
    [MaxLength(50)] public string? RoomType { get; set; }
    [MaxLength(200)] public string? GuestName { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NightCount { get; set; } = 1;
    public decimal PricePerNight { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
    [MaxLength(20)] public string Status { get; set; } = "BOOKED";
    [MaxLength(500)] public string? Notes { get; set; }
}

[Table("HotelBookingServices")]
public class HotelBookingService
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public int BookingId { get; set; }
    [MaxLength(50)] public string ServiceCode { get; set; } = string.Empty;
    [MaxLength(200)] public string? ServiceName { get; set; }
    [MaxLength(50)] public string? Category { get; set; }
    public decimal Quantity { get; set; } = 1;
    [MaxLength(50)] public string? Unit { get; set; }
    public decimal UnitPrice { get; set; } = 0;
    public decimal TotalPrice { get; set; } = 0;
    public DateOnly? ServiceDate { get; set; }
    [MaxLength(500)] public string? Notes { get; set; }
}
