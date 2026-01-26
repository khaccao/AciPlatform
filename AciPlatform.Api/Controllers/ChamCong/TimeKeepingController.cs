using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.ChamCong;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.ChamCong;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TimeKeepingController : ControllerBase
{
    private readonly ITimeKeepingService _service;

    public TimeKeepingController(ITimeKeepingService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int? userId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var items = await _service.GetByRangeAsync(userId, from, to);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TimeKeepingEntryRequest request)
    {
        var item = await _service.CreateAsync(request);
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TimeKeepingEntryRequest request)
    {
        await _service.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}
