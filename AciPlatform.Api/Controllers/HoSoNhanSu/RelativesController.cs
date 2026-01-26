using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.HoSoNhanSu;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RelativesController : ControllerBase
{
    private readonly IRelativeService _service;

    public RelativesController(IRelativeService service)
    {
        _service = service;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var items = await _service.GetByUserAsync(userId);
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
    public async Task<IActionResult> Create([FromBody] RelativeRequest request)
    {
        var item = await _service.CreateAsync(request);
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RelativeRequest request)
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
