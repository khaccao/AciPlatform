using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class GoodsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public GoodsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchViewModel param)
    {
        var query = _context.GoodWarehouses
            .Where(p => !p.IsDeleted && p.Quantity > 0)
            .AsQueryable();

        if (!string.IsNullOrEmpty(param.GoodType)) query = query.Where(x => x.GoodsType == param.GoodType);
        if (!string.IsNullOrEmpty(param.Account)) query = query.Where(x => x.Account == param.Account);
        if (!string.IsNullOrEmpty(param.Detail1)) query = query.Where(x => x.Detail1 == param.Detail1);
        if (!string.IsNullOrEmpty(param.PriceCode)) query = query.Where(x => x.PriceList == param.PriceCode);
        if (!string.IsNullOrEmpty(param.MenuType)) query = query.Where(x => x.MenuType == param.MenuType);
        if (!string.IsNullOrEmpty(param.Warehouse)) query = query.Where(x => x.Warehouse == param.Warehouse);
        if (!string.IsNullOrEmpty(param.GoodCode)) query = query.Where(x => x.Detail1 == param.GoodCode || x.Detail2 == param.GoodCode);
        if (!string.IsNullOrEmpty(param.SearchText))
        {
            query = query.Where(x =>
                (x.Detail2 != null && x.Detail2.Contains(param.SearchText)) ||
                (x.DetailName2 != null && x.DetailName2.Contains(param.SearchText)) ||
                (x.Detail1 != null && x.Detail1.Contains(param.SearchText)) ||
                (x.DetailName1 != null && x.DetailName1.Contains(param.SearchText)));
        }

        if (param.Status != 0)
        {
            query = query.Where(x => x.Status == param.Status);
        }

        var totalItems = await query.CountAsync();
        var goods = await query
            .OrderBy(x => x.IsPrinted)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(p => new GoodWarehousesViewModel
            {
                Id = p.Id,
                MenuType = p.MenuType,
                Account = p.Account,
                AccountName = p.AccountName,
                Warehouse = p.Warehouse,
                WarehouseName = p.WarehouseName,
                Detail1 = p.Detail1,
                Detail2 = p.Detail2,
                DetailName1 = p.DetailName1,
                DetailName2 = p.DetailName2,
                GoodsType = p.GoodsType,
                Image1 = p.Image1,
                Quantity = p.Quantity,
                QuantityInput = p.QuantityInput,
                Status = p.Status,
                PriceList = p.PriceList,
                Order = p.Order,
                OrginalVoucherNumber = p.OrginalVoucherNumber,
                LedgerId = p.LedgerId,
                QrCode = (!string.IsNullOrEmpty(p.Detail2) ? p.Detail2 : (p.Detail1 ?? p.Account)) + " " + p.Order + "-" + p.Id,
                GoodCode = !string.IsNullOrEmpty(p.Detail2) ? p.Detail2 : (p.Detail1 ?? p.Account),
                GoodName = !string.IsNullOrEmpty(p.DetailName2) ? p.DetailName2 : (p.DetailName1 ?? p.AccountName),
                Note = p.Note,
                DateExpiration = p.DateExpiration,
                DateManufacture = p.DateManufacture,
                IsPrinted = p.IsPrinted
            })
            .ToListAsync();

        return Ok(new BaseResponseModel
        {
            TotalItems = totalItems,
            Data = goods,
            PageSize = param.PageSize,
            CurrentPage = param.Page
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetGoods()
    {
        var goods = await _context.GoodWarehouses
            .Where(x => !x.IsDeleted && x.Quantity > 0)
            .OrderBy(x => x.DetailName2)
            .Select(x => new
            {
                x.Id,
                GoodCode = !string.IsNullOrEmpty(x.Detail2) ? x.Detail2 : (x.Detail1 ?? x.Account),
                GoodName = !string.IsNullOrEmpty(x.DetailName2) ? x.DetailName2 : (x.DetailName1 ?? x.AccountName)
            })
            .ToListAsync();

        return Ok(new ObjectReturn { data = goods, status = 200 });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var good = await _context.GoodWarehouses.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (good == null)
        {
            return NotFound(new { msg = "Good not found" });
        }
        return Ok(good);
    }

    [HttpGet("report-good-in-warehouse")]
    public async Task<IActionResult> ReportForGoodsInWarehouse([FromQuery] SearchViewModel param)
    {
        return await GetAll(param);
    }

    [HttpGet("SyncAccountGood")]
    public IActionResult SyncAccountGood()
    {
        // Compatible endpoint kept for frontend flow; business sync is handled by existing QLKho services.
        return Ok(new ObjectReturn { data = true, status = 200, message = "Sync endpoint accepted" });
    }
}
