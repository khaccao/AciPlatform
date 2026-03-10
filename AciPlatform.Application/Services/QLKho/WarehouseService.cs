using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class WarehouseService : IWarehouseService
{
    private readonly IApplicationDbContext _context;

    public WarehouseService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Warehouse>> GetAll()
    {
        return await _context.Warehouses
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<PagingResult<WarehousePaging>> GetPaging(FilterParams param)
    {
        var query = _context.Warehouses.Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(param.SearchText))
        {
            query = query.Where(x => (x.Name != null && x.Name.Contains(param.SearchText)) 
                                || (x.Code != null && x.Code.Contains(param.SearchText)));
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new WarehousePaging
            {
                Id = x.Id,
                BranchId = x.BranchId,
                Name = x.Name,
                Code = x.Code,
                ManagerName = x.ManagerName,
                IsSyncChartOfAccount = x.IsSyncChartOfAccount
            })
            .ToListAsync();

        foreach (var warehouse in items)
        {
            var shelveNames = await (from ws in _context.WareHouseWithShelves
                                     join s in _context.WareHouseShelves on ws.WareHouseShelveId equals s.Id
                                     where ws.WareHouseId == warehouse.Id
                                     select s.Name)
                                    .ToListAsync();

            warehouse.Shevles = string.Join(", ", shelveNames);
        }

        return new PagingResult<WarehousePaging>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = items
        };
    }

    public async Task<WarehouseSetterModel> GetById(int id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse == null) return null!;

        var shelveIds = await _context.WareHouseWithShelves
            .Where(x => x.WareHouseId == id)
            .Select(x => x.WareHouseShelveId)
            .ToListAsync();

        return new WarehouseSetterModel
        {
            Id = warehouse.Id,
            BranchId = warehouse.BranchId,
            Name = warehouse.Name,
            Code = warehouse.Code,
            ManagerName = warehouse.ManagerName,
            IsSyncChartOfAccount = warehouse.IsSyncChartOfAccount,
            ShelveIds = shelveIds
        };
    }

    public async Task Create(WarehouseSetterModel param, int userId, int yearFilter)
    {
        if (string.IsNullOrWhiteSpace(param.Name))
            throw new Exception("Name is required");

        var isExisted = await _context.Warehouses.AnyAsync(x => x.Code == param.Code && !x.IsDeleted);
        if (isExisted)
            throw new Exception("Warehouse code already exists");

        var warehouse = new Warehouse
        {
            Name = param.Name,
            Code = param.Code,
            ManagerName = param.ManagerName,
            UserCreated = userId,
            UserUpdated = userId,
            BranchId = param.BranchId,
            IsSyncChartOfAccount = param.IsSyncChartOfAccount,
            CreatedDate = DateTime.Now
        };

        _context.Warehouses.Add(warehouse);
        await _context.SaveChangesAsync();

        if (param.ShelveIds != null && param.ShelveIds.Any())
        {
            var shelveAdds = param.ShelveIds.Select(x => new WareHouseWithShelves
            {
                WareHouseId = warehouse.Id,
                WareHouseShelveId = x,
            });

            _context.WareHouseWithShelves.AddRange(shelveAdds);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update(WarehouseSetterModel param, int userId, int yearFilter)
    {
        var warehouse = await _context.Warehouses.FindAsync(param.Id);
        if (warehouse == null)
            throw new Exception("Warehouse not found");

        var checkCode = await _context.Warehouses.AnyAsync(x => x.Id != param.Id && x.Code == param.Code && !x.IsDeleted);
        if (checkCode)
            throw new Exception("Warehouse code already exists");

        warehouse.Name = param.Name;
        warehouse.Code = param.Code;
        warehouse.ManagerName = param.ManagerName;
        warehouse.UpdatedDate = DateTime.Now;
        warehouse.UserUpdated = userId;
        warehouse.BranchId = param.BranchId;
        warehouse.IsSyncChartOfAccount = param.IsSyncChartOfAccount;

        _context.Warehouses.Update(warehouse);

        var shelvelsDel = await _context.WareHouseWithShelves.Where(x => x.WareHouseId == param.Id).ToListAsync();
        _context.WareHouseWithShelves.RemoveRange(shelvelsDel);

        if (param.ShelveIds != null && param.ShelveIds.Any())
        {
            var shelveAdds = param.ShelveIds.Select(x => new WareHouseWithShelves
            {
                WareHouseId = warehouse.Id,
                WareHouseShelveId = x,
            });

            _context.WareHouseWithShelves.AddRange(shelveAdds);
        }

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var warehouse = await _context.Warehouses.FindAsync(id);
        if (warehouse != null)
        {
            warehouse.IsDeleted = true;
            warehouse.UpdatedDate = DateTime.Now;
            _context.Warehouses.Update(warehouse);
            await _context.SaveChangesAsync();
        }
    }
}
