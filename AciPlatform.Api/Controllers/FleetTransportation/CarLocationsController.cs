using AciPlatform.Application.DTOs;
using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.FleetTransportation;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[ServiceFilter(typeof(FleetExceptionFilter))]
public class CarLocationsController : ControllerBase
{
    private readonly ICarLocationService _carLocationService;

    public CarLocationsController(ICarLocationService carLocationService)
    {
        _carLocationService = carLocationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _carLocationService.GetPaging(param, HttpContext.GetIdentityUser().Id));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _carLocationService.GetDetail(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarLocationModel model)
    {
        await _carLocationService.Create(model, HttpContext.GetIdentityUser().Id);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarLocationModel model)
    {
        model.Id = id;
        await _carLocationService.Update(model, HttpContext.GetIdentityUser().Id);
        return Ok(new { code = 200 });
    }

    [HttpPut("accept/{id}")]
    public async Task<IActionResult> Accept(int id)
    {
        await _carLocationService.Accept(id, HttpContext.GetIdentityUser().Id);
        return Ok(new { code = 200 });
    }

    [HttpPut("not-accept/{id}")]
    public async Task<IActionResult> NotAccept(int id)
    {
        await _carLocationService.NotAccept(id, HttpContext.GetIdentityUser().Id);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _carLocationService.Delete(id);
        return Ok(new { code = 200 });
    }

    [HttpGet("get-procedure-number")]
    public async Task<IActionResult> GetProcedureNumber()
    {
        return Ok(new { data = await _carLocationService.GetProcedureNumber() });
    }

    [HttpPost("export/{id}")]
    public async Task<IActionResult> Export(int id)
    {
        return Ok(new { data = await _carLocationService.Export(id) });
    }
}
