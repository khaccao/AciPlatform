using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GoodWarehousesController : ControllerBase
{
    private readonly IGoodWarehousesService _goodWarehousesService;

    public GoodWarehousesController(IGoodWarehousesService goodWarehousesService)
    {
        _goodWarehousesService = goodWarehousesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchViewModel param)
    {
        return Ok(await _goodWarehousesService.GetAll(param));
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncChartOfAccount([FromHeader] int yearFilter)
    {
        await _goodWarehousesService.SyncChartOfAccount(yearFilter);
        return Ok(new { code = 200 });
    }
}
