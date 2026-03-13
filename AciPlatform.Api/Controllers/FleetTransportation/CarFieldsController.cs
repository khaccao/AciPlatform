using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Api.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.FleetTransportation;

[Authorize]
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(FleetExceptionFilter))]
public class CarFieldsController : ControllerBase
{
    private readonly ICarFieldService _carFieldService;

    public CarFieldsController(ICarFieldService carFieldService)
    {
        _carFieldService = carFieldService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _carFieldService.GetPaging(param));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _carFieldService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarFieldModel model)
    {
        await _carFieldService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarFieldModel model)
    {
        model.Id = id;
        await _carFieldService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _carFieldService.Delete(id);
        return Ok(new { code = 200 });
    }
}
