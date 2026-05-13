using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Hotel;

[Table("HotelProperties")]
public class HotelProperty
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [Required, MaxLength(50)] public string Code { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(50)] public string? ShortName { get; set; }
    [MaxLength(500)] public string? LogoUrl { get; set; }
    [MaxLength(500)] public string? Address { get; set; }
    [MaxLength(50)] public string? Phone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    [MaxLength(200)] public string? Website { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
    [MaxLength(50)] public string? HotelType { get; set; } = "HOTEL";
    public int StarRating { get; set; } = 0;
    [MaxLength(10)] public string? CheckInTime { get; set; } = "14:00";
    [MaxLength(10)] public string? CheckOutTime { get; set; } = "12:00";
    [MaxLength(10)] public string? Currency { get; set; } = "VND";
    [MaxLength(1000)] public string? PmsConnectionString { get; set; }
    [MaxLength(100)] public string? PmsDbName { get; set; }
    [MaxLength(200)] public string? PmsIpAddress { get; set; }
    [MaxLength(50)] public string? DmsAppId { get; set; }
    [MaxLength(100)] public string? DmsAppSecret { get; set; }
    public bool IsLinkedToAciCompany { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
}

[Table("HotelAreaTypes")]
public class HotelAreaType
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public Guid HotelGuid { get; set; }
    [MaxLength(50)] public string? GroupCode { get; set; }
    [Required, MaxLength(50)] public string Code { get; set; } = string.Empty;
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(500)] public string? Descriptions { get; set; }
}

[Table("HotelAreas")]
public class HotelArea
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    public int? ParentId { get; set; }
    public Guid? ParentGuid { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public Guid HotelGuid { get; set; }
    [MaxLength(50)] public string? AreaCode { get; set; }
    [Required, MaxLength(200)] public string AreaName { get; set; } = string.Empty;
    [MaxLength(50)] public string? AreaType { get; set; }     // BUILDING / FLOOR / WING / ZONE
    public Guid? AreaTypeGuid { get; set; }
    [MaxLength(500)] public string? AreaAlias { get; set; }
    [MaxLength(500)] public string? AreaDescription { get; set; }
    [MaxLength(500)] public string? AreaAvatar { get; set; }
    [MaxLength(20)] public string? Color { get; set; }
    public int? PositionX { get; set; }
    public int? PositionY { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public long? DmsLockId { get; set; }
    [MaxLength(50)] public string? DmsHardwareId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    // Navigation
    public HotelArea? Parent { get; set; }
    public ICollection<HotelArea> Children { get; set; } = new List<HotelArea>();
    public ICollection<HotelElement> Elements { get; set; } = new List<HotelElement>();
}

[Table("HotelElements")]
public class HotelElement
{
    [Key] public int Id { get; set; }
    public Guid Guid { get; set; } = Guid.NewGuid();
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public Guid HotelGuid { get; set; }
    public int AreaId { get; set; }
    public Guid AreaGuid { get; set; }
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [MaxLength(200)] public string? Alias { get; set; }
    [MaxLength(50)] public string Type { get; set; } = "ROOM";  // ROOM / BED / LOCK / SENSOR
    public int? Capacity { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    public int PositionX { get; set; } = 0;
    public int PositionY { get; set; } = 0;
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int Rotation { get; set; } = 0;
    [MaxLength(20)] public string? Color { get; set; }
    [MaxLength(10)] public string? Icon { get; set; }
    [MaxLength(4000)] public string? Settings { get; set; }
    [MaxLength(20)] public string Status { get; set; } = "VC"; // VC, VD, OC, OD, EA, ED, ED/EA
    public bool IsActive { get; set; } = true;
    public bool IsOccupied { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    // Navigation
    public HotelArea? Area { get; set; }
}

[Table("PMS_RoomTypes")]
public class PmsRoomType
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    public int? PmsItemId { get; set; }
    [MaxLength(50)] public string? Ma { get; set; }
    [MaxLength(200)] public string? Ten { get; set; }
    public decimal? DonGia { get; set; }
    public int? MaxPerson { get; set; }
    public int? SoLuong { get; set; }
    public int? FlagType { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    [MaxLength(500)] public string? ImageUrl { get; set; }
    [MaxLength(1000)] public string? Amenities { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? SyncDate { get; set; }
}

[Table("HotelSettings")]
public class HotelSetting
{
    [Key] public int Id { get; set; }
    [MaxLength(50)] public string HotelCode { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string SettingKey { get; set; } = string.Empty;
    [MaxLength(2000)] public string? SettingValue { get; set; }
    [MaxLength(500)] public string? Description { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
