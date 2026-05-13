using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces.HotelManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Hotel;

[Authorize]
[ApiController]
[Route("api/hotel-rooms")]
public class HotelRoomsController : ControllerBase
{
    private readonly IHotelRoomService _roomSvc;
    public HotelRoomsController(IHotelRoomService roomSvc) => _roomSvc = roomSvc;

    /// GET /api/hotel-rooms/{hotelCode}/map — Sơ đồ phòng toàn bộ
    [HttpGet("{hotelCode}/map")]
    public async Task<IActionResult> GetRoomMap(string hotelCode)
        => Ok(await _roomSvc.GetRoomStatusMapAsync(hotelCode));

    /// GET /api/hotel-rooms/{hotelCode}/availability — Kiểm tra phòng trống
    [HttpGet("{hotelCode}/availability")]
    public async Task<IActionResult> CheckAvailability(string hotelCode,
        [FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut, [FromQuery] string? roomType)
        => Ok(await _roomSvc.CheckAvailabilityAsync(new RoomAvailabilityRequest
        { HotelCode = hotelCode, CheckIn = checkIn, CheckOut = checkOut, RoomType = roomType }));

    /// GET /api/hotel-rooms/{hotelCode}/forecast — Room Forecast Calendar
    [HttpGet("{hotelCode}/forecast")]
    public async Task<IActionResult> GetForecast(string hotelCode,
        [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        => Ok(await _roomSvc.GetRoomForecastAsync(hotelCode, fromDate, toDate));

    /// GET /api/hotel-rooms/{hotelCode}/room-rack
    [HttpGet("{hotelCode}/room-rack")]
    public async Task<IActionResult> GetRoomRack(string hotelCode,
        [FromQuery] DateTime? fromDate, [FromQuery] int days = 21)
        => Ok(await _roomSvc.GetRoomRackAsync(hotelCode, fromDate ?? DateTime.Today, days));

    /// PATCH /api/hotel-rooms/{hotelCode}/room-rack/move
    [HttpPatch("{hotelCode}/room-rack/move")]
    public async Task<IActionResult> MoveRoomRackBooking(string hotelCode, [FromBody] MoveRoomRackBookingRequest req)
    {
        await _roomSvc.MoveRoomRackBookingAsync(hotelCode, req);
        return Ok(new { message = "Booking room moved." });
    }

    /// POST /api/hotel-rooms/{hotelCode}/block — Block phòng (HOLD/MAINTENANCE)
    [HttpPost("{hotelCode}/block")]
    public async Task<IActionResult> BlockRoom(string hotelCode, [FromBody] BlockRoomRequest req)
    {
        req.HotelCode = hotelCode;
        req.UserId = HttpContext.GetIdentityUser().Id;
        await _roomSvc.BlockRoomAsync(req);
        return Ok(new { message = "Phòng đã được block." });
    }

    /// DELETE /api/hotel-rooms/{hotelCode}/block — Unblock phòng
    [HttpDelete("{hotelCode}/block")]
    public async Task<IActionResult> UnblockRoom(string hotelCode,
        [FromQuery] string roomNo, [FromQuery] string? bedCode,
        [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
    {
        await _roomSvc.UnblockRoomAsync(hotelCode, roomNo, bedCode, fromDate, toDate);
        return Ok(new { message = "Phòng đã được unblock." });
    }

    /// PATCH /api/hotel-rooms/{hotelCode}/status — Cập nhật trạng thái phòng (Housekeeping)
    [HttpPatch("{hotelCode}/status")]
    public async Task<IActionResult> UpdateStatus(string hotelCode, [FromBody] UpdateRoomStatusRequest req)
    {
        req.HotelCode = hotelCode;
        await _roomSvc.UpdateRoomStatusAsync(req);
        return Ok(new { message = "Trạng thái phòng đã cập nhật." });
    }

    /// GET /api/hotel-rooms/{hotelCode}/{roomNo}/beds — Danh sách giường
    [HttpGet("{hotelCode}/{roomNo}/beds")]
    public async Task<IActionResult> GetBeds(string hotelCode, string roomNo)
        => Ok(await _roomSvc.GetBedsByRoomAsync(hotelCode, roomNo));

    /// PUT /api/hotel-rooms/{hotelCode}/{roomNo}/beds/{bedCode} — Upsert giường
    [HttpPut("{hotelCode}/{roomNo}/beds/{bedCode}")]
    public async Task<IActionResult> UpsertBed(string hotelCode, string roomNo, string bedCode,
        [FromBody] UpsertBedRequest req)
        => Ok(await _roomSvc.UpsertBedAsync(hotelCode, roomNo, bedCode, req.BedName, req.BedType, req.Status));

    /// PATCH /api/hotel-rooms/{hotelCode}/{roomNo}/beds/{bedCode}/status
    [HttpPatch("{hotelCode}/{roomNo}/beds/{bedCode}/status")]
    public async Task<IActionResult> UpdateBedStatus(string hotelCode, string roomNo, string bedCode,
        [FromBody] UpdateBedStatusRequest req)
    {
        req.HotelCode = hotelCode;
        req.RoomNo = roomNo;
        req.BedCode = bedCode;
        await _roomSvc.UpdateBedStatusAsync(req);
        return Ok(new { message = "Trạng thái giường đã cập nhật." });
    }

    /// DELETE /api/hotel-rooms/{hotelCode}/{roomNo}/beds/{bedCode}
    [HttpDelete("{hotelCode}/{roomNo}/beds/{bedCode}")]
    public async Task<IActionResult> DeleteBed(string hotelCode, string roomNo, string bedCode)
    {
        await _roomSvc.DeleteBedAsync(hotelCode, roomNo, bedCode);
        return Ok(new { message = "Giường đã xóa." });
    }
}

public class UpsertBedRequest
{
    public string BedName { get; set; } = string.Empty;
    public string BedType { get; set; } = "SINGLE";
    public string? Status { get; set; }
}
