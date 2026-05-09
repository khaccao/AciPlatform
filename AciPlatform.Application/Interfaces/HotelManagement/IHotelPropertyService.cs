namespace AciPlatform.Application.Interfaces.HotelManagement;

// ── Property DTO ────────────────────────────────────────────
public class HotelPropertyDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? HotelType { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public int StarRating { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public string? Currency { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; }
}

public class UpsertPropertyRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string? HotelType { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public int StarRating { get; set; }
    public string? CheckInTime { get; set; }
    public string? CheckOutTime { get; set; }
    public string? Currency { get; set; }
    public string? LogoUrl { get; set; }
    public bool IsActive { get; set; } = true;
}

// ── Area Tree DTOs ──────────────────────────────────────────
public class AreaTypeDto
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? GroupCode { get; set; }
    public string? Descriptions { get; set; }
}

public class AreaDto
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public string? AreaCode { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public string? AreaType { get; set; }
    public string? Color { get; set; }
    public string? AreaDescription { get; set; }
    public bool IsActive { get; set; }
    public List<AreaDto> Children { get; set; } = new();   // Tree children
    public List<ElementDto> Elements { get; set; } = new();
    public int RoomCount { get; set; }
}

public class ElementDto
{
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Alias { get; set; }
    public string Type { get; set; } = "ROOM";
    public int? Capacity { get; set; }
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsOccupied { get; set; }
}

public class UpsertAreaRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public string? AreaCode { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public string? AreaType { get; set; }
    public string? Color { get; set; }
    public string? AreaDescription { get; set; }
    public bool IsActive { get; set; } = true;
}

// ── RoomType DTOs ───────────────────────────────────────────
public class RoomTypeDto
{
    public int Id { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public string? Ma { get; set; }
    public string? Ten { get; set; }
    public decimal? DonGia { get; set; }
    public int? MaxPerson { get; set; }
    public int? SoLuong { get; set; }
    public int? FlagType { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Amenities { get; set; }
    public bool IsActive { get; set; }
}

public class UpsertRoomTypeRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string Ma { get; set; } = string.Empty;
    public string Ten { get; set; } = string.Empty;
    public decimal DonGia { get; set; } = 0;
    public int MaxPerson { get; set; } = 2;
    public int SoLuong { get; set; } = 0;
    public int FlagType { get; set; } = 1;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? Amenities { get; set; }
    public bool IsActive { get; set; } = true;
}

// ── Room CRUD DTOs ──────────────────────────────────────────
public class RoomDetailDto
{
    public int Id { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public string? So { get; set; }
    public string? Ma { get; set; }
    public string? RoomTypeName { get; set; }
    public string? Ten { get; set; }
    public string? Floor { get; set; }
    public string? KhuVucCode { get; set; }
    public int? MaxPerson { get; set; }
    public decimal? BasePrice { get; set; }
    public string? Status { get; set; }
    public int? CleanDirty { get; set; }
    public int? Inspected { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public List<BedStatusDto> Beds { get; set; } = new();
}

public class UpsertRoomRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string So { get; set; } = string.Empty;        // Room number
    public string Ma { get; set; } = string.Empty;        // RoomType code
    public string? Ten { get; set; }                       // Room name
    public string? Floor { get; set; }
    public string? KhuVucCode { get; set; }               // Area code
    public int? AreaId { get; set; }
    public int? MaxPerson { get; set; }
    public decimal? BasePrice { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
}

// ── Property Full Tree ──────────────────────────────────────
public class PropertyTreeDto
{
    public HotelPropertyDto Property { get; set; } = new();
    public List<AreaDto> Areas { get; set; } = new();      // Tree: Area → Children → Rooms/Elements
    public List<RoomDetailDto> Rooms { get; set; } = new();
    public int TotalRooms { get; set; }
    public int PrivateRooms { get; set; }
    public int DormRooms { get; set; }
    public int TotalBeds { get; set; }
}

public class SettingDto
{
    public string SettingKey { get; set; } = string.Empty;
    public string? SettingValue { get; set; }
    public string? Description { get; set; }
}

// ── Interface ────────────────────────────────────────────────
public interface IHotelPropertyService
{
    // ── Hotel Properties ──────────────────────────────────────
    Task<List<HotelPropertyDto>> GetAllPropertiesAsync();
    Task<HotelPropertyDto?> GetPropertyByCodeAsync(string hotelCode);
    Task<HotelPropertyDto> UpsertPropertyAsync(UpsertPropertyRequest req);
    Task DeletePropertyAsync(string hotelCode);

    // ── Full Property Tree (Property → Areas → Rooms → Beds) ──
    Task<PropertyTreeDto> GetPropertyTreeAsync(string hotelCode);

    // ── Area Types ─────────────────────────────────────────────
    Task<List<AreaTypeDto>> GetAreaTypesAsync(string hotelCode);
    Task<AreaTypeDto> UpsertAreaTypeAsync(string hotelCode, string code, string name, string? group, string? desc);
    Task DeleteAreaTypeAsync(int id);

    // ── Areas (Floors / Buildings / Wings) ────────────────────
    Task<List<AreaDto>> GetAreasTreeAsync(string hotelCode);
    Task<AreaDto?> GetAreaByIdAsync(int id);
    Task<AreaDto> CreateAreaAsync(UpsertAreaRequest req);
    Task<AreaDto> UpdateAreaAsync(int id, UpsertAreaRequest req);
    Task DeleteAreaAsync(int id);

    // ── Elements (Rooms in map) ───────────────────────────────
    Task<List<ElementDto>> GetElementsByAreaAsync(int areaId);
    Task<ElementDto> UpsertElementAsync(int areaId, string name, string type, int? capacity, string? color);
    Task DeleteElementAsync(int id);

    // ── Room Types ─────────────────────────────────────────────
    Task<List<RoomTypeDto>> GetRoomTypesAsync(string hotelCode);
    Task<RoomTypeDto?> GetRoomTypeByCodeAsync(string hotelCode, string ma);
    Task<RoomTypeDto> UpsertRoomTypeAsync(UpsertRoomTypeRequest req);
    Task DeleteRoomTypeAsync(int id);

    // ── Rooms (PMS_Rooms full CRUD) ────────────────────────────
    Task<List<RoomDetailDto>> GetRoomsAsync(string hotelCode, string? floor = null, string? roomType = null);
    Task<RoomDetailDto?> GetRoomByNumberAsync(string hotelCode, string roomNo);
    Task<RoomDetailDto> CreateRoomAsync(UpsertRoomRequest req);
    Task<RoomDetailDto> UpdateRoomAsync(int id, UpsertRoomRequest req);
    Task DeleteRoomAsync(int id);

    // ── Settings ───────────────────────────────────────────────
    Task<List<SettingDto>> GetSettingsAsync(string hotelCode);
    Task<SettingDto> UpsertSettingAsync(string hotelCode, string key, string value, string? desc);
    Task DeleteSettingAsync(string hotelCode, string key);
}
