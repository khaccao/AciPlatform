using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.FleetTransportation;

public class PetrolConsumptionService : IPetrolConsumptionService
{
    private readonly IApplicationDbContext _context;

    public PetrolConsumptionService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<PetrolConsumptionGetterModel>> GetPaging(FilterParams param)
    {
        var query = _context.PetrolConsumptions
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(param.SearchText)
                || (x.Note != null && x.Note.Contains(param.SearchText)));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new PetrolConsumptionGetterModel
            {
                Id = x.Id,
                Date = x.Date,
                UserId = x.UserId,
                CarId = x.CarId,
                PetroPrice = x.PetroPrice,
                KmFrom = x.KmFrom,
                KmTo = x.KmTo,
                LocationFrom = x.LocationFrom,
                LocationTo = x.LocationTo,
                AdvanceAmount = x.AdvanceAmount,
                Note = x.Note,
                RoadRouteId = x.RoadRouteId
            })
            .ToListAsync();

        return new PagingResult<PetrolConsumptionGetterModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(PetrolConsumptionModel param)
    {
        var item = new PetrolConsumption
        {
            Date = param.Date,
            UserId = param.UserId,
            CarId = param.CarId,
            PetroPrice = param.PetroPrice,
            KmFrom = param.KmFrom,
            KmTo = param.KmTo,
            LocationFrom = param.LocationFrom,
            LocationTo = param.LocationTo,
            AdvanceAmount = param.AdvanceAmount,
            Note = param.Note,
            RoadRouteId = param.RoadRouteId,
            CreatedDate = DateTime.Now
        };

        _context.PetrolConsumptions.Add(item);
        await _context.SaveChangesAsync();

        if (param.Points != null && param.Points.Any())
        {
            var points = param.Points.Select(x => new PetrolConsumptionPoliceCheckPoint
            {
                PetrolConsumptionId = item.Id,
                Amount = x.Amount,
                PoliceCheckPointName = x.PoliceCheckPointName,
                IsArise = x.IsArise
            }).ToList();

            _context.PetrolConsumptionPoliceCheckPoints.AddRange(points);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update(PetrolConsumptionModel param)
    {
        var item = await _context.PetrolConsumptions.FirstOrDefaultAsync(x => x.Id == param.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Petrol consumption not found");

        item.Date = param.Date;
        item.UserId = param.UserId;
        item.CarId = param.CarId;
        item.PetroPrice = param.PetroPrice;
        item.KmFrom = param.KmFrom;
        item.KmTo = param.KmTo;
        item.LocationFrom = param.LocationFrom;
        item.LocationTo = param.LocationTo;
        item.AdvanceAmount = param.AdvanceAmount;
        item.Note = param.Note;
        item.RoadRouteId = param.RoadRouteId;
        item.UpdatedDate = DateTime.Now;

        _context.PetrolConsumptions.Update(item);

        var oldPoints = await _context.PetrolConsumptionPoliceCheckPoints
            .Where(x => x.PetrolConsumptionId == item.Id)
            .ToListAsync();

        _context.PetrolConsumptionPoliceCheckPoints.RemoveRange(oldPoints);

        if (param.Points != null && param.Points.Any())
        {
            var newPoints = param.Points.Select(x => new PetrolConsumptionPoliceCheckPoint
            {
                PetrolConsumptionId = item.Id,
                Amount = x.Amount,
                PoliceCheckPointName = x.PoliceCheckPointName,
                IsArise = x.IsArise
            }).ToList();

            _context.PetrolConsumptionPoliceCheckPoints.AddRange(newPoints);
        }

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.PetrolConsumptions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Petrol consumption not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.PetrolConsumptions.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<PetrolConsumptionModel> GetById(int id)
    {
        var item = await _context.PetrolConsumptions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Petrol consumption not found");

        var points = await _context.PetrolConsumptionPoliceCheckPoints
            .Where(x => x.PetrolConsumptionId == id)
            .Select(x => new PetrolConsumptionPoliceCheckPointModel
            {
                Amount = x.Amount,
                PoliceCheckPointName = x.PoliceCheckPointName,
                IsArise = x.IsArise
            })
            .ToListAsync();

        return new PetrolConsumptionModel
        {
            Id = item.Id,
            Date = item.Date,
            UserId = item.UserId,
            CarId = item.CarId,
            PetroPrice = item.PetroPrice,
            KmFrom = item.KmFrom,
            KmTo = item.KmTo,
            LocationFrom = item.LocationFrom,
            LocationTo = item.LocationTo,
            AdvanceAmount = item.AdvanceAmount,
            Note = item.Note,
            RoadRouteId = item.RoadRouteId,
            Points = points
        };
    }

    public async Task<IEnumerable<PetrolConsumptionReportModel>> ReportAsync(PetrolConsumptionReportRequestModel param)
    {
        var query = _context.PetrolConsumptions.Where(x => !x.IsDeleted);

        if (param.FromDate.HasValue)
        {
            query = query.Where(x => x.Date >= param.FromDate.Value);
        }

        if (param.ToDate.HasValue)
        {
            query = query.Where(x => x.Date <= param.ToDate.Value);
        }

        if (param.CarId.HasValue)
        {
            query = query.Where(x => x.CarId == param.CarId.Value);
        }

        if (param.UserId.HasValue)
        {
            query = query.Where(x => x.UserId == param.UserId.Value);
        }

        return await query
            .OrderByDescending(x => x.Date)
            .Select(x => new PetrolConsumptionReportModel
            {
                Id = x.Id,
                Date = x.Date,
                CarId = x.CarId,
                UserId = x.UserId,
                KmFrom = x.KmFrom,
                KmTo = x.KmTo,
                LocationFrom = x.LocationFrom,
                LocationTo = x.LocationTo,
                AdvanceAmount = x.AdvanceAmount,
                PetroPrice = x.PetroPrice,
                Note = x.Note,
                RoadRouteId = x.RoadRouteId,
                LicensePlates = _context.Cars.Where(c => c.Id == x.CarId && !c.IsDeleted).Select(c => c.LicensePlates).FirstOrDefault(),
                DriverName = _context.Users.Where(u => u.Id == x.UserId && !u.IsDeleted).Select(u => u.FullName).FirstOrDefault(),
                RoadRouteName = _context.RoadRoutes.Where(r => r.Id == x.RoadRouteId && !r.IsDeleted).Select(r => r.Name).FirstOrDefault(),
                TotalPoliceCheckPointAmount = _context.PetrolConsumptionPoliceCheckPoints
                    .Where(p => p.PetrolConsumptionId == x.Id)
                    .Select(p => (double?)p.Amount)
                    .Sum() ?? 0
            })
            .ToListAsync();
    }
}
