using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.QLKho;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.QLKho;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GoodWarehouseExportsController : ControllerBase
{
    private readonly IGoodWarehouseExportService _goodWarehouseExportService;

    public GoodWarehouseExportsController(IGoodWarehouseExportService goodWarehouseExportService)
    {
        _goodWarehouseExportService = goodWarehouseExportService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GoodWarehouseExportRequestModel param)
    {
        var result = await _goodWarehouseExportService.GetAll(param);
        return Ok(new ObjectReturn { data = result, status = 200 });
    }

    [HttpDelete("{billId}")]
    public async Task<IActionResult> Delete(int billId)
    {
        await _goodWarehouseExportService.Delete(billId);
        return Ok(new ObjectReturn { status = 200 });
    }
}
