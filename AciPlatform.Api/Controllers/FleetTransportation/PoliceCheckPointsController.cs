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
public class PoliceCheckPointsController : ControllerBase
{
    private readonly IPoliceCheckPointService _policeCheckPointService;

    public PoliceCheckPointsController(IPoliceCheckPointService policeCheckPointService)
    {
        _policeCheckPointService = policeCheckPointService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaging([FromQuery] FilterParams searchRequest)
    {
        return Ok(await _policeCheckPointService.GetPaging(searchRequest));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        return Ok(await _policeCheckPointService.GetDetail(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PoliceCheckPointModel model)
    {
        await _policeCheckPointService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PoliceCheckPointModel model)
    {
        model.Id = id;
        await _policeCheckPointService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _policeCheckPointService.Delete(id);
        return Ok(new { code = 200 });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _policeCheckPointService.GetAll());
    }
}
