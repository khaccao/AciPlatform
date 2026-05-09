using AciPlatform.Application.Interfaces.HotelManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Hotel;

[Authorize]
[ApiController]
[Route("api/hotel-vehicles")]
public class HotelVehiclesController : ControllerBase
{
    private readonly IHotelVehicleService _svc;
    public HotelVehiclesController(IHotelVehicleService svc) => _svc = svc;

    [HttpGet("{hotelCode}")] // GET all vehicles
    public async Task<IActionResult> GetAll(string hotelCode, [FromQuery] string? status)
        => Ok(await _svc.GetVehiclesAsync(hotelCode, status));

    [HttpGet("{hotelCode}/{vehicleCode}")]
    public async Task<IActionResult> GetByCode(string hotelCode, string vehicleCode)
    {
        var v = await _svc.GetVehicleByCodeAsync(hotelCode, vehicleCode);
        return v == null ? NotFound() : Ok(v);
    }

    [HttpGet("{hotelCode}/available")]
    public async Task<IActionResult> GetAvailable(string hotelCode,
        [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] string? vehicleType)
        => Ok(await _svc.GetAvailableVehiclesAsync(hotelCode, from, to, vehicleType));

    [HttpPost] // Tạo xe mới
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest req)
        => Ok(await _svc.CreateVehicleAsync(req));

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateVehicleRequest req)
        => Ok(await _svc.UpdateVehicleAsync(id, req));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteVehicleAsync(id);
        return Ok(new { message = "Xe đã xóa." });
    }

    [HttpPatch("{hotelCode}/{vehicleCode}/status")]
    public async Task<IActionResult> UpdateStatus(string hotelCode, string vehicleCode,
        [FromBody] VehicleStatusRequest req)
    {
        await _svc.UpdateVehicleStatusAsync(hotelCode, vehicleCode, req.Status, req.FuelLevel, req.Condition);
        return Ok(new { message = "Trạng thái xe đã cập nhật." });
    }

    // ── Rental Management ─────────────────────────────────────

    [HttpGet("{hotelCode}/rentals/active")]
    public async Task<IActionResult> GetActiveRentals(string hotelCode)
        => Ok(await _svc.GetActiveRentalsAsync(hotelCode));

    [HttpGet("{hotelCode}/rentals/history")]
    public async Task<IActionResult> GetRentalHistory(string hotelCode,
        [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        => Ok(await _svc.GetRentalHistoryAsync(hotelCode, from, to));

    [HttpGet("rentals/{id:int}")]
    public async Task<IActionResult> GetRentalById(int id)
    {
        var r = await _svc.GetRentalByIdAsync(id);
        return r == null ? NotFound() : Ok(r);
    }

    [HttpPost("rentals")] // Tạo giao dịch thuê xe
    public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.GuestName) || string.IsNullOrWhiteSpace(req.VehicleCode))
            return BadRequest(new { message = "Thiếu thông tin bắt buộc." });
        var result = await _svc.CreateRentalAsync(req);
        return Ok(result);
    }

    [HttpPost("rentals/{id:int}/return")] // Trả xe
    public async Task<IActionResult> ReturnVehicle(int id, [FromBody] ReturnVehicleRequest req)
    {
        req.RentalId = id;
        return Ok(await _svc.ReturnVehicleAsync(req));
    }
}

public class VehicleStatusRequest
{
    public string Status { get; set; } = string.Empty;
    public int? FuelLevel { get; set; }
    public string? Condition { get; set; }
}

// ── Guests Controller ─────────────────────────────────────────

[Authorize]
[ApiController]
[Route("api/hotel-guests")]
public class HotelGuestsController : ControllerBase
{
    private readonly IHotelGuestService _svc;
    public HotelGuestsController(IHotelGuestService svc) => _svc = svc;

    [HttpGet("{hotelCode}")]
    public async Task<IActionResult> Search(string hotelCode, [FromQuery] string? q, [FromQuery] int page = 1)
        => Ok(await _svc.SearchGuestsAsync(hotelCode, q, page));

    [HttpGet("{hotelCode}/phone/{phone}")]
    public async Task<IActionResult> GetByPhone(string hotelCode, string phone)
    {
        var g = await _svc.GetGuestByPhoneAsync(hotelCode, phone);
        return g == null ? NotFound() : Ok(g);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var g = await _svc.GetGuestByIdAsync(id);
        return g == null ? NotFound() : Ok(g);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertGuestRequest req)
        => Ok(await _svc.UpsertGuestAsync(req));

    [HttpGet("{id:int}/bookings")]
    public async Task<IActionResult> GetBookings(int id)
        => Ok(await _svc.GetGuestBookingHistoryAsync(id));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteGuestAsync(id);
        return Ok(new { message = "Khách đã xóa." });
    }
}

// ── Services Catalog Controller ───────────────────────────────

[Authorize]
[ApiController]
[Route("api/hotel-services")]
public class HotelServicesCatalogController : ControllerBase
{
    private readonly IHotelServiceCatalogService _svc;
    public HotelServicesCatalogController(IHotelServiceCatalogService svc) => _svc = svc;

    [HttpGet("{hotelCode}")]
    public async Task<IActionResult> GetAll(string hotelCode, [FromQuery] string? category)
        => Ok(await _svc.GetServicesAsync(hotelCode, category));

    [HttpGet("{hotelCode}/{serviceCode}")]
    public async Task<IActionResult> GetByCode(string hotelCode, string serviceCode)
    {
        var s = await _svc.GetServiceByCodeAsync(hotelCode, serviceCode);
        return s == null ? NotFound() : Ok(s);
    }

    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertServiceRequest req)
        => Ok(await _svc.UpsertServiceAsync(req));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteServiceAsync(id);
        return Ok(new { message = "Dịch vụ đã xóa." });
    }

    [HttpPatch("{hotelCode}/{serviceCode}/toggle")]
    public async Task<IActionResult> Toggle(string hotelCode, string serviceCode, [FromQuery] bool available)
    {
        await _svc.ToggleAvailabilityAsync(hotelCode, serviceCode, available);
        return Ok(new { message = $"Dịch vụ {serviceCode} {(available ? "bật" : "tắt")}." });
    }
}
