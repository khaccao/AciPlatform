using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.FleetTransportation;
using AciPlatform.Domain.Entities.FleetTransportation;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AciPlatform.Application.Services.FleetTransportation;

public class CarService : ICarService
{
    private readonly IApplicationDbContext _context;

    public CarService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CarGetterModel>> GetList()
    {
        return await _context.Cars
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Id)
            .Select(x => new CarGetterModel
            {
                Id = x.Id,
                LicensePlates = x.LicensePlates,
                Note = x.Note,
                CarFleetId = x.CarFleetId
            })
            .ToListAsync();
    }

    public async Task<PagingResult<CarGetterPagingModel>> GetPaging(FilterParams param)
    {
        var query = _context.Cars
            .Where(x => !x.IsDeleted)
            .Where(x => string.IsNullOrEmpty(param.SearchText)
                || (x.LicensePlates != null && x.LicensePlates.Contains(param.SearchText))
                || (x.Note != null && x.Note.Contains(param.SearchText)));

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.Id)
            .Skip((param.Page - 1) * param.PageSize)
            .Take(param.PageSize)
            .Select(x => new CarGetterPagingModel
            {
                Id = x.Id,
                LicensePlates = x.LicensePlates,
                Note = x.Note,
                MileageAllowance = x.MileageAllowance,
                FuelAmount = x.FuelAmount,
                CarFleetId = x.CarFleetId
            })
            .ToListAsync();

        return new PagingResult<CarGetterPagingModel>
        {
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task Create(CarModel param)
    {
        var item = new Car
        {
            LicensePlates = param.LicensePlates,
            Note = param.Note,
            Content = param.Content,
            MileageAllowance = param.MileageAllowance,
            FuelAmount = param.FuelAmount,
            CarFleetId = param.CarFleetId,
            FileLink = SerializeFiles(param.Files),
            CreatedDate = DateTime.Now
        };

        _context.Cars.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(CarModel param)
    {
        var item = await _context.Cars.FirstOrDefaultAsync(x => x.Id == param.Id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car not found");

        item.LicensePlates = param.LicensePlates;
        item.Note = param.Note;
        item.Content = param.Content;
        item.MileageAllowance = param.MileageAllowance;
        item.FuelAmount = param.FuelAmount;
        item.CarFleetId = param.CarFleetId;
        item.FileLink = SerializeFiles(param.Files);
        item.UpdatedDate = DateTime.Now;

        _context.Cars.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var item = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car not found");

        item.IsDeleted = true;
        item.UpdatedDate = DateTime.Now;

        _context.Cars.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<CarGetterDetailModel> GetById(int id)
    {
        var item = await _context.Cars.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
            ?? throw new KeyNotFoundException("Car not found");

        return new CarGetterDetailModel
        {
            Id = item.Id,
            LicensePlates = item.LicensePlates,
            Note = item.Note,
            Content = item.Content,
            MileageAllowance = item.MileageAllowance,
            FuelAmount = item.FuelAmount,
            Files = DeserializeFiles(item.FileLink)
        };
    }

    public async Task<List<CarFieldSetupGetterModel>> GetCarFieldSetup(int carId)
    {
        var carFields = await _context.CarFields
            .Where(x => x.CarId == carId && !x.IsDeleted)
            .OrderBy(x => x.Order)
            .ToListAsync();

        var setups = await _context.CarFieldSetups
            .Where(x => x.CarId == carId && !x.IsDeleted)
            .ToListAsync();

        var result = new List<CarFieldSetupGetterModel>();

        foreach (var field in carFields)
        {
            var setup = setups.FirstOrDefault(x => x.CarFieldId == field.Id);
            result.Add(new CarFieldSetupGetterModel
            {
                CarFieldId = field.Id,
                Name = field.Name,
                Order = field.Order,
                ValueNumber = setup?.ValueNumber,
                FromAt = setup?.FromAt,
                ToAt = setup?.ToAt,
                WarningAt = setup?.WarningAt,
                UserIdString = setup?.UserIdString,
                Note = setup?.Note,
                FileLink = setup?.FileLink
            });
        }

        return result;
    }

    public async Task UpdateCarFieldSetup(int carId, List<CarFieldSetupModel> carFieldSetups)
    {
        var existingSetups = await _context.CarFieldSetups
            .Where(x => x.CarId == carId)
            .ToListAsync();

        _context.CarFieldSetups.RemoveRange(existingSetups);

        foreach (var setup in carFieldSetups)
        {
            _context.CarFieldSetups.Add(new CarFieldSetup
            {
                CarId = carId,
                CarFieldId = setup.CarFieldId,
                ValueNumber = setup.ValueNumber,
                FromAt = setup.FromAt,
                ToAt = setup.ToAt,
                WarningAt = setup.WarningAt,
                UserIdString = setup.UserIdString,
                Note = setup.Note,
                FileLink = setup.FileLink,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });
        }

        await _context.SaveChangesAsync();
    }

    private static string? SerializeFiles(List<string>? files)
    {
        return files == null ? null : JsonConvert.SerializeObject(files);
    }

    private static List<string>? DeserializeFiles(string? fileLink)
    {
        if (string.IsNullOrEmpty(fileLink))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<List<string>>(fileLink);
    }
}

