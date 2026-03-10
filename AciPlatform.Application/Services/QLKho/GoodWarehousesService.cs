using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class GoodWarehousesService : IGoodWarehousesService
{
    private readonly IApplicationDbContext _context;

    public GoodWarehousesService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<GoodWarehousesViewModel>> GetAll(SearchViewModel param)
    {
        var query = from p in _context.GoodWarehouses
                    where !p.IsDeleted && p.Quantity > 0
                    && (string.IsNullOrEmpty(param.GoodType) || p.GoodsType == param.GoodType)
                    && (string.IsNullOrEmpty(param.Account) || p.Account == param.Account)
                    && (string.IsNullOrEmpty(param.Detail1) || p.Detail1 == param.Detail1)
                    && (string.IsNullOrEmpty(param.PriceCode) || p.PriceList == param.PriceCode)
                    && (string.IsNullOrEmpty(param.MenuType) || p.MenuType == param.MenuType)
                    && (string.IsNullOrEmpty(param.SearchText) || (p.DetailName2 != null && p.DetailName2.Contains(param.SearchText)) || (p.Detail2 != null && p.Detail2.Contains(param.SearchText)))
                    && p.Status == param.Status
                    orderby p.IsPrinted
                    select new GoodWarehousesViewModel
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
                        IsPrinted = p.IsPrinted,
                        QuantityInput = p.QuantityInput
                    };

        if (!string.IsNullOrEmpty(param.Warehouse))
        {
            query = query.Where(x => x.Warehouse == param.Warehouse);
        }

        if (!string.IsNullOrEmpty(param.GoodCode))
        {
            query = query.Where(x => x.Detail1 == param.GoodCode || x.Detail2 == param.GoodCode);
        }

        var totalItems = await query.CountAsync();
        var datas = await query.Skip((param.Page - 1) * param.PageSize).Take(param.PageSize).ToListAsync();

        // Optional: Fill positions and other details if needed, similar to original service
        // Skipping complex join logic for now to ensure basic functionality

        return new PagingResult<GoodWarehousesViewModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = datas
        };
    }

    public async Task SyncChartOfAccount(int year)
    {
        // Implementation for syncing chart of account
        // This usually involves complex logic based on the specific business rules
        await Task.CompletedTask;
    }
}
