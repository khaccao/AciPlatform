using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces.HotelManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Hotel;

/// <summary>
/// Booking Controller — FIT (cá nhân) + GIT (đoàn)
/// Route: /api/hotel-bookings
/// </summary>
[Authorize]
[ApiController]
[Route("api/hotel-bookings")]
public class HotelBookingsController : ControllerBase
{
    private readonly IHotelBookingService _svc;
    public HotelBookingsController(IHotelBookingService svc) => _svc = svc;

    /// GET /api/hotel-bookings?hotelCode=HOMEHG&status=CONFIRMED&page=1
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] BookingFilterRequest filter)
    {
        var (items, total) = await _svc.GetBookingsAsync(filter);
        return Ok(new { items, total, page = filter.Page, pageSize = filter.PageSize });
    }

    /// GET /api/hotel-bookings/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var b = await _svc.GetBookingByIdAsync(id);
        return b == null ? NotFound() : Ok(b);
    }

    /// GET /api/hotel-bookings/code/{hotelCode}/{bookingCode}
    [HttpGet("code/{hotelCode}/{bookingCode}")]
    public async Task<IActionResult> GetByCode(string hotelCode, string bookingCode)
    {
        var b = await _svc.GetBookingByCodeAsync(hotelCode, bookingCode);
        return b == null ? NotFound() : Ok(b);
    }

    /// POST /api/hotel-bookings — Tạo booking mới (FIT hoặc GIT)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest req)
    {
        req.CreatedBy = HttpContext.GetIdentityUser().Id;
        if (string.IsNullOrWhiteSpace(req.GuestName))
            return BadRequest(new { message = "Tên khách không được để trống." });
        if (req.CheckIn >= req.CheckOut)
            return BadRequest(new { message = "CheckIn phải trước CheckOut." });
        if (!req.Rooms.Any())
            return BadRequest(new { message = "Phải chọn ít nhất 1 phòng hoặc giường." });

        var result = await _svc.CreateBookingAsync(req);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// PUT /api/hotel-bookings/{id} — Cập nhật booking
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateBookingRequest req)
        => Ok(await _svc.UpdateBookingAsync(id, req));

    /// POST /api/hotel-bookings/{id}/services — Thêm dịch vụ/minibar
    [HttpPost("{id:int}/services")]
    public async Task<IActionResult> AddService(int id, [FromBody] AddBookingServiceRequest req)
        => Ok(await _svc.AddBookingServiceAsync(id, req));

    /// PATCH /api/hotel-bookings/{id}/status — Check-in / Check-out / Cancel
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusRequest req)
    {
        req.BookingId = id;
        await _svc.UpdateStatusAsync(req);
        return Ok(new { message = $"Booking đã chuyển sang trạng thái {req.Status}." });
    }

    /// DELETE /api/hotel-bookings/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteBookingAsync(id);
        return Ok(new { message = "Booking đã xóa." });
    }

    /// GET /api/hotel-bookings/{hotelCode}/price — Tính giá phòng (với seasonal pricing)
    [HttpGet("{hotelCode}/price")]
    public async Task<IActionResult> CalculatePrice(string hotelCode,
        [FromQuery] string roomType, [FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
    {
        var price = await _svc.CalculateRoomPriceAsync(hotelCode, roomType, checkIn, checkOut);
        return Ok(new { roomType, checkIn, checkOut, totalPrice = price,
            nights = (int)(checkOut.Date - checkIn.Date).TotalDays });
    }

    /// GET /api/hotel-bookings/{hotelCode}/dashboard — Dashboard hôm nay
    [HttpGet("{hotelCode}/dashboard")]
    public async Task<IActionResult> Dashboard(string hotelCode)
        => Ok(await _svc.GetTodayDashboardAsync(hotelCode));

    /// POST /api/hotel-bookings/{id}/invoice — Xuất hóa đơn
    [HttpPost("{id:int}/invoice")]
    public async Task<IActionResult> GenerateInvoice(int id, [FromBody] InvoiceRequest req)
        => Ok(await _svc.GenerateInvoiceAsync(id, req.PaymentMethod));

    // ── Catalog & Mapping Endpoints ───────────────────────────
    
    [HttpGet("{hotelCode}/services")]
    public async Task<IActionResult> GetServices(string hotelCode, [FromQuery] string? category)
        => Ok(await _svc.GetServicesAsync(hotelCode, category));

    [HttpPost("services")]
    public async Task<IActionResult> UpsertService([FromBody] HotelServiceDto req)
        => Ok(await _svc.UpsertServiceAsync(req));

    [HttpDelete("services/{id:int}")]
    public async Task<IActionResult> DeleteService(int id) { await _svc.DeleteServiceAsync(id); return Ok(); }

    [HttpGet("{hotelCode}/areas")]
    public async Task<IActionResult> GetAreas(string hotelCode)
        => Ok(await _svc.GetAreasAsync(hotelCode));

    [HttpPost("areas")]
    public async Task<IActionResult> UpsertArea([FromBody] HotelAreaDto req)
        => Ok(await _svc.UpsertAreaAsync(req));

    [HttpDelete("areas/{id:int}")]
    public async Task<IActionResult> DeleteArea(int id) { await _svc.DeleteAreaAsync(id); return Ok(); }

    [HttpGet("{hotelCode}/elements")]
    public async Task<IActionResult> GetElements(string hotelCode, [FromQuery] int? areaId)
        => Ok(await _svc.GetElementsAsync(hotelCode, areaId));

    [HttpPost("elements")]
    public async Task<IActionResult> UpsertElement([FromBody] HotelElementDto req)
        => Ok(await _svc.UpsertElementAsync(req));

    [HttpDelete("elements/{id:int}")]
    public async Task<IActionResult> DeleteElement(int id) { await _svc.DeleteElementAsync(id); return Ok(); }
}

public class InvoiceRequest { public string PaymentMethod { get; set; } = "CASH"; }
