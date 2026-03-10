using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.QLKho;
using AciPlatform.Domain.Entities.QLKho;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.QLKho;

public class WareHouseFloorService : IWareHouseFloorService
{
    private readonly IApplicationDbContext _context;

    public WareHouseFloorService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<WarehouseFloorPaging>> GetAll(FilterParams param)
    {
        var query = _context.WareHouseFloors.Where(x => !x.IsDeleted);

        if (!string.IsNullOrEmpty(param.SearchText))
        {
            query = query.Where(x => (x.Code != null && x.Code.Contains(param.SearchText)) || (x.Name != null && x.Name.Contains(param.SearchText)));
        }

        var totalItems = await query.CountAsync();
        var floors = await query.OrderBy(x => x.Id).Skip((param.Page - 1) * param.PageSize).Take(param.PageSize)
            .Select(x => new WarehouseFloorPaging
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Note = x.Note
            }).ToListAsync();

        foreach (var floor in floors)
        {
            var positions = await (from fp in _context.WareHouseFloorWithPositions
                                   join p in _context.WareHousePositions on fp.WareHousePositionId equals p.Id
                                   where fp.WareHouseFloorId == floor.Id
                                   select p.Name).ToListAsync();

            floor.Positions = string.Join(", ", positions);
        }

        return new PagingResult<WarehouseFloorPaging>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = floors
        };
    }

    public async Task<IEnumerable<WareHouseFloorGetAllModel>> GetAll()
    {
        return await _context.WareHouseFloors
           .Join(_context.WareHouseShelvesWithFloors,
                   b => b.Id,
                   d => d.WareHouseFloorId,
                   (b, d) => new WareHouseFloorGetAllModel
                   {
                       Id = b.Id,
                       Code = b.Code,
                       Name = b.Name,
                       WareHouseShelveId = d.WareHouseShelvesId
                   })
           .ToListAsync();
    }

    public async Task<WarehouseFloorSetterModel> GetById(int id)
    {
        var floor = await _context.WareHouseFloors.FindAsync(id);
        if (floor == null) return null!;

        var positionIds = await _context.WareHouseFloorWithPositions
            .Where(x => x.WareHouseFloorId == id)
            .Select(x => x.WareHousePositionId)
            .ToListAsync();

        return new WarehouseFloorSetterModel
        {
            Id = floor.Id,
            Code = floor.Code,
            Name = floor.Name,
            Note = floor.Note,
            PositionIds = positionIds
        };
    }

    public async Task Create(WarehouseFloorSetterModel param)
    {
        var floor = new WareHouseFloor
        {
            Name = param.Name,
            Code = param.Code,
            Note = param.Note,
            CreatedDate = DateTime.Now
        };
        _context.WareHouseFloors.Add(floor);
        await _context.SaveChangesAsync();

        if (param.PositionIds != null)
        {
            var relations = param.PositionIds.Select(x => new WareHouseFloorWithPosition
            {
                WareHouseFloorId = floor.Id,
                WareHousePositionId = x,
            });

            await _context.WareHouseFloorWithPositions.AddRangeAsync(relations);
        }

        await _context.SaveChangesAsync();
    }

    public async Task Update(WarehouseFloorSetterModel param)
    {
        var floor = await _context.WareHouseFloors.FindAsync(param.Id);
        if (floor == null) throw new Exception("Floor not found");

        floor.Name = param.Name;
        floor.Code = param.Code;
        floor.Note = param.Note;
        floor.UpdatedDate = DateTime.Now;

        _context.WareHouseFloors.Update(floor);

        var relationsDel = await _context.WareHouseFloorWithPositions.Where(x => x.WareHouseFloorId == param.Id).ToListAsync();
        _context.WareHouseFloorWithPositions.RemoveRange(relationsDel);

        if (param.PositionIds != null)
        {
            var relationsAdd = param.PositionIds.Select(x => new WareHouseFloorWithPosition
            {
                WareHouseFloorId = floor.Id,
                WareHousePositionId = x,
            });

            await _context.WareHouseFloorWithPositions.AddRangeAsync(relationsAdd);
        }

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var floor = await _context.WareHouseFloors.FindAsync(id);
        if (floor != null)
        {
            _context.WareHouseFloors.Remove(floor);
            await _context.SaveChangesAsync();
        }
    }
}
