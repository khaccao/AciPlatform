using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WareHouseShelvesController : ControllerBase
{
    private readonly IWareHouseShelvesService _wareHouseShelvesService;

    public WareHouseShelvesController(IWareHouseShelvesService wareHouseShelvesService)
    {
        _wareHouseShelvesService = wareHouseShelvesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _wareHouseShelvesService.GetAll(param));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        return Ok(new ObjectReturn { data = await _wareHouseShelvesService.GetAll(), status = 200 });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var model = await _wareHouseShelvesService.GetById(id);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WarehouseShelvesSetterModel model)
    {
        await _wareHouseShelvesService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] WarehouseShelvesSetterModel model)
    {
        model.Id = id;
        await _wareHouseShelvesService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _wareHouseShelvesService.Delete(id);
        return Ok(new { code = 200 });
    }
}
