using ManageEmployee.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AciPlatform.Application.Services.Ledger.Interfaces.Ledgers;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;

namespace ManageEmployee.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class LedgerWarehousesController : ControllerBase
{
    private readonly ILedgerWareHouseService _ledgerWareHouseService;

    public LedgerWarehousesController(ILedgerWareHouseService ledgerWareHouseService)
    {
        _ledgerWareHouseService = ledgerWareHouseService;
    }

    [HttpPost]
    [TypeFilter(typeof(ResponseWrapperFilterAttribute))]
    public async Task<IActionResult> Create([FromHeader] int yearFilter, List<LedgerWarehouseCreate> requests, string typePay, int customerId, bool isPrintBill)
    {
        await _ledgerWareHouseService.Create(requests, typePay, customerId, isPrintBill, yearFilter);
        return Ok();
    }

    [HttpGet]
    [TypeFilter(typeof(ResponseWrapperFilterAttribute))]
    public async Task<IActionResult> GetListHistory([FromQuery] LedgerWarehousesRequestPaging param)
    {
        var response = await _ledgerWareHouseService.GetListHistory(param);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [TypeFilter(typeof(ResponseWrapperFilterAttribute))]
    public async Task<IActionResult> GetListHistory(int id)
    {
        var response = await _ledgerWareHouseService.GetDetailHistory(id);
        return Ok(new ObjectReturn
        {
            data = response,
            status = 200,
        });
    }
}

