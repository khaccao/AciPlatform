using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.FleetTransportation;

public class PoliceCheckPointService : IPoliceCheckPointService
{
    private readonly IApplicationDbContext _context;

    public PoliceCheckPointService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<PoliceCheckPointModel>> GetPaging(FilterParams searchRequest)
    {
        var query = _context.PoliceCheckPoints
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(searchRequest.SearchText)
                || (x.Name != null && x.Name.Contains(searchRequest.SearchText))
                || (x.Code != null && x.Code.Contains(searchRequest.SearchText)));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((searchRequest.Page - 1) * searchRequest.PageSize)
            .Take(searchRequest.PageSize)
            .Select(x => new PoliceCheckPointModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Amount = x.Amount
            })
            .ToListAsync();

        return new PagingResult<PoliceCheckPointModel>
        {
            CurrentPage = searchRequest.Page,
            PageSize = searchRequest.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(PoliceCheckPointModel form)
    {
        _context.PoliceCheckPoints.Add(new PoliceCheckPoint
        {
            Code = form.Code,
            Name = form.Name,
            Amount = form.Amount,
            CreatedDate = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public async Task Update(PoliceCheckPointModel form)
    {
        var item = await _context.PoliceCheckPoints.FirstOrDefaultAsync(x => x.Id == form.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Police checkpoint not found");

        item.Code = form.Code;
        item.Name = form.Name;
        item.Amount = form.Amount;
        item.UpdatedDate = DateTime.Now;

        _context.PoliceCheckPoints.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.PoliceCheckPoints.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Police checkpoint not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.PoliceCheckPoints.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<PoliceCheckPointModel> GetDetail(int id)
    {
        var item = await _context.PoliceCheckPoints.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Police checkpoint not found");

        return new PoliceCheckPointModel
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Amount = item.Amount
        };
    }

    public async Task<IEnumerable<PoliceCheckPointModel>> GetAll()
    {
        return await _context.PoliceCheckPoints
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .Select(x => new PoliceCheckPointModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                Amount = x.Amount
            })
            .ToListAsync();
    }
}

