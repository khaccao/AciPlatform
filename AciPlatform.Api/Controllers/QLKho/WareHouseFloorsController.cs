using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WareHouseFloorsController : ControllerBase
{
    private readonly IWareHouseFloorService _wareHouseFloorService;

    public WareHouseFloorsController(IWareHouseFloorService wareHouseFloorService)
    {
        _wareHouseFloorService = wareHouseFloorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _wareHouseFloorService.GetAll(param));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        return Ok(await _wareHouseFloorService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var model = await _wareHouseFloorService.GetById(id);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WarehouseFloorSetterModel model)
    {
        await _wareHouseFloorService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WarehouseFloorSetterModel model)
    {
        model.Id = id;
        await _wareHouseFloorService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _wareHouseFloorService.Delete(id);
        return Ok(new { code = 200 });
    }
}
