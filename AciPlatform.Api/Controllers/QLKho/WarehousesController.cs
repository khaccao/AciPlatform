using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaging([FromQuery] FilterParams param)
    {
        return Ok(await _warehouseService.GetPaging(param));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetSelectList()
    {
        var warehouses = await _warehouseService.GetAll();
        return Ok(new ObjectReturn { data = warehouses, status = 200 });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var model = await _warehouseService.GetById(id);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromHeader] int yearFilter, [FromBody] WarehouseSetterModel warehouse)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            await _warehouseService.Create(warehouse, userId, yearFilter);
            return Ok(new { code = 200 });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = 400, msg = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromHeader] int yearFilter, [FromRoute] int id, [FromBody] WarehouseSetterModel warehouse)
    {
        try
        {
            warehouse.Id = id;
            var userId = int.Parse(User.FindFirstValue("UserId") ?? "0");
            await _warehouseService.Update(warehouse, userId, yearFilter);
            return Ok(new { code = 200 });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = 400, msg = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _warehouseService.Delete(id);
            return Ok(new { code = 200 });
        }
        catch (Exception ex)
        {
            return BadRequest(new { code = 400, msg = ex.Message });
        }
    }
}
