using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class WareHouseShelvesService : IWareHouseShelvesService
{
    private readonly IApplicationDbContext _context;

    public WareHouseShelvesService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<WarehouseShelvesPaging>> GetAll(FilterParams param)
    {
        var query = _context.WareHouseShelves.Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(param.SearchText))
        {
            query = query.Where(x => (x.Code != null && x.Code.Contains(param.SearchText)) || (x.Name != null && x.Name.Contains(param.SearchText)));
        }

        var totalItems = await query.CountAsync();
        var shelves = await query.OrderBy(x => x.Id).Skip((param.Page - 1) * param.PageSize).Take(param.PageSize)
            .Select(x => new WarehouseShelvesPaging
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Note = x.Note
            }).ToListAsync();

        foreach (var shelve in shelves)
        {
            var floors = await (from sf in _context.WareHouseShelvesWithFloors
                                join f in _context.WareHouseFloors on sf.WareHouseFloorId equals f.Id
                                where sf.WareHouseShelvesId == shelve.Id
                                select f.Name).ToListAsync();

            shelve.Floors = string.Join(", ", floors);
        }

        return new PagingResult<WarehouseShelvesPaging>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = shelves
        };
    }

    public async Task<IEnumerable<WareHouseShelvesGetAllModel>> GetAll()
    {
        return await _context.WareHouseShelves.Where(x => !x.IsDeleted)
            .Join(_context.WareHouseWithShelves,
                    b => b.Id,
                    d => d.WareHouseShelveId,
                    (b, d) => new WareHouseShelvesGetAllModel
                    {
                        Id = b.Id,
                        Code = b.Code,
                        Name = b.Name,
                        WareHouseId = d.WareHouseId
                    })
            .ToListAsync();
    }

    public async Task<WarehouseShelvesSetterModel> GetById(int id)
    {
        var item = await _context.WareHouseShelves.FindAsync(id);
        if (item == null) return null!;

        var floorIds = await _context.WareHouseShelvesWithFloors
            .Where(x => x.WareHouseShelvesId == id)
            .Select(x => x.WareHouseFloorId)
            .ToListAsync();

        return new WarehouseShelvesSetterModel
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Note = item.Note,
            OrderHorizontal = item.OrderHorizontal,
            OrderVertical = item.OrderVertical,
            FloorIds = floorIds
        };
    }

    public async Task Create(WarehouseShelvesSetterModel param)
    {
        var shelve = new WareHouseShelves
        {
            Name = param.Name,
            Code = param.Code,
            Note = param.Note,
            OrderHorizontal = param.OrderHorizontal,
            OrderVertical = param.OrderVertical,
            CreatedDate = DateTime.Now
        };
        _context.WareHouseShelves.Add(shelve);
        await _context.SaveChangesAsync();

        if (param.FloorIds != null)
        {
            var relations = param.FloorIds.Select(x => new WareHouseShelvesWithFloors
            {
                WareHouseShelvesId = shelve.Id,
                WareHouseFloorId = x,
            });

            await _context.WareHouseShelvesWithFloors.AddRangeAsync(relations);
        }
        await _context.SaveChangesAsync();
    }

    public async Task Update(WarehouseShelvesSetterModel param)
    {
        var shelve = await _context.WareHouseShelves.FindAsync(param.Id);
        if (shelve == null) throw new Exception("Shelve not found");

        shelve.Name = param.Name;
        shelve.Code = param.Code;
        shelve.Note = param.Note;
        shelve.OrderHorizontal = param.OrderHorizontal;
        shelve.OrderVertical = param.OrderVertical;
        shelve.UpdatedDate = DateTime.Now;

        _context.WareHouseShelves.Update(shelve);

        var relationsDel = await _context.WareHouseShelvesWithFloors.Where(x => x.WareHouseShelvesId == param.Id).ToListAsync();
        _context.WareHouseShelvesWithFloors.RemoveRange(relationsDel);

        if (param.FloorIds != null)
        {
            var relationsAdd = param.FloorIds.Select(x => new WareHouseShelvesWithFloors
            {
                WareHouseShelvesId = shelve.Id,
                WareHouseFloorId = x,
            });

            await _context.WareHouseShelvesWithFloors.AddRangeAsync(relationsAdd);
        }
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var shelve = await _context.WareHouseShelves.FindAsync(id);
        if (shelve != null)
        {
            shelve.IsDeleted = true;
            shelve.UpdatedDate = DateTime.Now;
            _context.WareHouseShelves.Update(shelve);
            await _context.SaveChangesAsync();
        }
    }
}
