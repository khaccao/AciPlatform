using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.FleetTransportation;

public class CarFieldService : ICarFieldService
{
    private readonly IApplicationDbContext _context;

    public CarFieldService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<CarFieldPagingModel>> GetPaging(FilterParams param)
    {
        var query = _context.CarFields
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(param.SearchText)
                || (x.Name != null && x.Name.Contains(param.SearchText)));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new CarFieldPagingModel
            {
                Id = x.Id,
                CarId = x.CarId,
                Name = x.Name,
                Order = x.Order
            })
            .ToListAsync();

        return new PagingResult<CarFieldPagingModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(CarFieldModel param)
    {
        _context.CarFields.Add(new CarField
        {
            CarId = param.CarId,
            Name = param.Name,
            Order = param.Order,
            CreatedDate = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public async Task Update(CarFieldModel param)
    {
        var item = await _context.CarFields.FirstOrDefaultAsync(x => x.Id == param.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car field not found");

        item.CarId = param.CarId;
        item.Name = param.Name;
        item.Order = param.Order;
        item.UpdatedDate = DateTime.Now;

        _context.CarFields.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.CarFields.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car field not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.CarFields.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<CarFieldModel> GetById(int id)
    {
        var item = await _context.CarFields.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car field not found");

        return new CarFieldModel
        {
            Id = item.Id,
            CarId = item.CarId,
            Name = item.Name,
            Order = item.Order
        };
    }
}
