using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelGuests")]
public class HotelGuest
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [MaxLength(50)] public string? GuestCode { get; set; }
    [Required, MaxLength(200)] public string FullName { get; set; } = string.Empty;
    [MaxLength(20)] public string? Phone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(50)] public string? IdCard { get; set; }
    [MaxLength(20)] public string IdType { get; set; } = "CCCD";
    [MaxLength(100)] public string? Nationality { get; set; } = "Việt Nam";
    public DateOnly? DateOfBirth { get; set; }
    [MaxLength(10)] public string? Gender { get; set; }
    [MaxLength(500)] public string? Address { get; set; }
    [MaxLength(50)] public string? PreferRoomType { get; set; }
    [MaxLength(50)] public string? PreferVehicle { get; set; }
    [MaxLength(1000)] public string? Notes { get; set; }
    public int TotalVisits { get; set; } = 0;
    public decimal TotalSpend { get; set; } = 0;
    public DateOnly? LastVisitDate { get; set; }
    [MaxLength(50)] public string Source { get; set; } = "DIRECT";
    public bool IsVIP { get; set; } = false;
    public bool IsBlacklisted { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}
