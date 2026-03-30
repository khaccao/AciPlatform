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
public class CarFleetsController : ControllerBase
{
    private readonly ICarFleetService _carFleetService;

    public CarFleetsController(ICarFleetService carFleetService)
    {
        _carFleetService = carFleetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _carFleetService.GetPaging(param));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        return Ok(await _carFleetService.GetList());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _carFleetService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarFleetModel model)
    {
        await _carFleetService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarFleetModel model)
    {
        model.Id = id;
        await _carFleetService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _carFleetService.Delete(id);
        return Ok(new { code = 200 });
    }
}
