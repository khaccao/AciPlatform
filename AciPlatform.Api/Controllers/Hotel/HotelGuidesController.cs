using AciPlatform.Application.Interfaces.HotelManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Hotel;

[Authorize]
[ApiController]
[Route("api/hotel-guides")]
public class HotelGuidesController : ControllerBase
{
    private readonly IHotelGuideService _svc;
    public HotelGuidesController(IHotelGuideService svc) => _svc = svc;

    // ── CRUD Guides ────────────────────────────────────────────

    /// GET /api/hotel-guides/{hotelCode} — Danh sách HDV
    [HttpGet("{hotelCode}")]
    public async Task<IActionResult> GetAll(string hotelCode, [FromQuery] bool? isActive)
        => Ok(await _svc.GetGuidesAsync(hotelCode, isActive));

    /// GET /api/hotel-guides/{hotelCode}/{id} — Chi tiết HDV
    [HttpGet("{hotelCode}/{id:int}")]
    public async Task<IActionResult> GetById(string hotelCode, int id)
    {
        var g = await _svc.GetGuideByIdAsync(id);
        return g == null ? NotFound() : Ok(g);
    }

    /// POST /api/hotel-guides — Tạo mới / cập nhật HDV
    [HttpPost]
    public async Task<IActionResult> Upsert([FromBody] UpsertTourGuideRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return BadRequest(new { message = "Tên hướng dẫn viên không được để trống." });
        return Ok(await _svc.UpsertGuideAsync(req));
    }

    /// PUT /api/hotel-guides/{id} — Cập nhật HDV
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpsertTourGuideRequest req)
        => Ok(await _svc.UpsertGuideAsync(req));

    /// DELETE /api/hotel-guides/{id} — Xóa mềm HDV
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _svc.DeleteGuideAsync(id);
        return Ok(new { message = "Hướng dẫn viên đã được xóa." });
    }

    /// PATCH /api/hotel-guides/{id}/toggle — Bật/tắt hoạt động
    [HttpPatch("{id:int}/toggle")]
    public async Task<IActionResult> Toggle(int id, [FromQuery] bool isActive)
    {
        await _svc.ToggleGuideStatusAsync(id, isActive);
        return Ok(new { message = $"Hướng dẫn viên {(isActive ? "đã kích hoạt" : "đã tạm dừng")}." });
    }

    /// GET /api/hotel-guides/{hotelCode}/{id}/stats — Thống kê HDV theo năm
    [HttpGet("{hotelCode}/{id:int}/stats")]
    public async Task<IActionResult> GetStats(string hotelCode, int id, [FromQuery] int? year)
        => Ok(await _svc.GetGuideStatsAsync(hotelCode, id, year ?? DateTime.Now.Year));

    // ── Contracts ──────────────────────────────────────────────

    /// GET /api/hotel-guides/{hotelCode}/contracts — Danh sách hợp đồng
    [HttpGet("{hotelCode}/contracts")]
    public async Task<IActionResult> GetContracts(string hotelCode, [FromQuery] int? guideId)
        => Ok(await _svc.GetContractsAsync(hotelCode, guideId));

    /// POST /api/hotel-guides/contracts — Ký hợp đồng mới
    [HttpPost("contracts")]
    public async Task<IActionResult> CreateContract([FromBody] CreateGuideContractRequest req)
    {
        if (req.GuideId <= 0)
            return BadRequest(new { message = "GuideId không hợp lệ." });
        return Ok(await _svc.CreateContractAsync(req));
    }

    /// PATCH /api/hotel-guides/contracts/{id}/status — Cập nhật trạng thái hợp đồng
    [HttpPatch("contracts/{id:int}/status")]
    public async Task<IActionResult> UpdateContractStatus(int id, [FromQuery] string status)
    {
        await _svc.UpdateContractStatusAsync(id, status);
        return Ok(new { message = "Trạng thái hợp đồng đã cập nhật." });
    }

    // ── Salary / Payroll ───────────────────────────────────────

    /// GET /api/hotel-guides/{hotelCode}/salaries — Danh sách bảng lương
    [HttpGet("{hotelCode}/salaries")]
    public async Task<IActionResult> GetSalaries(string hotelCode,
        [FromQuery] int? month, [FromQuery] int? year)
        => Ok(await _svc.GetSalariesAsync(hotelCode, month, year));

    /// POST /api/hotel-guides/salaries/calculate — Tính lương tháng
    [HttpPost("salaries/calculate")]
    public async Task<IActionResult> CalculateSalary([FromBody] CreateGuideSalaryRequest req)
    {
        if (req.Month < 1 || req.Month > 12)
            return BadRequest(new { message = "Tháng không hợp lệ (1-12)." });
        return Ok(await _svc.CalculateSalaryAsync(req));
    }

    /// PATCH /api/hotel-guides/salaries/{id}/approve — Duyệt lương
    [HttpPatch("salaries/{id:int}/approve")]
    public async Task<IActionResult> ApproveSalary(int id)
    {
        await _svc.ApproveSalaryAsync(id);
        return Ok(new { message = "Bảng lương đã được duyệt." });
    }

    /// PATCH /api/hotel-guides/salaries/{id}/paid — Xác nhận đã chi trả
    [HttpPatch("salaries/{id:int}/paid")]
    public async Task<IActionResult> MarkPaid(int id)
    {
        await _svc.MarkSalaryPaidAsync(id);
        return Ok(new { message = "Đã xác nhận chi trả lương." });
    }
}
