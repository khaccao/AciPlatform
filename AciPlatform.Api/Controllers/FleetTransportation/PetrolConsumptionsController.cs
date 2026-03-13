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
public class PetrolConsumptionsController : ControllerBase
{
    private readonly IPetrolConsumptionService _petrolConsumptionService;

    public PetrolConsumptionsController(IPetrolConsumptionService petrolConsumptionService)
    {
        _petrolConsumptionService = petrolConsumptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] FilterParams param)
    {
        return Ok(await _petrolConsumptionService.GetPaging(param));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _petrolConsumptionService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PetrolConsumptionModel model)
    {
        await _petrolConsumptionService.Create(model);
        return Ok(new { code = 200 });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PetrolConsumptionModel model)
    {
        model.Id = id;
        await _petrolConsumptionService.Update(model);
        return Ok(new { code = 200 });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _petrolConsumptionService.Delete(id);
        return Ok(new { code = 200 });
    }

    [HttpGet("report")]
    public async Task<IActionResult> ReportAsync([FromQuery] PetrolConsumptionReportRequestModel param)
    {
        return Ok(await _petrolConsumptionService.ReportAsync(param));
    }
}
