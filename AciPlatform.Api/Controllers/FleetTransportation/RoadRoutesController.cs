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
public class RoadRoutesController : ControllerBase
{
    private readonly IRoadRouteService _roadRouteService;

    public RoadRoutesController(IRoadRouteService roadRouteService)
    {
        _roadRouteService = roadRouteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams searchRequest)
    {
        return Ok(await _roadRouteService.GetPaging(searchRequest));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        return Ok(await _roadRouteService.GetDetail(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoadRouteModel model)
    {
        await _roadRouteService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoadRouteModel model)
    {
        model.Id = id;
        await _roadRouteService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _roadRouteService.Delete(id);
        return Ok(new { code = 200 });
    }
}
