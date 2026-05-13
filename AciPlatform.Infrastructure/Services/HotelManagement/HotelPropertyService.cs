using AciPlatform.Application.Interfaces.HotelManagement;
using AciPlatform.Domain.Entities.Hotel;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Services.HotelManagement;

public class HotelPropertyService : IHotelPropertyService
{
    private readonly HotelDbContext _db;
    public HotelPropertyService(HotelDbContext db) => _db = db;

    public async Task<List<HotelPropertyDto>> GetAllPropertiesAsync()
        => await _db.HotelProperties.OrderBy(p => p.Name).Select(p => ToPropertyDto(p)).ToListAsync();

    public async Task<HotelPropertyDto?> GetPropertyByCodeAsync(string code)
    { var p = await _db.HotelProperties.FirstOrDefaultAsync(x => x.Code == code); return p == null ? null : ToPropertyDto(p); }

    public async Task<HotelPropertyDto> UpsertPropertyAsync(UpsertPropertyRequest req)
    {
        var p = await _db.HotelProperties.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Code == req.Code);
        if (p == null) { p = new HotelProperty { Code = req.Code }; _db.HotelProperties.Add(p); }
        p.Name = req.Name; p.ShortName = req.ShortName; p.HotelType = req.HotelType;
        p.Address = req.Address; p.Phone = req.Phone; p.Email = req.Email;
        p.Website = req.Website; p.Description = req.Description; p.StarRating = req.StarRating;
        p.CheckInTime = req.CheckInTime; p.CheckOutTime = req.CheckOutTime;
        p.Currency = req.Currency; p.LogoUrl = req.LogoUrl; p.IsActive = req.IsActive;
        p.IsDeleted = false; p.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync(); return ToPropertyDto(p);
    }

    public async Task DeletePropertyAsync(string code)
    { var p = await _db.HotelProperties.FirstOrDefaultAsync(x => x.Code == code);
      if (p != null) { p.IsDeleted = true; p.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<PropertyTreeDto> GetPropertyTreeAsync(string hotelCode)
    {
        var prop = await _db.HotelProperties.FirstOrDefaultAsync(x => x.Code == hotelCode);
        var rooms = await GetRoomsAsync(hotelCode);
        var areas = await GetAreasTreeAsync(hotelCode);
        var beds = await _db.HotelBeds.Where(b => b.HotelCode == hotelCode && b.IsActive).ToListAsync();
        return new PropertyTreeDto
        {
            Property = prop != null ? ToPropertyDto(prop) : new HotelPropertyDto { Code = hotelCode },
            Areas = areas, Rooms = rooms, TotalRooms = rooms.Count,
            PrivateRooms = rooms.Count(r => r.Ma == "KHEPKIN"),
            DormRooms = rooms.Count(r => r.Ma?.StartsWith("TAPTHE") == true),
            TotalBeds = beds.Count
        };
    }

    public async Task<List<AreaTypeDto>> GetAreaTypesAsync(string hotelCode)
        => await _db.HotelAreaTypes.Where(t => t.HotelCode == hotelCode)
            .Select(t => new AreaTypeDto { Id = t.Id, Guid = t.Guid, HotelCode = t.HotelCode, Code = t.Code, Name = t.Name, GroupCode = t.GroupCode, Descriptions = t.Descriptions })
            .ToListAsync();

    public async Task<AreaTypeDto> UpsertAreaTypeAsync(string hotelCode, string code, string name, string? group, string? desc)
    {
        var prop = await _db.HotelProperties.FirstOrDefaultAsync(x => x.Code == hotelCode)
            ?? throw new InvalidOperationException($"Hotel '{hotelCode}' not found.");
        var t = await _db.HotelAreaTypes.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.Code == code);
        if (t == null) { t = new HotelAreaType { HotelCode = hotelCode, HotelGuid = prop.Guid, Code = code }; _db.HotelAreaTypes.Add(t); }
        t.Name = name; t.GroupCode = group; t.Descriptions = desc;
        await _db.SaveChangesAsync();
        return new AreaTypeDto { Id = t.Id, Guid = t.Guid, HotelCode = t.HotelCode, Code = t.Code, Name = t.Name };
    }

    public async Task DeleteAreaTypeAsync(int id)
    { var t = await _db.HotelAreaTypes.FindAsync(id); if (t != null) { _db.HotelAreaTypes.Remove(t); await _db.SaveChangesAsync(); } }

    public async Task<List<AreaDto>> GetAreasTreeAsync(string hotelCode)
    {
        var all = await _db.HotelAreas.Where(a => a.HotelCode == hotelCode && a.IsActive)
            .OrderBy(a => a.AreaCode).ToListAsync();
        var rooms = await _db.PmsRooms.Where(r => r.HotelCode == hotelCode && r.IsActive).ToListAsync();
        var elements = await _db.HotelElements.Where(e => e.HotelCode == hotelCode && e.IsActive).ToListAsync();
        return all.Where(a => a.ParentId == null).Select(r => BuildAreaTree(r, all, rooms, elements)).ToList();
    }

    private AreaDto BuildAreaTree(HotelArea area, List<HotelArea> all, List<PmsRoom> rooms, List<HotelElement>? elements = null)
    {
        var children = all.Where(a => a.ParentId == area.Id).ToList();
        var areaRooms = rooms.Where(r => r.KhuVucCode == area.AreaCode).ToList();
        var areaElements = elements?.Where(e => e.AreaId == area.Id).ToList() ?? area.Elements.ToList();
        return new AreaDto
        {
            Id = area.Id, Guid = area.Guid, HotelCode = area.HotelCode, ParentId = area.ParentId,
            AreaCode = area.AreaCode, AreaName = area.AreaName, AreaType = area.AreaType,
            Color = area.Color, AreaDescription = area.AreaDescription, IsActive = area.IsActive,
            RoomCount = areaRooms.Count,
            Children = children.Select(c => BuildAreaTree(c, all, rooms, elements)).ToList(),
            Elements = areaElements.Select(e => new ElementDto { Id = e.Id, Guid = e.Guid, Name = e.Name,
                Alias = e.Alias, Type = e.Type, Capacity = e.Capacity, Color = e.Color, Icon = e.Icon, IsOccupied = e.IsOccupied }).ToList()
        };
    }

    public async Task<AreaDto?> GetAreaByIdAsync(int id)
    { var a = await _db.HotelAreas.FirstOrDefaultAsync(x => x.Id == id);
      if (a == null) return null;
      var elements = await _db.HotelElements.Where(e => e.AreaId == id && e.IsActive).ToListAsync();
      return BuildAreaTree(a, new List<HotelArea>(), new List<PmsRoom>(), elements); }

    public async Task<AreaDto> CreateAreaAsync(UpsertAreaRequest req)
    {
        var prop = await _db.HotelProperties.FirstOrDefaultAsync(x => x.Code == req.HotelCode)
            ?? throw new InvalidOperationException($"Hotel '{req.HotelCode}' not found.");
        var a = new HotelArea { HotelCode = req.HotelCode, HotelGuid = prop.Guid, ParentId = req.ParentId,
            AreaCode = req.AreaCode, AreaName = req.AreaName, AreaType = req.AreaType,
            Color = req.Color, AreaDescription = req.AreaDescription, IsActive = req.IsActive };
        if (req.ParentId.HasValue)
        { var parent = await _db.HotelAreas.FindAsync(req.ParentId.Value); if (parent != null) a.ParentGuid = parent.Guid; }
        _db.HotelAreas.Add(a); await _db.SaveChangesAsync();
        return BuildAreaTree(a, new List<HotelArea>(), new List<PmsRoom>());
    }

    public async Task<AreaDto> UpdateAreaAsync(int id, UpsertAreaRequest req)
    {
        var a = await _db.HotelAreas.FindAsync(id) ?? throw new InvalidOperationException("Area not found.");
        a.AreaCode = req.AreaCode; a.AreaName = req.AreaName; a.AreaType = req.AreaType;
        a.Color = req.Color; a.AreaDescription = req.AreaDescription; a.IsActive = req.IsActive;
        a.ParentId = req.ParentId; a.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return BuildAreaTree(a, new List<HotelArea>(), new List<PmsRoom>());
    }

    public async Task DeleteAreaAsync(int id)
    { var a = await _db.HotelAreas.FindAsync(id);
      if (a != null) { a.IsActive = false; a.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<List<ElementDto>> GetElementsByAreaAsync(int areaId)
        => await _db.HotelElements.Where(e => e.AreaId == areaId && e.IsActive)
            .Select(e => new ElementDto { Id = e.Id, Guid = e.Guid, Name = e.Name, Alias = e.Alias,
                Type = e.Type, Capacity = e.Capacity, Color = e.Color, Icon = e.Icon, IsOccupied = e.IsOccupied })
            .ToListAsync();

    public async Task<ElementDto> UpsertElementAsync(int areaId, string name, string type, int? capacity, string? color)
    {
        var area = await _db.HotelAreas.FindAsync(areaId) ?? throw new InvalidOperationException("Area not found.");
        var e = new HotelElement { HotelCode = area.HotelCode, HotelGuid = area.HotelGuid,
            AreaId = areaId, AreaGuid = area.Guid, Name = name, Type = type, Capacity = capacity, Color = color };
        _db.HotelElements.Add(e); await _db.SaveChangesAsync();
        return new ElementDto { Id = e.Id, Guid = e.Guid, Name = e.Name, Type = e.Type, Capacity = e.Capacity };
    }

    public async Task DeleteElementAsync(int id)
    { var e = await _db.HotelElements.FindAsync(id);
      if (e != null) { e.IsActive = false; e.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<List<RoomTypeDto>> GetRoomTypesAsync(string hotelCode)
        => await _db.PmsRoomTypes.Where(t => t.HotelCode == hotelCode).OrderBy(t => t.FlagType).ThenBy(t => t.Ma)
            .Select(t => ToRoomTypeDto(t)).ToListAsync();

    public async Task<RoomTypeDto?> GetRoomTypeByCodeAsync(string hotelCode, string ma)
    { var t = await _db.PmsRoomTypes.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.Ma == ma);
      return t == null ? null : ToRoomTypeDto(t); }

    public async Task<RoomTypeDto> UpsertRoomTypeAsync(UpsertRoomTypeRequest req)
    {
        var t = await _db.PmsRoomTypes.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.HotelCode == req.HotelCode && x.Ma == req.Ma);
        if (t == null) { t = new PmsRoomType { HotelCode = req.HotelCode, Ma = req.Ma }; _db.PmsRoomTypes.Add(t); }
        t.Ten = req.Ten; t.DonGia = req.DonGia; t.MaxPerson = req.MaxPerson; t.SoLuong = req.SoLuong;
        t.FlagType = req.FlagType; t.Description = req.Description; t.ImageUrl = req.ImageUrl;
        t.Amenities = req.Amenities; t.IsActive = req.IsActive; t.IsDeleted = false; t.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync(); return ToRoomTypeDto(t);
    }

    public async Task DeleteRoomTypeAsync(int id)
    { var t = await _db.PmsRoomTypes.FindAsync(id);
      if (t != null) { t.IsDeleted = true; t.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<List<RoomDetailDto>> GetRoomsAsync(string hotelCode, string? floor = null, string? roomType = null)
    {
        var q = _db.PmsRooms.Where(r => r.HotelCode == hotelCode && r.IsActive);
        if (floor != null) q = q.Where(r => r.Floor == floor);
        if (roomType != null) q = q.Where(r => r.Ma == roomType);
        var rooms = await q.OrderBy(r => r.Floor).ThenBy(r => r.So).ToListAsync();
        var beds = await _db.HotelBeds.Where(b => b.HotelCode == hotelCode && b.IsActive).ToListAsync();
        var rts = await _db.PmsRoomTypes.Where(t => t.HotelCode == hotelCode).ToListAsync();
        return rooms.Select(r => ToRoomDto(r, beds, rts)).ToList();
    }

    public async Task<RoomDetailDto?> GetRoomByNumberAsync(string hotelCode, string roomNo)
    {
        var r = await _db.PmsRooms.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.So == roomNo && x.IsActive);
        if (r == null) return null;
        var beds = await _db.HotelBeds.Where(b => b.HotelCode == hotelCode && b.RoomNo == roomNo && b.IsActive).ToListAsync();
        var rts = await _db.PmsRoomTypes.Where(t => t.HotelCode == hotelCode).ToListAsync();
        return ToRoomDto(r, beds, rts);
    }

    public async Task<RoomDetailDto> CreateRoomAsync(UpsertRoomRequest req)
    {
        var r = new PmsRoom { HotelCode = req.HotelCode, So = req.So, Ma = req.Ma, Ten = req.Ten,
            Floor = req.Floor, KhuVucCode = req.KhuVucCode, AreaId = req.AreaId,
            MaxPerson = req.MaxPerson ?? 2, BasePrice = req.BasePrice ?? 0,
            Description = req.Description, ImageUrl = req.ImageUrl,
            IsActive = req.IsActive, Status = "VACANT", CleanDirty = 1, Inspected = 0, TinhTrang = 0 };
        _db.PmsRooms.Add(r); await _db.SaveChangesAsync();
        return ToRoomDto(r, new List<HotelBed>(), await _db.PmsRoomTypes.Where(t => t.HotelCode == req.HotelCode).ToListAsync());
    }

    public async Task<RoomDetailDto> UpdateRoomAsync(int id, UpsertRoomRequest req)
    {
        var r = await _db.PmsRooms.FindAsync(id) ?? throw new InvalidOperationException("Room not found.");
        r.So = req.So; r.Ma = req.Ma; r.Ten = req.Ten; r.Floor = req.Floor;
        r.KhuVucCode = req.KhuVucCode; r.AreaId = req.AreaId;
        if (req.MaxPerson.HasValue) r.MaxPerson = req.MaxPerson.Value;
        if (req.BasePrice.HasValue) r.BasePrice = req.BasePrice.Value;
        r.Description = req.Description; r.ImageUrl = req.ImageUrl; r.IsActive = req.IsActive; r.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        var beds = await _db.HotelBeds.Where(b => b.HotelCode == r.HotelCode && b.RoomNo == r.So).ToListAsync();
        var rts = await _db.PmsRoomTypes.Where(t => t.HotelCode == r.HotelCode).ToListAsync();
        return ToRoomDto(r, beds, rts);
    }

    public async Task DeleteRoomAsync(int id)
    { var r = await _db.PmsRooms.FindAsync(id);
      if (r != null) { r.IsActive = false; r.IsDeleted = true; r.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<List<SettingDto>> GetSettingsAsync(string hotelCode)
        => await _db.HotelSettings.Where(s => s.HotelCode == hotelCode)
            .Select(s => new SettingDto { SettingKey = s.SettingKey, SettingValue = s.SettingValue, Description = s.Description })
            .ToListAsync();

    public async Task<SettingDto> UpsertSettingAsync(string hotelCode, string key, string value, string? desc)
    {
        var s = await _db.HotelSettings.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.SettingKey == key);
        if (s == null) { s = new HotelSetting { HotelCode = hotelCode, SettingKey = key }; _db.HotelSettings.Add(s); }
        s.SettingValue = value; s.Description = desc; s.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return new SettingDto { SettingKey = key, SettingValue = value, Description = desc };
    }

    public async Task DeleteSettingAsync(string hotelCode, string key)
    { var s = await _db.HotelSettings.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.SettingKey == key);
      if (s != null) { _db.HotelSettings.Remove(s); await _db.SaveChangesAsync(); } }

    // Mappers
    private static HotelPropertyDto ToPropertyDto(HotelProperty p) => new()
    {
        Id = p.Id, Code = p.Code, Name = p.Name, ShortName = p.ShortName, HotelType = p.HotelType,
        Address = p.Address, Phone = p.Phone, Email = p.Email, Website = p.Website,
        Description = p.Description, StarRating = p.StarRating, CheckInTime = p.CheckInTime,
        CheckOutTime = p.CheckOutTime, Currency = p.Currency, LogoUrl = p.LogoUrl, IsActive = p.IsActive
    };

    private static RoomTypeDto ToRoomTypeDto(PmsRoomType t) => new()
    {
        Id = t.Id, HotelCode = t.HotelCode, Ma = t.Ma, Ten = t.Ten, DonGia = t.DonGia,
        MaxPerson = t.MaxPerson, SoLuong = t.SoLuong, FlagType = t.FlagType,
        Description = t.Description, ImageUrl = t.ImageUrl, Amenities = t.Amenities, IsActive = t.IsActive
    };

    private static RoomDetailDto ToRoomDto(PmsRoom r, List<HotelBed> allBeds, List<PmsRoomType> rts)
    {
        var rt = rts.FirstOrDefault(t => t.Ma == r.Ma);
        return new RoomDetailDto
        {
            Id = r.Id, HotelCode = r.HotelCode, So = r.So, Ma = r.Ma, RoomTypeName = rt?.Ten, Ten = r.Ten,
            Floor = r.Floor, KhuVucCode = r.KhuVucCode, MaxPerson = r.MaxPerson ?? rt?.MaxPerson,
            BasePrice = r.BasePrice ?? rt?.DonGia, Status = r.Status, CleanDirty = r.CleanDirty,
            Inspected = r.Inspected, Description = r.Description, ImageUrl = r.ImageUrl, IsActive = r.IsActive,
            Beds = allBeds.Where(b => b.RoomNo == r.So).OrderBy(b => b.SortOrder)
                .Select(b => new BedStatusDto { BedCode = b.BedCode, BedName = b.BedName, BedType = b.BedType, Status = b.Status }).ToList()
        };
    }
}
