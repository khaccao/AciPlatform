using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WareHousePositionsController : ControllerBase
{
    private readonly IWareHousePositionService _wareHousePositionService;

    public WareHousePositionsController(IWareHousePositionService wareHousePositionService)
    {
        _wareHousePositionService = wareHousePositionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _wareHousePositionService.GetAll(param));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        return Ok(new ObjectReturn { data = await _wareHousePositionService.GetAll(), status = 200 });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var model = await _wareHousePositionService.GetById(id);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WareHousePosition model)
    {
        await _wareHousePositionService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WareHousePosition model)
    {
        model.Id = id;
        await _wareHousePositionService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _wareHousePositionService.Delete(id);
        return Ok(new { code = 200 });
    }
}
