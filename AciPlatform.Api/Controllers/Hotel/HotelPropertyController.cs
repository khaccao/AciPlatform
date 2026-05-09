using AciPlatform.Application.Interfaces.HotelManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Hotel;

/// <summary>
/// Full Property Tree CRUD: Property → AreaTypes → Areas (Floors/Buildings) → Rooms → RoomTypes → Beds → Settings
/// Route: /api/hotel-property
/// </summary>
[Authorize]
[ApiController]
[Route("api/hotel-property")]
public class HotelPropertyController : ControllerBase
{
    private readonly IHotelPropertyService _svc;
    public HotelPropertyController(IHotelPropertyService svc) => _svc = svc;

    // ── Properties ────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllPropertiesAsync());

    [HttpGet("{hotelCode}")]
    public async Task<IActionResult> GetByCode(string hotelCode)
    { var p = await _svc.GetPropertyByCodeAsync(hotelCode); return p == null ? NotFound() : Ok(p); }

    /// GET /api/hotel-property/{hotelCode}/tree — Full tree: Areas→Rooms→Beds
    [HttpGet("{hotelCode}/tree")]
    public async Task<IActionResult> GetTree(string hotelCode)
        => Ok(await _svc.GetPropertyTreeAsync(hotelCode));

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertPropertyRequest req)
        => Ok(await _svc.UpsertPropertyAsync(req));

    [HttpDelete("{hotelCode}")]
    public async Task<IActionResult> Delete(string hotelCode)
    { await _svc.DeletePropertyAsync(hotelCode); return Ok(new { message = "Property deleted." }); }

    // ── Area Types ────────────────────────────────────────────

    [HttpGet("{hotelCode}/area-types")]
    public async Task<IActionResult> GetAreaTypes(string hotelCode)
        => Ok(await _svc.GetAreaTypesAsync(hotelCode));

    [HttpPost("{hotelCode}/area-types")]
    public async Task<IActionResult> UpsertAreaType(string hotelCode, [FromBody] AreaTypeRequest req)
        => Ok(await _svc.UpsertAreaTypeAsync(hotelCode, req.Code, req.Name, req.GroupCode, req.Descriptions));

    [HttpDelete("{hotelCode}/area-types/{id:int}")]
    public async Task<IActionResult> DeleteAreaType(int id)
    { await _svc.DeleteAreaTypeAsync(id); return Ok(new { message = "Area type deleted." }); }

    // ── Areas (Floors/Buildings) — Tree ───────────────────────

    /// GET /api/hotel-property/{hotelCode}/areas — Tree structure
    [HttpGet("{hotelCode}/areas")]
    public async Task<IActionResult> GetAreas(string hotelCode)
        => Ok(await _svc.GetAreasTreeAsync(hotelCode));

    [HttpGet("{hotelCode}/areas/{id:int}")]
    public async Task<IActionResult> GetArea(int id)
    { var a = await _svc.GetAreaByIdAsync(id); return a == null ? NotFound() : Ok(a); }

    [HttpPost("{hotelCode}/areas")]
    public async Task<IActionResult> CreateArea(string hotelCode, [FromBody] UpsertAreaRequest req)
    { req.HotelCode = hotelCode; return Ok(await _svc.CreateAreaAsync(req)); }

    [HttpPut("{hotelCode}/areas/{id:int}")]
    public async Task<IActionResult> UpdateArea(string hotelCode, int id, [FromBody] UpsertAreaRequest req)
    { req.HotelCode = hotelCode; return Ok(await _svc.UpdateAreaAsync(id, req)); }

    [HttpDelete("{hotelCode}/areas/{id:int}")]
    public async Task<IActionResult> DeleteArea(int id)
    { await _svc.DeleteAreaAsync(id); return Ok(new { message = "Area deleted." }); }

    // ── Elements (items inside area, e.g. for floor map) ─────

    [HttpGet("{hotelCode}/areas/{areaId:int}/elements")]
    public async Task<IActionResult> GetElements(int areaId)
        => Ok(await _svc.GetElementsByAreaAsync(areaId));

    [HttpPost("{hotelCode}/areas/{areaId:int}/elements")]
    public async Task<IActionResult> AddElement(int areaId, [FromBody] ElementRequest req)
        => Ok(await _svc.UpsertElementAsync(areaId, req.Name, req.Type, req.Capacity, req.Color));

    [HttpDelete("{hotelCode}/areas/{areaId:int}/elements/{id:int}")]
    public async Task<IActionResult> DeleteElement(int id)
    { await _svc.DeleteElementAsync(id); return Ok(new { message = "Element deleted." }); }

    // ── Room Types ────────────────────────────────────────────

    [HttpGet("{hotelCode}/room-types")]
    public async Task<IActionResult> GetRoomTypes(string hotelCode)
        => Ok(await _svc.GetRoomTypesAsync(hotelCode));

    [HttpGet("{hotelCode}/room-types/{ma}")]
    public async Task<IActionResult> GetRoomType(string hotelCode, string ma)
    { var t = await _svc.GetRoomTypeByCodeAsync(hotelCode, ma); return t == null ? NotFound() : Ok(t); }

    [HttpPost("{hotelCode}/room-types")]
    public async Task<IActionResult> UpsertRoomType(string hotelCode, [FromBody] UpsertRoomTypeRequest req)
    { req.HotelCode = hotelCode; return Ok(await _svc.UpsertRoomTypeAsync(req)); }

    [HttpDelete("{hotelCode}/room-types/{id:int}")]
    public async Task<IActionResult> DeleteRoomType(int id)
    { await _svc.DeleteRoomTypeAsync(id); return Ok(new { message = "Room type deleted." }); }

    // ── Rooms (PMS_Rooms full CRUD) ────────────────────────────

    [HttpGet("{hotelCode}/rooms")]
    public async Task<IActionResult> GetRooms(string hotelCode, [FromQuery] string? floor, [FromQuery] string? roomType)
        => Ok(await _svc.GetRoomsAsync(hotelCode, floor, roomType));

    [HttpGet("{hotelCode}/rooms/{roomNo}")]
    public async Task<IActionResult> GetRoom(string hotelCode, string roomNo)
    { var r = await _svc.GetRoomByNumberAsync(hotelCode, roomNo); return r == null ? NotFound() : Ok(r); }

    [HttpPost("{hotelCode}/rooms")]
    public async Task<IActionResult> CreateRoom(string hotelCode, [FromBody] UpsertRoomRequest req)
    { req.HotelCode = hotelCode; return Ok(await _svc.CreateRoomAsync(req)); }

    [HttpPut("{hotelCode}/rooms/{id:int}")]
    public async Task<IActionResult> UpdateRoom(string hotelCode, int id, [FromBody] UpsertRoomRequest req)
    { req.HotelCode = hotelCode; return Ok(await _svc.UpdateRoomAsync(id, req)); }

    [HttpDelete("{hotelCode}/rooms/{id:int}")]
    public async Task<IActionResult> DeleteRoom(int id)
    { await _svc.DeleteRoomAsync(id); return Ok(new { message = "Room deleted." }); }

    // ── Settings ──────────────────────────────────────────────

    [HttpGet("{hotelCode}/settings")]
    public async Task<IActionResult> GetSettings(string hotelCode)
        => Ok(await _svc.GetSettingsAsync(hotelCode));

    [HttpPost("{hotelCode}/settings")]
    public async Task<IActionResult> UpsertSetting(string hotelCode, [FromBody] UpsertSettingRequest req)
        => Ok(await _svc.UpsertSettingAsync(hotelCode, req.Key, req.Value, req.Description));

    [HttpDelete("{hotelCode}/settings/{key}")]
    public async Task<IActionResult> DeleteSetting(string hotelCode, string key)
    { await _svc.DeleteSettingAsync(hotelCode, key); return Ok(new { message = "Setting deleted." }); }
}

// Request models
public class AreaTypeRequest { public string Code { get; set; } = ""; public string Name { get; set; } = ""; public string? GroupCode { get; set; } public string? Descriptions { get; set; } }
public class ElementRequest { public string Name { get; set; } = ""; public string Type { get; set; } = "ROOM"; public int? Capacity { get; set; } public string? Color { get; set; } }
public class UpsertSettingRequest { public string Key { get; set; } = ""; public string Value { get; set; } = ""; public string? Description { get; set; } }

// ── Tour + Report Controllers ─────────────────────────────────

[Authorize]
[ApiController]
[Route("api/hotel-tours")]
public class HotelToursController : ControllerBase
{
    private readonly IHotelTourService _svc;
    public HotelToursController(IHotelTourService svc) => _svc = svc;

    [HttpGet("{hotelCode}")] public async Task<IActionResult> GetAll(string hotelCode, [FromQuery] string? tourType) => Ok(await _svc.GetToursAsync(hotelCode, tourType));
    [HttpGet("{hotelCode}/{tourCode}")] public async Task<IActionResult> GetByCode(string hotelCode, string tourCode) { var t = await _svc.GetTourByCodeAsync(hotelCode, tourCode); return t == null ? NotFound() : Ok(t); }
    [HttpPost] public async Task<IActionResult> Upsert([FromBody] UpsertTourRequest req) => Ok(await _svc.UpsertTourAsync(req));
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) { await _svc.DeleteTourAsync(id); return Ok(); }
    [HttpPatch("{hotelCode}/{tourCode}/toggle")] public async Task<IActionResult> Toggle(string hotelCode, string tourCode, [FromQuery] bool available) { await _svc.ToggleTourAvailabilityAsync(hotelCode, tourCode, available); return Ok(); }

    // Guides
    [HttpGet("{hotelCode}/guides")] public async Task<IActionResult> GetGuides(string hotelCode) => Ok(await _svc.GetGuidesAsync(hotelCode));
    [HttpPost("{hotelCode}/guides")] public async Task<IActionResult> UpsertGuide([FromBody] UpsertTourGuideRequest req) => Ok(await _svc.UpsertGuideAsync(req));
    [HttpDelete("{hotelCode}/guides/{id:int}")] public async Task<IActionResult> DeleteGuide(int id) { await _svc.DeleteGuideAsync(id); return Ok(); }

    // Schedules
    [HttpGet("{hotelCode}/schedules")] public async Task<IActionResult> GetSchedules(string hotelCode, [FromQuery] string? tourCode, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to) => Ok(await _svc.GetSchedulesAsync(hotelCode, tourCode, from, to));
    [HttpGet("{hotelCode}/schedules/available")] public async Task<IActionResult> GetAvailable(string hotelCode, [FromQuery] DateOnly date) => Ok(await _svc.GetAvailableSchedulesAsync(hotelCode, date));
    [HttpPost("{hotelCode}/schedules")] public async Task<IActionResult> UpsertSchedule([FromBody] UpsertScheduleRequest req) => Ok(await _svc.UpsertScheduleAsync(req));
    [HttpDelete("{hotelCode}/schedules/{id:int}")] public async Task<IActionResult> DeleteSchedule(int id) { await _svc.DeleteScheduleAsync(id); return Ok(); }

    // Group Members
    [HttpGet("bookings/{bookingId:int}/members")] public async Task<IActionResult> GetMembers(int bookingId) => Ok(await _svc.GetGroupMembersAsync(bookingId));
    [HttpPost("bookings/{bookingId:int}/members")] public async Task<IActionResult> AddMember(int bookingId, [FromBody] UpsertGroupMemberRequest req) { req.BookingId = bookingId; return Ok(await _svc.AddGroupMemberAsync(req)); }
    [HttpPut("bookings/{bookingId:int}/members/{id:int}")] public async Task<IActionResult> UpdateMember(int bookingId, int id, [FromBody] UpsertGroupMemberRequest req) { req.BookingId = bookingId; return Ok(await _svc.UpdateGroupMemberAsync(id, req)); }
    [HttpDelete("bookings/{bookingId:int}/members/{id:int}")] public async Task<IActionResult> DeleteMember(int id) { await _svc.DeleteGroupMemberAsync(id); return Ok(); }
}

[Authorize]
[ApiController]
[Route("api/hotel-reports")]
public class HotelReportsController : ControllerBase
{
    private readonly IHotelReportService _svc;
    public HotelReportsController(IHotelReportService svc) => _svc = svc;

    [HttpGet("{hotelCode}/occupancy")] public async Task<IActionResult> Occupancy(string hotelCode, [FromQuery] DateOnly from, [FromQuery] DateOnly to) => Ok(await _svc.GetOccupancyReportAsync(hotelCode, from, to));
    [HttpGet("{hotelCode}/revenue/monthly")] public async Task<IActionResult> MonthlyRevenue(string hotelCode, [FromQuery] int year = 0) => Ok(await _svc.GetRevenueByMonthAsync(hotelCode, year == 0 ? DateTime.Now.Year : year));
    [HttpGet("{hotelCode}/revenue/today")] public async Task<IActionResult> TodayRevenue(string hotelCode) => Ok(await _svc.GetRevenueTodayAsync(hotelCode));
    [HttpGet("{hotelCode}/services/popularity")] public async Task<IActionResult> ServicePop(string hotelCode, [FromQuery] DateOnly from, [FromQuery] DateOnly to) => Ok(await _svc.GetServicePopularityAsync(hotelCode, from, to));
    [HttpGet("{hotelCode}/vehicles/utilization")] public async Task<IActionResult> VehicleUtil(string hotelCode, [FromQuery] DateOnly from, [FromQuery] DateOnly to) => Ok(await _svc.GetVehicleUtilizationAsync(hotelCode, from, to));
}
