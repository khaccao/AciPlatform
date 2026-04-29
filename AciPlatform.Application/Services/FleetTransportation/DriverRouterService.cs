using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using AciPlatform.Domain.Enums.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.FleetTransportation;

public class DriverRouterService : IDriverRouterService
{
    private readonly IApplicationDbContext _context;

    public DriverRouterService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<DriverRouterPagingModel>> GetPaging(FilterParams param)
    {
        var query = _context.DriverRouters
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(param.SearchText)
                || (x.Note != null && x.Note.Contains(param.SearchText)));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new DriverRouterPagingModel
            {
                Id = x.Id,
                Date = x.Date,
                Amount = x.Amount,
                Note = x.Note,
                Status = x.Status,
                PetrolConsumptionId = x.PetrolConsumptionId,
                AdvancePaymentAmount = x.AdvancePaymentAmount,
                FuelAmount = x.FuelAmount
            })
            .ToListAsync();

        foreach (var item in data)
        {
            var petrol = await _context.PetrolConsumptions
                .FirstOrDefaultAsync(x => x.Id == item.PetrolConsumptionId && !x.IsDeleted);

            if (petrol == null)
            {
                continue;
            }

            item.RoadRouteName = await _context.RoadRoutes
                .Where(x => x.Id == petrol.RoadRouteId && !x.IsDeleted)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            item.LicensePlates = await _context.Cars
                .Where(x => x.Id == petrol.CarId && !x.IsDeleted)
                .Select(x => x.LicensePlates)
                .FirstOrDefaultAsync();

            item.Driver = await _context.Users
                .Where(x => x.Id == petrol.UserId && !x.IsDeleted)
                .Select(x => x.FullName)
                .FirstOrDefaultAsync();

            item.KmFrom = petrol.KmFrom;
            item.KmTo = petrol.KmTo;
        }

        return new PagingResult<DriverRouterPagingModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Start(int petrolConsumptionId)
    {
        var item = await _context.DriverRouters
            .FirstOrDefaultAsync(x => x.PetrolConsumptionId == petrolConsumptionId
                && x.Status != nameof(DriverRouterStatus.Finish)
                && !x.IsDeleted);

        if (item == null)
        {
            var petrolConsumption = await _context.PetrolConsumptions
                .FirstOrDefaultAsync(x => x.Id == petrolConsumptionId && !x.IsDeleted)
                ?? throw new KeyNotFoundException("Petrol consumption not found");

            item = new DriverRouter
            {
                Date = DateTime.Now,
                PetrolConsumptionId = petrolConsumptionId,
                Status = nameof(DriverRouterStatus.Start),
                AdvancePaymentAmount = petrolConsumption.AdvanceAmount,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            _context.DriverRouters.Add(item);
            await _context.SaveChangesAsync();

            var startDetail = new DriverRouterDetail
            {
                DriverRouterId = item.Id,
                Location = petrolConsumption.LocationFrom,
                Date = DateTime.Now,
                Status = nameof(DriverRouterStatus.Start)
            };

            _context.DriverRouterDetails.Add(startDetail);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Finish(int petrolConsumptionId)
    {
        var item = await _context.DriverRouters
            .FirstOrDefaultAsync(x => x.PetrolConsumptionId == petrolConsumptionId && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Driver router not found");

        var petrolConsumption = await _context.PetrolConsumptions
            .FirstOrDefaultAsync(x => x.Id == petrolConsumptionId && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Petrol consumption not found");

        item.Status = nameof(DriverRouterStatus.Finish);
        item.UpdatedDate = DateTime.Now;

        _context.DriverRouters.Update(item);

        var finishDetail = await _context.DriverRouterDetails
            .FirstOrDefaultAsync(x => x.DriverRouterId == item.Id && x.Status == nameof(DriverRouterStatus.Finish));

        if (finishDetail == null)
        {
            _context.DriverRouterDetails.Add(new DriverRouterDetail
            {
                DriverRouterId = item.Id,
                Location = petrolConsumption.LocationTo,
                Date = DateTime.Now,
                Status = nameof(DriverRouterStatus.Finish)
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<DriverRouterModel> GetById(int id)
    {
        var item = await _context.DriverRouters.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Driver router not found");

        return new DriverRouterModel
        {
            Id = item.Id,
            Date = item.Date,
            Amount = item.Amount,
            Note = item.Note,
            Status = item.Status,
            PetrolConsumptionId = item.PetrolConsumptionId,
            AdvancePaymentAmount = item.AdvancePaymentAmount,
            FuelAmount = item.FuelAmount
        };
    }

    public async Task Update(DriverRouterModel form)
    {
        var item = await _context.DriverRouters.FirstOrDefaultAsync(x => x.Id == form.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Driver router not found");

        item.Note = form.Note;
        item.Amount = form.Amount;
        item.AdvancePaymentAmount = form.AdvancePaymentAmount;
        item.FuelAmount = form.FuelAmount;
        item.UpdatedDate = DateTime.Now;

        _context.DriverRouters.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.DriverRouters.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Driver router not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.DriverRouters.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PoliceCheckPointModel>> GetListPoliceCheckPoint(int id)
    {
        var petrolConsumption = await _context.PetrolConsumptions
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Petrol consumption not found");

        var roadRoute = await _context.RoadRoutes
            .FirstOrDefaultAsync(x => x.Id == petrolConsumption.RoadRouteId && !x.IsDeleted);

        if (roadRoute == null || string.IsNullOrWhiteSpace(roadRoute.PoliceCheckPointIdStr))
        {
            return new List<PoliceCheckPointModel>();
        }

        var ids = roadRoute.PoliceCheckPointIdStr
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => int.TryParse(x.Trim(), out var value) ? value : 0)
            .Where(x => x > 0)
            .ToList();

        if (!ids.Any())
        {
            return new List<PoliceCheckPointModel>();
        }

        return await _context.PoliceCheckPoints
            .Where(x => !x.IsDeleted && ids.Contains(x.Id))
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

