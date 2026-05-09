using AciPlatform.Application.Interfaces.HotelManagement;
using AciPlatform.Domain.Entities.Hotel;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Services.HotelManagement;

public class HotelVehicleService : IHotelVehicleService
{
    private readonly HotelDbContext _db;
    public HotelVehicleService(HotelDbContext db) => _db = db;

    public async Task<List<HotelVehicleDto>> GetVehiclesAsync(string hotelCode, string? status = null)
    {
        var q = _db.HotelVehicles.Where(v => v.HotelCode == hotelCode);
        if (status != null) q = q.Where(v => v.Status == status);
        return await q.OrderBy(v => v.VehicleType).ThenBy(v => v.VehicleCode)
            .Select(v => ToDto(v)).ToListAsync();
    }

    public async Task<HotelVehicleDto?> GetVehicleByCodeAsync(string hotelCode, string code)
    {
        var v = await _db.HotelVehicles.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.VehicleCode == code);
        return v == null ? null : ToDto(v);
    }

    public async Task<HotelVehicleDto> CreateVehicleAsync(CreateVehicleRequest req)
    {
        var v = new HotelVehicle
        {
            HotelCode = req.HotelCode, VehicleCode = req.VehicleCode, BienSo = req.BienSo,
            TenXe = req.TenXe, VehicleType = req.VehicleType, ServiceCode = req.ServiceCode,
            Brand = req.Brand, Model = req.Model, Color = req.Color, YearMade = req.YearMade,
            PricePerDay = req.PricePerDay, DepositRequired = req.DepositRequired, Notes = req.Notes
        };
        _db.HotelVehicles.Add(v);
        await _db.SaveChangesAsync();
        return ToDto(v);
    }

    public async Task<HotelVehicleDto> UpdateVehicleAsync(int id, CreateVehicleRequest req)
    {
        var v = await _db.HotelVehicles.FindAsync(id) ?? throw new InvalidOperationException("Vehicle not found.");
        v.BienSo = req.BienSo; v.TenXe = req.TenXe; v.VehicleType = req.VehicleType;
        v.Brand = req.Brand; v.Model = req.Model; v.Color = req.Color; v.YearMade = req.YearMade;
        v.PricePerDay = req.PricePerDay; v.DepositRequired = req.DepositRequired;
        v.Notes = req.Notes; v.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ToDto(v);
    }

    public async Task DeleteVehicleAsync(int id)
    {
        var v = await _db.HotelVehicles.FindAsync(id);
        if (v != null) { v.IsDeleted = true; v.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    public async Task UpdateVehicleStatusAsync(string hotelCode, string vehicleCode, string status, int? fuelLevel, string? condition)
    {
        var v = await _db.HotelVehicles.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.VehicleCode == vehicleCode)
            ?? throw new InvalidOperationException("Vehicle not found.");
        v.Status = status;
        if (fuelLevel.HasValue) v.FuelLevel = fuelLevel.Value;
        if (condition != null) v.Condition = condition;
        v.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
    }

    public async Task<HotelVehicleRentalDto> CreateRentalAsync(CreateRentalRequest req)
    {
        var vehicle = await _db.HotelVehicles
            .FirstOrDefaultAsync(v => v.HotelCode == req.HotelCode && v.VehicleCode == req.VehicleCode)
            ?? throw new InvalidOperationException($"Vehicle '{req.VehicleCode}' not found.");
        if (vehicle.Status != "AVAILABLE")
            throw new InvalidOperationException($"Vehicle '{req.VehicleCode}' is not available (Status: {vehicle.Status}).");

        var today = DateTime.Today.ToString("yyyyMMdd");
        var count = await _db.HotelVehicleRentals.CountAsync(r => r.HotelCode == req.HotelCode && r.CreatedDate.Date == DateTime.Today);
        var days = Math.Max(1, (decimal)(req.RentTo - req.RentFrom).TotalDays);
        var total = vehicle.PricePerDay * days;

        var rental = new HotelVehicleRental
        {
            HotelCode = req.HotelCode, RentalCode = $"VR-{today}-{(count + 1):D3}",
            BookingId = req.BookingId, VehicleCode = req.VehicleCode,
            GuestName = req.GuestName, GuestPhone = req.GuestPhone, GuestIdCard = req.GuestIdCard,
            RentFrom = req.RentFrom, RentTo = req.RentTo, TotalDays = days,
            PricePerDay = vehicle.PricePerDay, TotalAmount = total,
            DepositAmount = req.DepositAmount, FuelLevelOut = req.FuelLevelOut,
            ConditionOut = req.ConditionOut, Notes = req.Notes, CreatedBy = req.CreatedBy
        };
        _db.HotelVehicleRentals.Add(rental);
        vehicle.Status = "RENTED"; vehicle.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ToRentalDto(rental, vehicle);
    }

    public async Task<HotelVehicleRentalDto> ReturnVehicleAsync(ReturnVehicleRequest req)
    {
        var rental = await _db.HotelVehicleRentals.FindAsync(req.RentalId)
            ?? throw new InvalidOperationException("Rental not found.");
        var vehicle = await _db.HotelVehicles.FirstOrDefaultAsync(v =>
            v.HotelCode == rental.HotelCode && v.VehicleCode == rental.VehicleCode);

        rental.ActualReturnDate = req.ActualReturnDate;
        rental.FuelLevelIn = req.FuelLevelIn;
        rental.ConditionIn = req.ConditionIn;
        rental.DamageFee = req.DamageFee;
        rental.DepositReturned = req.DepositReturned;
        rental.DamageNotes = req.DamageNotes;
        rental.Status = "RETURNED";
        rental.UpdatedDate = DateTime.Now;

        if (vehicle != null)
        {
            vehicle.Status = "AVAILABLE";
            vehicle.FuelLevel = req.FuelLevelIn;
            vehicle.Condition = req.ConditionIn;
            vehicle.UpdatedDate = DateTime.Now;
        }
        await _db.SaveChangesAsync();
        return ToRentalDto(rental, vehicle);
    }

    public async Task<List<HotelVehicleRentalDto>> GetActiveRentalsAsync(string hotelCode)
    {
        var rentals = await _db.HotelVehicleRentals.Where(r => r.HotelCode == hotelCode && r.Status == "ACTIVE")
            .OrderBy(r => r.RentTo).ToListAsync();
        var vehicles = await _db.HotelVehicles.Where(v => v.HotelCode == hotelCode).ToListAsync();
        return rentals.Select(r => ToRentalDto(r, vehicles.FirstOrDefault(v => v.VehicleCode == r.VehicleCode))).ToList();
    }

    public async Task<List<HotelVehicleRentalDto>> GetRentalHistoryAsync(string hotelCode, DateTime? from, DateTime? to)
    {
        var q = _db.HotelVehicleRentals.Where(r => r.HotelCode == hotelCode);
        if (from.HasValue) q = q.Where(r => r.RentFrom >= from.Value);
        if (to.HasValue) q = q.Where(r => r.RentFrom <= to.Value);
        var rentals = await q.OrderByDescending(r => r.CreatedDate).ToListAsync();
        var vehicles = await _db.HotelVehicles.Where(v => v.HotelCode == hotelCode).ToListAsync();
        return rentals.Select(r => ToRentalDto(r, vehicles.FirstOrDefault(v => v.VehicleCode == r.VehicleCode))).ToList();
    }

    public async Task<HotelVehicleRentalDto?> GetRentalByIdAsync(int id)
    {
        var r = await _db.HotelVehicleRentals.FindAsync(id);
        if (r == null) return null;
        var v = await _db.HotelVehicles.FirstOrDefaultAsync(x => x.HotelCode == r.HotelCode && x.VehicleCode == r.VehicleCode);
        return ToRentalDto(r, v);
    }

    public async Task<List<HotelVehicleDto>> GetAvailableVehiclesAsync(string hotelCode, DateTime from, DateTime to, string? vehicleType)
    {
        var rentedCodes = await _db.HotelVehicleRentals
            .Where(r => r.HotelCode == hotelCode && r.Status == "ACTIVE"
                && r.RentFrom < to && r.RentTo > from)
            .Select(r => r.VehicleCode).ToListAsync();
        var q = _db.HotelVehicles.Where(v => v.HotelCode == hotelCode && v.IsActive && !rentedCodes.Contains(v.VehicleCode));
        if (vehicleType != null) q = q.Where(v => v.VehicleType == vehicleType);
        return await q.Select(v => ToDto(v)).ToListAsync();
    }

    private static HotelVehicleDto ToDto(HotelVehicle v) => new()
    {
        Id = v.Id, VehicleCode = v.VehicleCode, BienSo = v.BienSo, TenXe = v.TenXe,
        VehicleType = v.VehicleType, Brand = v.Brand, Model = v.Model, Color = v.Color,
        YearMade = v.YearMade, PricePerDay = v.PricePerDay, DepositRequired = v.DepositRequired,
        FuelLevel = v.FuelLevel, Condition = v.Condition, Status = v.Status, Notes = v.Notes
    };

    private static HotelVehicleRentalDto ToRentalDto(HotelVehicleRental r, HotelVehicle? v) => new()
    {
        Id = r.Id, RentalCode = r.RentalCode, VehicleCode = r.VehicleCode,
        BienSo = v?.BienSo, TenXe = v?.TenXe,
        GuestName = r.GuestName, GuestPhone = r.GuestPhone,
        RentFrom = r.RentFrom, RentTo = r.RentTo, ActualReturnDate = r.ActualReturnDate,
        TotalDays = r.TotalDays, PricePerDay = r.PricePerDay, TotalAmount = r.TotalAmount,
        DepositAmount = r.DepositAmount, DepositReturned = r.DepositReturned, DamageFee = r.DamageFee,
        Status = r.Status, IsOverdue = r.Status == "ACTIVE" && r.RentTo < DateTime.Now
    };
}

public class HotelGuestService : IHotelGuestService
{
    private readonly HotelDbContext _db;
    public HotelGuestService(HotelDbContext db) => _db = db;

    public async Task<List<HotelGuestDto>> SearchGuestsAsync(string hotelCode, string? kw, int page = 1)
    {
        var q = _db.HotelGuests.Where(g => g.HotelCode == hotelCode);
        if (!string.IsNullOrEmpty(kw))
            q = q.Where(g => g.FullName.Contains(kw) || g.Phone!.Contains(kw) || g.IdCard!.Contains(kw));
        return await q.OrderByDescending(g => g.TotalVisits).Skip((page - 1) * 20).Take(20)
            .Select(g => ToDto(g)).ToListAsync();
    }

    public async Task<HotelGuestDto?> GetGuestByPhoneAsync(string hotelCode, string phone)
    {
        var g = await _db.HotelGuests.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.Phone == phone);
        return g == null ? null : ToDto(g);
    }

    public async Task<HotelGuestDto?> GetGuestByIdAsync(int id)
    {
        var g = await _db.HotelGuests.FindAsync(id);
        return g == null ? null : ToDto(g);
    }

    public async Task<HotelGuestDto> UpsertGuestAsync(UpsertGuestRequest req)
    {
        var g = await _db.HotelGuests.FirstOrDefaultAsync(x => x.HotelCode == req.HotelCode && x.Phone == req.Phone);
        if (g == null)
        {
            g = new HotelGuest { HotelCode = req.HotelCode };
            _db.HotelGuests.Add(g);
        }
        g.FullName = req.FullName; g.Phone = req.Phone; g.Email = req.Email;
        g.IdCard = req.IdCard; g.IdType = req.IdType; g.Nationality = req.Nationality;
        g.Address = req.Address; g.Notes = req.Notes; g.Source = req.Source;
        g.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ToDto(g);
    }

    public async Task<List<BookingDto>> GetGuestBookingHistoryAsync(int guestId)
    {
        var g = await _db.HotelGuests.FindAsync(guestId);
        if (g?.Phone == null) return new();
        var bookings = await _db.HotelBookings
            .Where(b => b.HotelCode == g.HotelCode && b.GuestPhone == g.Phone)
            .Include(b => b.Rooms).Include(b => b.Services)
            .OrderByDescending(b => b.CheckIn).Take(20).ToListAsync();
        // Use mapper from booking service - direct mapping here
        return bookings.Select(b => new BookingDto { Id = b.Id, BookingCode = b.BookingCode,
            GuestName = b.GuestName, CheckIn = b.CheckIn, CheckOut = b.CheckOut,
            TotalAmount = b.TotalAmount, Status = b.Status }).ToList();
    }

    public async Task DeleteGuestAsync(int id)
    {
        var g = await _db.HotelGuests.FindAsync(id);
        if (g != null) { g.IsDeleted = true; g.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    private static HotelGuestDto ToDto(HotelGuest g) => new()
    {
        Id = g.Id, GuestCode = g.GuestCode, FullName = g.FullName, Phone = g.Phone,
        Email = g.Email, IdCard = g.IdCard, IdType = g.IdType, Nationality = g.Nationality,
        Address = g.Address, PreferRoomType = g.PreferRoomType, PreferVehicle = g.PreferVehicle,
        TotalVisits = g.TotalVisits, TotalSpend = g.TotalSpend, LastVisitDate = g.LastVisitDate,
        Source = g.Source, IsVIP = g.IsVIP, Notes = g.Notes
    };
}

public class HotelServiceCatalogService : IHotelServiceCatalogService
{
    private readonly HotelDbContext _db;
    public HotelServiceCatalogService(HotelDbContext db) => _db = db;

    public async Task<List<HotelServiceDto>> GetServicesAsync(string hotelCode, string? category = null)
    {
        var q = _db.HotelServices.Where(s => s.HotelCode == hotelCode);
        if (category != null) q = q.Where(s => s.Category == category);
        return await q.OrderBy(s => s.SortOrder).ThenBy(s => s.ServiceName)
            .Select(s => ToDto(s)).ToListAsync();
    }

    public async Task<HotelServiceDto?> GetServiceByCodeAsync(string hotelCode, string code)
    {
        var s = await _db.HotelServices.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.ServiceCode == code);
        return s == null ? null : ToDto(s);
    }

    public async Task<HotelServiceDto> UpsertServiceAsync(UpsertServiceRequest req)
    {
        var s = await _db.HotelServices.FirstOrDefaultAsync(x => x.HotelCode == req.HotelCode && x.ServiceCode == req.ServiceCode);
        if (s == null) { s = new HotelService { HotelCode = req.HotelCode, ServiceCode = req.ServiceCode }; _db.HotelServices.Add(s); }
        s.ServiceName = req.ServiceName; s.ServiceNameEN = req.ServiceNameEN;
        s.Category = req.Category; s.SubCategory = req.SubCategory;
        s.Description = req.Description; s.Unit = req.Unit; s.UnitPrice = req.UnitPrice;
        s.IsAvailable = req.IsAvailable; s.SortOrder = req.SortOrder; s.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ToDto(s);
    }

    public async Task DeleteServiceAsync(int id)
    {
        var s = await _db.HotelServices.FindAsync(id);
        if (s != null) { s.IsDeleted = true; s.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    public async Task ToggleAvailabilityAsync(string hotelCode, string serviceCode, bool isAvailable)
    {
        var s = await _db.HotelServices.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.ServiceCode == serviceCode);
        if (s != null) { s.IsAvailable = isAvailable; s.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    private static HotelServiceDto ToDto(HotelService s) => new()
    {
        Id = s.Id, ServiceCode = s.ServiceCode, ServiceName = s.ServiceName, ServiceNameEN = s.ServiceNameEN,
        Category = s.Category, SubCategory = s.SubCategory, Description = s.Description,
        Unit = s.Unit, UnitPrice = s.UnitPrice, IsAvailable = s.IsAvailable, SortOrder = s.SortOrder
    };
}
