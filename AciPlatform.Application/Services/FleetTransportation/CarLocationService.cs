using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.FleetTransportation;

public class CarLocationService : ICarLocationService
{
    private readonly IApplicationDbContext _context;

    public CarLocationService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagingResult<CarLocationPagingModel>> GetPaging(FilterParams param, int userId)
    {
        var query = _context.CarLocations
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(param.SearchText)
                || (x.Note != null && x.Note.Contains(param.SearchText)));

        if (userId > 0)
        {
            query = query.Where(x => x.CreatedBy == userId);
        }

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new CarLocationPagingModel
            {
                Id = x.Id,
                Date = x.Date,
                Note = x.Note,
                ProcedureNumber = x.ProcedureNumber,
                Status = x.Status
            })
            .ToListAsync();

        return new PagingResult<CarLocationPagingModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(CarLocationModel form, int userId)
    {
        var item = new CarLocation
        {
            Date = form.Date,
            Note = form.Note,
            ProcedureNumber = await GetProcedureNumber(),
            Status = "Draft",
            CreatedBy = userId,
            UpdatedBy = userId,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        _context.CarLocations.Add(item);
        await _context.SaveChangesAsync();

        if (form.Details != null && form.Details.Any())
        {
            var details = form.Details.Select(x => new CarLocationDetail
            {
                CarLocationId = item.Id,
                LicensePlates = x.LicensePlates,
                Type = x.Type,
                Payload = x.Payload,
                DriverName = x.DriverName,
                Location = x.Location,
                PlanInprogress = x.PlanInprogress,
                PlanExpected = x.PlanExpected,
                Note = x.Note,
                FileStr = x.FileStr
            }).ToList();

            _context.CarLocationDetails.AddRange(details);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Update(CarLocationModel form, int userId)
    {
        var item = await _context.CarLocations.FirstOrDefaultAsync(x => x.Id == form.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car location not found");

        item.Note = form.Note;
        item.Date = form.Date;
        item.Status = form.Status ?? item.Status;
        item.UpdatedDate = DateTime.Now;
        item.UpdatedBy = userId;

        _context.CarLocations.Update(item);

        var oldDetails = await _context.CarLocationDetails
            .Where(x => x.CarLocationId == form.Id)
            .ToListAsync();

        _context.CarLocationDetails.RemoveRange(oldDetails);

        if (form.Details != null && form.Details.Any())
        {
            var newDetails = form.Details.Select(x => new CarLocationDetail
            {
                CarLocationId = item.Id,
                LicensePlates = x.LicensePlates,
                Type = x.Type,
                Payload = x.Payload,
                DriverName = x.DriverName,
                Location = x.Location,
                PlanInprogress = x.PlanInprogress,
                PlanExpected = x.PlanExpected,
                Note = x.Note,
                FileStr = x.FileStr
            }).ToList();

            _context.CarLocationDetails.AddRange(newDetails);
        }

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.CarLocations.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car location not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.CarLocations.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<CarLocationModel> GetDetail(int id)
    {
        var item = await _context.CarLocations.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car location not found");

        var details = await _context.CarLocationDetails
            .Where(x => x.CarLocationId == id && !x.IsDeleted)
            .Select(x => new CarLocationDetailModel
            {
                Id = x.Id,
                CarLocationId = x.CarLocationId,
                LicensePlates = x.LicensePlates,
                Type = x.Type,
                Payload = x.Payload,
                DriverName = x.DriverName,
                Location = x.Location,
                PlanInprogress = x.PlanInprogress,
                PlanExpected = x.PlanExpected,
                Note = x.Note,
                FileStr = x.FileStr
            })
            .ToListAsync();

        return new CarLocationModel
        {
            Id = item.Id,
            Date = item.Date,
            Note = item.Note,
            ProcedureNumber = item.ProcedureNumber,
            Status = item.Status,
            Details = details
        };
    }

    public async Task Accept(int id, int userId)
    {
        var item = await _context.CarLocations.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car location not found");

        item.Status = "Accepted";
        item.UpdatedBy = userId;
        item.UpdatedDate = DateTime.Now;

        _context.CarLocations.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task NotAccept(int id, int userId)
    {
        var item = await _context.CarLocations.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car location not found");

        item.Status = "NotAccepted";
        item.UpdatedBy = userId;
        item.UpdatedDate = DateTime.Now;

        _context.CarLocations.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetProcedureNumber()
    {
        var count = await _context.CarLocations.CountAsync(x => !x.IsDeleted);
        return $"CL-{DateTime.Now:yyyyMMdd}-{count + 1:D4}";
    }

    public async Task<string> Export(int id)
    {
        var existed = await _context.CarLocations.AnyAsync(x => x.Id == id && !x.IsDeleted);
        if (!existed)
        {
            throw new KeyNotFoundException("Car location not found");
        }

        return $"CarLocation-{id}";
    }
}
