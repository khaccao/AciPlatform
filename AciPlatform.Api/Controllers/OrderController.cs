using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public OrderController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> SearchOrder([FromQuery] OrderSearchModel search)
    {
        var fromDate = search.FromDate ?? DateTime.Today.AddMonths(-1);
        var toDate = (search.ToDate ?? DateTime.Today).AddDays(1);

        var query = _context.GoodWarehouseExports
            .Where(x => x.CreatedAt >= fromDate && x.CreatedAt < toDate)
            .AsQueryable();

        if (search.BillId.HasValue)
        {
            query = query.Where(x => x.BillId == search.BillId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search.SearchText) && int.TryParse(search.SearchText, out var billId))
        {
            query = query.Where(x => x.BillId == billId);
        }

        var grouped = query
            .GroupBy(x => x.BillId)
            .Select(g => new OrderViewModelResponse
            {
                BillId = g.Key,
                TotalLineItems = g.Count(),
                CreatedAt = g.Max(x => x.CreatedAt),
                IsDeleted = g.All(x => x.IsDeleted)
            });

        var totalItems = await grouped.CountAsync();
        var data = await grouped
            .OrderByDescending(x => x.CreatedAt)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .ToListAsync();

        return Ok(new BaseResponseModel
        {
            Data = data,
            TotalItems = totalItems,
            PageSize = search.PageSize,
            CurrentPage = search.Page
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var result = await (from ex in _context.GoodWarehouseExports
                            join gw in _context.GoodWarehouses on ex.GoodWarehouseId equals gw.Id
                            where ex.BillId == id
                            orderby ex.Id descending
                            select new OrderLineViewModel
                            {
                                ExportId = ex.Id,
                                GoodWarehouseId = ex.GoodWarehouseId,
                                GoodCode = !string.IsNullOrEmpty(gw.Detail2) ? gw.Detail2 : (gw.Detail1 ?? gw.Account),
                                GoodName = !string.IsNullOrEmpty(gw.DetailName2) ? gw.DetailName2 : (gw.DetailName1 ?? gw.AccountName),
                                Quantity = gw.Quantity,
                                CreatedAt = ex.CreatedAt
                            }).ToListAsync();

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderViewModelResponse model)
    {
        var exports = await _context.GoodWarehouseExports.Where(x => x.BillId == id).ToListAsync();
        if (!exports.Any())
        {
            return NotFound(new { msg = "Order not found" });
        }

        foreach (var item in exports)
        {
            item.IsDeleted = model.IsDeleted;
        }

        await _context.SaveChangesAsync();
        return Ok(new BaseResponseModel { Data = string.Empty });
    }

    [Route("notification-order")]
    [HttpGet]
    public async Task<ActionResult<OrderViewModelResponse>> GetNotificationToStaffCount()
    {
        var from = DateTime.Today;
        var result = await _context.GoodWarehouseExports
            .Where(x => !x.IsDeleted && x.CreatedAt >= from)
            .GroupBy(x => x.BillId)
            .Select(g => new OrderViewModelResponse
            {
                BillId = g.Key,
                TotalLineItems = g.Count(),
                CreatedAt = g.Max(x => x.CreatedAt),
                IsDeleted = false
            })
            .OrderByDescending(x => x.CreatedAt)
            .Take(20)
            .ToListAsync();

        return Ok(result);
    }
}

