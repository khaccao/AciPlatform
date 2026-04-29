using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class GoodWarehouseExportService : IGoodWarehouseExportService
{
    private readonly IApplicationDbContext _context;

    public GoodWarehouseExportService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<GoodWarehouseExportsViewModel>> GetAll(GoodWarehouseExportRequestModel param)
    {
        var fromdt = param.Fromdt ?? DateTime.Today;
        var todt = (param.Todt ?? DateTime.Today).AddDays(1);

        var query = from ex in _context.GoodWarehouseExports
                    join p in _context.GoodWarehouses on ex.GoodWarehouseId equals p.Id
                    where !ex.IsDeleted && ex.CreatedAt >= fromdt && ex.CreatedAt < todt
                    select new GoodWarehouseExportsViewModel
                    {
                        Id = ex.Id,
                        Warehouse = p.Warehouse,
                        WarehouseName = p.WarehouseName,
                        Quantity = p.Quantity,
                        DateExpiration = p.DateExpiration,
                        Order = p.Order,
                        OrginalVoucherNumber = p.OrginalVoucherNumber,
                        QrCode = (!string.IsNullOrEmpty(p.Detail2) ? p.Detail2 : (p.Detail1 ?? p.Account)) + " " + p.Order + "-" + p.Id,
                        GoodCode = !string.IsNullOrEmpty(p.Detail2) ? p.Detail2 : (p.Detail1 ?? p.Account),
                        GoodName = !string.IsNullOrEmpty(p.DetailName2) ? p.DetailName2 : (p.DetailName1 ?? p.AccountName)
                    };

        var totalItems = await query.CountAsync();
        var data = await query.OrderByDescending(x => x.Id).Skip((param.Page - 1) * param.PageSize).Take(param.PageSize).ToListAsync();

        return new PagingResult<GoodWarehouseExportsViewModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Delete(int billId)
    {
        var exports = await _context.GoodWarehouseExports.Where(x => x.BillId == billId).ToListAsync();
        if (exports.Any())
        {
            foreach (var item in exports)
            {
                item.IsDeleted = true;
            }
            _context.GoodWarehouseExports.UpdateRange(exports);
            await _context.SaveChangesAsync();
        }
    }
}

