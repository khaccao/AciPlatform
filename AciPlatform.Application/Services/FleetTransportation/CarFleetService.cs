using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;
using AciPlatform.Application.Interfaces;

namespace AciPlatform.Application.Services.FleetTransportation;

public class CarFleetService : ICarFleetService
{
    private readonly IApplicationDbContext _context;

    public CarFleetService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarFleetModel>> GetList()
    {
        return await _context.CarFleets
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new CarFleetModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToListAsync();
    }

    public async Task<PagingResult<CarFleetPagingModel>> GetPaging(FilterParams param)
    {
        var query = _context.CarFleets
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(param.SearchText) || x.Name.Contains(param.SearchText));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderBy(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new CarFleetPagingModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CarCount = x.Cars.Count(c => !c.IsDeleted)
            })
            .ToListAsync();

        return new PagingResult<CarFleetPagingModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(CarFleetModel model)
    {
        var item = new CarFleet
        {
            Name = model.Name,
            Description = model.Description
        };
        _context.CarFleets.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(CarFleetModel model)
    {
        var item = await _context.CarFleets.FindAsync(model.Id) ?? throw new KeyNotFoundException();
        item.Name = model.Name;
        item.Description = model.Description;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.CarFleets.FindAsync(id) ?? throw new KeyNotFoundException();
        item.IsDeleted = true;
        await _context.SaveChangesAsync();
    }

    public async Task<CarFleetModel> GetById(int id)
    {
        var item = await _context.CarFleets.FindAsync(id) ?? throw new KeyNotFoundException();
        return new CarFleetModel { Id = item.Id, Name = item.Name, Description = item.Description };
    }
}

