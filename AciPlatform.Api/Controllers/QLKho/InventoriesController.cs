using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InventoriesController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoriesController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetListData([FromQuery] FilterParams param, [FromHeader] int yearFilter)
    {
        var result = await _inventoryService.GetListData(param, yearFilter);
        return Ok(new ObjectReturn { data = result, status = 200 });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] List<Inventory> datas)
    {
        await _inventoryService.Create(datas);
        return Ok(new ObjectReturn { status = 200 });
    }

    [HttpGet("list-inventory")]
    public async Task<IActionResult> GetListInventory([FromQuery] DateTime? dtMax)
    {
        var result = await _inventoryService.GetListInventory(dtMax);
        return Ok(new ObjectReturn { data = result, status = 200 });
    }

    [HttpGet("list-date")]
    public async Task<IActionResult> GetListDateInventory()
    {
        var result = await _inventoryService.GetListDateInventory();
        return Ok(new ObjectReturn { data = result, status = 200 });
    }

    [HttpPost("accept")]
    public async Task<IActionResult> Accept([FromBody] List<Inventory> datas)
    {
        await _inventoryService.Accept(datas);
        return Ok(new ObjectReturn { status = 200 });
    }
}
