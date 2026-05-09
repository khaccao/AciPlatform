using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelRoomForecast")]
public class HotelRoomForecast
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [MaxLength(20)] public string RoomNo { get; set; } = string.Empty;
    [MaxLength(20)] public string? BedCode { get; set; }
    public DateOnly ForecastDate { get; set; }
    public int? BookingId { get; set; }
    [MaxLength(20)] public string BlockType { get; set; } = "BOOKING"; // BOOKING/MAINTENANCE/HOLD
    [MaxLength(200)] public string? BlockNote { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}

[Table("HotelSeasonalPricing")]
public class HotelSeasonalPricing
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string SeasonName { get; set; } = string.Empty;
    [MaxLength(20)] public string SeasonType { get; set; } = "MID"; // HIGH/MID/LOW
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal PriceMultiplier { get; set; } = 1.0m;
    public bool IsActive { get; set; } = true;
    [MaxLength(500)] public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}

[Table("HotelInvoices")]
public class HotelInvoice
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string InvoiceCode { get; set; } = string.Empty;
    public int? BookingId { get; set; }
    [MaxLength(200)] public string? GuestName { get; set; }
    public decimal RoomAmount { get; set; } = 0;
    public decimal ServiceAmount { get; set; } = 0;
    public decimal VehicleAmount { get; set; } = 0;
    public decimal DiscountAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public decimal PaidAmount { get; set; } = 0;
    [MaxLength(50)] public string PaymentMethod { get; set; } = "CASH";
    [MaxLength(20)] public string Status { get; set; } = "UNPAID"; // UNPAID/PARTIAL/PAID
    [MaxLength(500)] public string? Notes { get; set; }
    public int? IssuedBy { get; set; }
    public DateTime IssuedDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}

[Table("HotelServices")]
public class HotelService
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string ServiceCode { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string ServiceName { get; set; } = string.Empty;
    [MaxLength(200)] public string? ServiceNameEN { get; set; }
    [Required, MaxLength(50)] public string Category { get; set; } = "OTHER"; // VEHICLE/TOUR/FOOD/OTHER
    [MaxLength(50)] public string? SubCategory { get; set; }
    [MaxLength(1000)] public string? Description { get; set; }
    [MaxLength(50)] public string? Unit { get; set; }
    public decimal UnitPrice { get; set; } = 0;
    [MaxLength(10)] public string Currency { get; set; } = "VND";
    public decimal TyLeSC { get; set; } = 0;
    public decimal TyLeVAT { get; set; } = 0;
    public int? MaxQuantity { get; set; }
    public bool IsAvailable { get; set; } = true;
    [MaxLength(500)] public string? ImageUrl { get; set; }
    public int SortOrder { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}

[Table("PMS_Rooms")]
public class PmsRoom
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public int? PmsRoomId { get; set; }
    [MaxLength(20)] public string? So { get; set; }
    public int? Loai { get; set; }
    [MaxLength(50)] public string? Ma { get; set; }
    [MaxLength(200)] public string? Ten { get; set; }
    [MaxLength(10)] public string? Floor { get; set; }
    [MaxLength(50)] public string? KhuVucCode { get; set; }
    [MaxLength(50)] public string? BuildingID { get; set; }
    public int? AreaId { get; set; }
    public int? SachBan { get; set; }
    public int? CleanDirty { get; set; }
    public int? Inspected { get; set; }
    public int? TinhTrang { get; set; }
    [MaxLength(20)] public string? Status { get; set; }
    public int? MaxPerson { get; set; }
    public decimal? BasePrice { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    [MaxLength(500)] public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime? SyncDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

