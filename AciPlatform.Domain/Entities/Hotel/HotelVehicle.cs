using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelVehicles")]
public class HotelVehicle
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string VehicleCode { get; set; } = string.Empty;
    [MaxLength(20)] public string? BienSo { get; set; }
    [Required, MaxLength(200)] public string TenXe { get; set; } = string.Empty;
    [MaxLength(50)] public string VehicleType { get; set; } = "MOTORBIKE_MANUAL";
    [MaxLength(50)] public string? ServiceCode { get; set; }
    [MaxLength(100)] public string? Brand { get; set; }
    [MaxLength(100)] public string? Model { get; set; }
    [MaxLength(50)] public string? Color { get; set; }
    public int? YearMade { get; set; }
    public decimal PricePerDay { get; set; } = 0;
    public decimal DepositRequired { get; set; } = 0;
    public int FuelLevel { get; set; } = 100;
    [MaxLength(20)] public string Condition { get; set; } = "GOOD";
    [MaxLength(20)] public string Status { get; set; } = "AVAILABLE";
    [MaxLength(500)] public string? ImageUrl { get; set; }
    [MaxLength(500)] public string? Notes { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}

[Table("HotelVehicleRentals")]
public class HotelVehicleRental
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string RentalCode { get; set; } = string.Empty;
    public int? BookingId { get; set; }
    [Required, MaxLength(50)] public string VehicleCode { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string GuestName { get; set; } = string.Empty;
    [MaxLength(20)] public string? GuestPhone { get; set; }
    [MaxLength(50)] public string? GuestIdCard { get; set; }
    public DateTime RentFrom { get; set; }
    public DateTime RentTo { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal TotalDays { get; set; } = 1;
    public decimal PricePerDay { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public decimal DepositAmount { get; set; } = 0;
    public decimal DepositReturned { get; set; } = 0;
    public decimal DamageFee { get; set; } = 0;
    public int FuelLevelOut { get; set; } = 100;
    public int? FuelLevelIn { get; set; }
    [MaxLength(20)] public string ConditionOut { get; set; } = "GOOD";
    [MaxLength(20)] public string? ConditionIn { get; set; }
    [MaxLength(500)] public string? DamageNotes { get; set; }
    [MaxLength(20)] public string Status { get; set; } = "ACTIVE";
    public decimal PaidAmount { get; set; } = 0;
    [MaxLength(500)] public string? Notes { get; set; }
    public int? CreatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}
