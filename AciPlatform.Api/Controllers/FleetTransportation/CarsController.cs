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
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _carService.GetPaging(param));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList()
    {
        return Ok(await _carService.GetList());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _carService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CarModel model)
    {
        await _carService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarModel model)
    {
        model.Id = id;
        await _carService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _carService.Delete(id);
        return Ok(new { code = 200 });
    }

    [HttpGet("car-field-setup")]
    public async Task<IActionResult> GetCarFieldSetup(int carId)
    {
        return Ok(await _carService.GetCarFieldSetup(carId));
    }

    [HttpPut("car-field-setup")]
    public async Task<IActionResult> UpdateCarFieldSetup([FromQuery] int carId, [FromBody] List<CarFieldSetupModel> carFieldSetups)
    {
        await _carService.UpdateCarFieldSetup(carId, carFieldSetups);
        return Ok(new { code = 200 });
    }
}
