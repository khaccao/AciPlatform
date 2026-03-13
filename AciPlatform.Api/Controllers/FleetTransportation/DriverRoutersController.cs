using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.FleetTransportation;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(FleetExceptionFilter))]
public class DriverRoutersController : ControllerBase
{
    private readonly IDriverRouterService _driverRouterService;

    public DriverRoutersController(IDriverRouterService driverRouterService)
    {
        _driverRouterService = driverRouterService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams searchRequest)
    {
        return Ok(await _driverRouterService.GetPaging(searchRequest));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        return Ok(await _driverRouterService.GetById(id));
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start(int petrolConsumptionId)
    {
        await _driverRouterService.Start(petrolConsumptionId);
        return Ok(new { code = 200 });
    }

    [HttpPost("finish")]
    public async Task<IActionResult> Finish(int petrolConsumptionId)
    {
        await _driverRouterService.Finish(petrolConsumptionId);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DriverRouterModel model)
    {
        model.Id = id;
        await _driverRouterService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _driverRouterService.Delete(id);
        return Ok(new { code = 200 });
    }

    [HttpGet("list/police-point/{id}")]
    public async Task<IActionResult> GetListPoliceCheckPoint(int id)
    {
        return Ok(await _driverRouterService.GetListPoliceCheckPoint(id));
    }
}
