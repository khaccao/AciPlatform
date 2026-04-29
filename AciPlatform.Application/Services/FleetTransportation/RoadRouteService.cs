using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.FleetTransportation;

public class RoadRouteService : IRoadRouteService
{
    private readonly IApplicationDbContext _context;

    public RoadRouteService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<RoadRoutePagingModel>> GetPaging(FilterParams searchRequest)
    {
        var query = _context.RoadRoutes
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(searchRequest.SearchText)
                || (x.Name != null && x.Name.Contains(searchRequest.SearchText))
                || (x.Code != null && x.Code.Contains(searchRequest.SearchText)));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((searchRequest.Page - 1) * searchRequest.PageSize)
            .Take(searchRequest.PageSize)
            .Select(x => new RoadRoutePagingModel
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
                RoadRouteDetail = x.RoadRouteDetail,
                PoliceCheckPointIdStr = x.PoliceCheckPointIdStr,
                NumberOfTrips = x.NumberOfTrips
            })
            .ToListAsync();

        return new PagingResult<RoadRoutePagingModel>
        {
            CurrentPage = searchRequest.Page,
            PageSize = searchRequest.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(RoadRouteModel form)
    {
        _context.RoadRoutes.Add(new RoadRoute
        {
            Code = form.Code,
            Name = form.Name,
            RoadRouteDetail = form.RoadRouteDetail,
            PoliceCheckPointIdStr = form.PoliceCheckPointIdStr,
            NumberOfTrips = form.NumberOfTrips,
            CreatedDate = DateTime.Now
        });

        await _context.SaveChangesAsync();
    }

    public async Task Update(RoadRouteModel form)
    {
        var item = await _context.RoadRoutes.FirstOrDefaultAsync(x => x.Id == form.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Road route not found");

        item.Code = form.Code;
        item.Name = form.Name;
        item.RoadRouteDetail = form.RoadRouteDetail;
        item.PoliceCheckPointIdStr = form.PoliceCheckPointIdStr;
        item.NumberOfTrips = form.NumberOfTrips;
        item.UpdatedDate = DateTime.Now;

        _context.RoadRoutes.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.RoadRoutes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Road route not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.RoadRoutes.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<RoadRouteModel> GetDetail(int id)
    {
        var item = await _context.RoadRoutes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Road route not found");

        return new RoadRouteModel
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            RoadRouteDetail = item.RoadRouteDetail,
            PoliceCheckPointIdStr = item.PoliceCheckPointIdStr,
            NumberOfTrips = item.NumberOfTrips
        };
    }
}

