using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class InventoryService : IInventoryService
{
    private readonly IApplicationDbContext _context;

    public InventoryService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inventory>> GetListData(FilterParams param, int year)
    {
        // Simplified version: Fetch from GoodWarehouses and calculate
        var query = _context.GoodWarehouses.Where(x => !x.IsDeleted && x.Status == 1);
        
        if (!string.IsNullOrEmpty(param.SearchText))
        {
            query = query.Where(x => (x.DetailName2 != null && x.DetailName2.Contains(param.SearchText)) 
                                || (x.Detail2 != null && x.Detail2.Contains(param.SearchText)));
        }

        var goods = await query.Skip((param.Page - 1) * param.PageSize).Take(param.PageSize)
            .Select(k => new Inventory
            {
                Account = k.Account,
                AccountName = k.AccountName,
                Detail1 = k.Detail1,
                DetailName1 = k.DetailName1,
                Detail2 = k.Detail2,
                DetailName2 = k.DetailName2,
                Warehouse = k.Warehouse,
                WarehouseName = k.WarehouseName,
                CreateAt = DateTime.Today,
                Image1 = k.Image1,
                InputQuantity = k.QuantityInput,
                CloseQuantity = k.Quantity
                // OutputQuantity and other logic would need Ledger integration
            }).ToListAsync();

        return goods;
    }

    public async Task Create(List<Inventory> datas)
    {
        foreach (var data in datas)
        {
            if (data.Id > 0)
                _context.Inventories.Update(data);
            else
                _context.Inventories.Add(data);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Inventory>> GetListInventory(DateTime? dtMax)
    {
        if (dtMax == null)
        {
            dtMax = await _context.Inventories.Select(x => x.CreateAt).OrderByDescending(x => x).FirstOrDefaultAsync();
        }
        return await _context.Inventories.Where(x => x.CreateAt == dtMax).ToListAsync();
    }

    public async Task<List<DateTime?>> GetListDateInventory()
    {
        return await _context.Inventories.OrderByDescending(x => x.CreateAt).Select(x => x.CreateAt).Distinct().ToListAsync();
    }

    public async Task Accept(List<Inventory> datas)
    {
        foreach (var data in datas)
        {
            data.isCheck = true;
            var goodWareHouse = await _context.GoodWarehouses.FirstOrDefaultAsync(x => x.Account == data.Account
                                    && (string.IsNullOrEmpty(data.Detail1) || x.Detail1 == data.Detail1)
                                    && (string.IsNullOrEmpty(data.Detail2) || x.Detail2 == data.Detail2)
                                    && (string.IsNullOrEmpty(data.Warehouse) || x.Warehouse == data.Warehouse)
                                    && x.DateExpiration == data.DateExpiration);
            
            if (goodWareHouse == null)
            {
                goodWareHouse = new GoodWarehouses
                {
                    Account = data.Account,
                    AccountName = data.AccountName,
                    Detail1 = data.Detail1,
                    DetailName1 = data.DetailName1,
                    Detail2 = data.Detail2,
                    DetailName2 = data.DetailName2,
                    Warehouse = data.Warehouse,
                    WarehouseName = data.WarehouseName,
                    Quantity = data.CloseQuantityReal,
                    Order = 0,
                    Status = 1,
                    DateExpiration = data.DateExpiration,
                    CreatedDate = DateTime.Now
                };
                _context.GoodWarehouses.Add(goodWareHouse);
            }
            else
            {
                goodWareHouse.Quantity = data.CloseQuantityReal;
                _context.GoodWarehouses.Update(goodWareHouse);
            }
        }
        await _context.SaveChangesAsync();
    }
}
