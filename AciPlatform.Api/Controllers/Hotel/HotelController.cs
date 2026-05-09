using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces.Hotel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Hotel;

/// <summary>
/// Hotel Management Controller
/// Route: /api/hotel
/// 
/// Thiết kế: Company = Hotel
/// - GET  /api/hotel              → Danh sách hotels (theo quyền user)
/// - GET  /api/hotel/all          → Tất cả hotels (SuperAdmin)
/// - GET  /api/hotel/{code}       → Chi tiết 1 hotel
/// - POST /api/hotel              → Tạo/cập nhật hotel
/// - GET  /api/hotel/{code}/users → Users của hotel
/// - POST /api/hotel/{code}/users → Gán user vào hotel
/// - DELETE /api/hotel/{code}/users/{userId} → Xóa user khỏi hotel
/// </summary>
[Authorize]
[ApiController]
[Route("api/hotel")]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    /// <summary>Lấy danh sách Hotels mà user hiện tại có quyền truy cập</summary>
    [HttpGet]
    public async Task<IActionResult> GetMyHotels()
    {
        var identity = HttpContext.GetIdentityUser();
        var isSuperAdmin = identity.Role?.Contains("SuperAdmin") == true;

        List<AciPlatform.Application.Interfaces.Hotel.HotelDto> hotels;
        if (isSuperAdmin)
            hotels = await _hotelService.GetAllHotelsAsync();
        else
            hotels = await _hotelService.GetHotelsByUserAsync(identity.Id);

        return Ok(hotels);
    }

    /// <summary>Tất cả hotels — SuperAdmin only</summary>
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var identity = HttpContext.GetIdentityUser();
        if (identity.Role?.Contains("SuperAdmin") != true)
            return Forbid();

        var hotels = await _hotelService.GetAllHotelsAsync();
        return Ok(hotels);
    }

    /// <summary>Chi tiết 1 hotel theo code</summary>
    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var hotel = await _hotelService.GetHotelByCodeAsync(code);
        if (hotel == null) return NotFound();
        return Ok(hotel);
    }

    /// <summary>Tạo mới hoặc cập nhật Hotel (đồng thời upsert Customers)</summary>
    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertHotelRequest request)
    {
        var identity = HttpContext.GetIdentityUser();
        if (identity.Role?.Contains("SuperAdmin") != true && identity.Role?.Contains("Admin") != true)
            return Forbid();

        if (string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { message = "Code và Name không được để trống." });

        var result = await _hotelService.UpsertHotelAsync(request);
        return Ok(result);
    }

    /// <summary>Danh sách users đã gán vào hotel này</summary>
    [HttpGet("{code}/users")]
    public async Task<IActionResult> GetUsers(string code)
    {
        var users = await _hotelService.GetUsersOfHotelAsync(code);
        return Ok(users);
    }

    /// <summary>Gán user vào Hotel (CompanyCode = HotelCode) kèm tài khoản PMS</summary>
    [HttpPost("{code}/users")]
    public async Task<IActionResult> AssignUser(string code, [FromBody] AssignHotelUserRequest request)
    {
        var identity = HttpContext.GetIdentityUser();
        if (identity.Role?.Contains("SuperAdmin") != true && identity.Role?.Contains("Admin") != true)
            return Forbid();

        request.HotelCode = code;
        await _hotelService.AssignUserToHotelAsync(request);
        return Ok(new { message = $"User {request.UserId} đã được gán vào Hotel '{code}'." });
    }

    /// <summary>Xóa user khỏi Hotel</summary>
    [HttpDelete("{code}/users/{userId}")]
    public async Task<IActionResult> RemoveUser(string code, int userId)
    {
        var identity = HttpContext.GetIdentityUser();
        if (identity.Role?.Contains("SuperAdmin") != true && identity.Role?.Contains("Admin") != true)
            return Forbid();

        await _hotelService.RemoveUserFromHotelAsync(userId, code);
        return Ok(new { message = $"User {userId} đã được xóa khỏi Hotel '{code}'." });
    }
}
