using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelTours")]
public class HotelTour
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string TourCode { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string TourName { get; set; } = string.Empty;
    [MaxLength(200)] public string? TourNameEN { get; set; }
    [MaxLength(50)] public string TourType { get; set; } = "DAY_TRIP";
    public int DurationDays { get; set; } = 1;
    public int DurationNights { get; set; } = 0;
    public int MaxPerson { get; set; } = 10;
    public int MinPerson { get; set; } = 1;
    public decimal PricePerPerson { get; set; } = 0;
    public decimal GroupPrice { get; set; } = 0;
    public int GroupDiscountFrom { get; set; } = 5;
    [MaxLength(2000)] public string? Highlights { get; set; }   // JSON array
    [MaxLength(4000)] public string? Itinerary { get; set; }    // JSON
    [MaxLength(1000)] public string? Inclusions { get; set; }   // JSON array
    [MaxLength(1000)] public string? Exclusions { get; set; }   // JSON array
    [MaxLength(500)] public string? MeetingPoint { get; set; }
    [MaxLength(20)] public string Difficulty { get; set; } = "EASY";
    [MaxLength(500)] public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public int SortOrder { get; set; } = 0;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    // Navigation
    public ICollection<HotelTourSchedule> Schedules { get; set; } = new List<HotelTourSchedule>();
}

[Table("HotelTourGuides")]
public class HotelTourGuide
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(20)] public string? Phone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(200)] public string? Languages { get; set; }     // JSON: ["vi","en"]
    [MaxLength(200)] public string? Speciality { get; set; }    // Loop/Trek/Cultural/Car
    public bool IsFreelance { get; set; } = false;
    public decimal DailyRate { get; set; } = 0;
    [MaxLength(1000)] public string? Bio { get; set; }
    [MaxLength(500)] public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}

[Table("HotelTourSchedules")]
public class HotelTourSchedule
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [MaxLength(50)] public string TourCode { get; set; } = string.Empty;
    public DateOnly TourDate { get; set; }
    public int? GuideId { get; set; }
    public int MaxSlots { get; set; } = 10;
    public int BookedSlots { get; set; } = 0;
    public decimal? PriceOverride { get; set; }
    [MaxLength(20)] public string Status { get; set; } = "OPEN";   // OPEN/FULL/CANCELLED
    [MaxLength(500)] public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}

[Table("HotelGroupMembers")]
public class HotelGroupMember
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public int MemberNo { get; set; } = 1;
    [Required, MaxLength(200)] public string GuestName { get; set; } = string.Empty;
    [MaxLength(20)] public string? GuestPhone { get; set; }
    [MaxLength(50)] public string? GuestIdCard { get; set; }
    [MaxLength(100)] public string? Nationality { get; set; } = "Việt Nam";
    [MaxLength(20)] public string? RoomNo { get; set; }
    [MaxLength(20)] public string? BedCode { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    [MaxLength(20)] public string Status { get; set; } = "PENDING";
    [MaxLength(500)] public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
