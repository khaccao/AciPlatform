using AciPlatform.Application.Interfaces.HotelManagement;
using AciPlatform.Domain.Entities.Hotel;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Services.HotelManagement;

public class HotelRoomService : IHotelRoomService
{
    private readonly HotelDbContext _db;
    public HotelRoomService(HotelDbContext db) => _db = db;

    public async Task<List<RoomStatusDto>> GetRoomStatusMapAsync(string hotelCode)
    {
        var rooms = await _db.PmsRooms.Where(r => r.HotelCode == hotelCode && r.IsActive)
            .OrderBy(r => r.Floor).ThenBy(r => r.So).ToListAsync();
        var beds = await _db.HotelBeds.Where(b => b.HotelCode == hotelCode && b.IsActive).ToListAsync();
        var today = DateOnly.FromDateTime(DateTime.Today);
        var forecasts = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.ForecastDate == today).ToListAsync();
        var activeBookings = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_IN" && b.CheckOut >= DateTime.Today)
            .Include(b => b.Rooms).ToListAsync();

        return rooms.Select(r =>
        {
            var roomBeds = beds.Where(b => b.RoomNo == r.So).OrderBy(b => b.SortOrder).ToList();
            var bookingForRoom = activeBookings.FirstOrDefault(b =>
                b.Rooms.Any(br => br.RoomNo == r.So && br.BedCode == null));
            return new RoomStatusDto
            {
                RoomNo = r.So ?? "",
                RoomType = r.Ma,
                Floor = r.Floor,
                Status = bookingForRoom != null ? "OCCUPIED" : (r.Status ?? "VACANT"),
                CleanDirty = r.CleanDirty,
                Inspected = r.Inspected,
                CurrentGuest = bookingForRoom?.GuestName,
                CheckOut = bookingForRoom?.CheckOut,
                IsCheckoutToday = bookingForRoom?.CheckOut.Date == DateTime.Today,
                Beds = roomBeds.Select(b => new BedStatusDto
                {
                    BedCode = b.BedCode,
                    BedName = b.BedName,
                    BedType = b.BedType,
                    Status = forecasts.Any(f => f.RoomNo == r.So && f.BedCode == b.BedCode) ? "OCCUPIED" : "VACANT"
                }).ToList()
            };
        }).ToList();
    }

    public async Task<List<RoomAvailabilityResult>> CheckAvailabilityAsync(RoomAvailabilityRequest req)
    {
        var from = DateOnly.FromDateTime(req.CheckIn.Date);
        var to = DateOnly.FromDateTime(req.CheckOut.Date.AddDays(-1));
        var blocked = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == req.HotelCode && f.ForecastDate >= from && f.ForecastDate <= to)
            .ToListAsync();
        var rooms = await _db.PmsRooms
            .Where(r => r.HotelCode == req.HotelCode && r.IsActive && (req.RoomType == null || r.Ma == req.RoomType))
            .ToListAsync();
        var beds = await _db.HotelBeds.Where(b => b.HotelCode == req.HotelCode && b.IsActive).ToListAsync();
        var pricing = await _db.HotelSeasonalPricings
            .Where(p => p.HotelCode == req.HotelCode && p.IsActive && p.StartDate <= to && p.EndDate >= from)
            .FirstOrDefaultAsync();
        var multiplier = pricing?.PriceMultiplier ?? 1.0m;

        return rooms.Select(r =>
        {
            var roomBeds = beds.Where(b => b.RoomNo == r.So).ToList();
            var roomBlocked = blocked.Any(f => f.RoomNo == r.So && f.BedCode == null);
            var basePrice = r.Ma == "KHEPKIN" ? 250000m : 100000m;
            return new RoomAvailabilityResult
            {
                RoomNo = r.So ?? "",
                RoomType = r.Ma,
                Floor = r.Floor,
                IsAvailable = !roomBlocked,
                PricePerNight = basePrice * multiplier,
                Beds = roomBeds.Select(b => new BedAvailability
                {
                    BedCode = b.BedCode,
                    BedName = b.BedName,
                    IsAvailable = !roomBlocked && !blocked.Any(f => f.RoomNo == r.So && f.BedCode == b.BedCode)
                }).ToList()
            };
        }).ToList();
    }

    public async Task<List<object>> GetRoomForecastAsync(string hotelCode, DateTime fromDate, DateTime toDate)
    {
        var from = DateOnly.FromDateTime(fromDate);
        var to = DateOnly.FromDateTime(toDate);
        var rooms = await _db.PmsRooms.Where(r => r.HotelCode == hotelCode && r.IsActive)
            .OrderBy(r => r.Floor).ThenBy(r => r.So).ToListAsync();
        var forecasts = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.ForecastDate >= from && f.ForecastDate <= to).ToListAsync();
        var bookings = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status != "CANCELLED"
                && b.CheckIn.Date <= toDate && b.CheckOut.Date >= fromDate)
            .Include(b => b.Rooms).ToListAsync();
        var days = (to.DayNumber - from.DayNumber) + 1;

        return rooms.Select(r => (object)new
        {
            RoomNo = r.So,
            RoomType = r.Ma,
            Floor = r.Floor,
            Blocks = Enumerable.Range(0, days).Select(d =>
            {
                var date = from.AddDays(d);
                var bk = bookings.FirstOrDefault(b =>
                    b.Rooms.Any(br => br.RoomNo == r.So && br.BedCode == null
                        && DateOnly.FromDateTime(br.CheckIn) <= date
                        && DateOnly.FromDateTime(br.CheckOut) > date));
                var fc = forecasts.FirstOrDefault(f => f.RoomNo == r.So && f.BedCode == null && f.ForecastDate == date);
                return new { Date = date, IsBlocked = bk != null || fc != null,
                    BlockType = bk != null ? "BOOKING" : fc?.BlockType, BookingCode = bk?.BookingCode, GuestName = bk?.GuestName };
            })
        }).ToList();
    }

    public async Task BlockRoomAsync(BlockRoomRequest req)
    {
        var from = DateOnly.FromDateTime(req.FromDate);
        var to = DateOnly.FromDateTime(req.ToDate.AddDays(-1));
        for (var d = from; d <= to; d = d.AddDays(1))
        {
            if (!await _db.HotelRoomForecasts.AnyAsync(f =>
                f.HotelCode == req.HotelCode && f.RoomNo == req.RoomNo
                && f.BedCode == req.BedCode && f.ForecastDate == d))
            {
                _db.HotelRoomForecasts.Add(new HotelRoomForecast
                {
                    HotelCode = req.HotelCode, RoomNo = req.RoomNo, BedCode = req.BedCode,
                    ForecastDate = d, BlockType = req.BlockType, BlockNote = req.Note, CreatedBy = req.UserId
                });
            }
        }
        await _db.SaveChangesAsync();
    }

    public async Task UnblockRoomAsync(string hotelCode, string roomNo, string? bedCode, DateTime fromDate, DateTime toDate)
    {
        var from = DateOnly.FromDateTime(fromDate);
        var to = DateOnly.FromDateTime(toDate);
        var records = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.RoomNo == roomNo && f.BedCode == bedCode
                && f.ForecastDate >= from && f.ForecastDate <= to && f.BookingId == null)
            .ToListAsync();
        _db.HotelRoomForecasts.RemoveRange(records);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateRoomStatusAsync(UpdateRoomStatusRequest req)
    {
        var room = await _db.PmsRooms.FirstOrDefaultAsync(r => r.HotelCode == req.HotelCode && r.So == req.RoomNo)
            ?? throw new InvalidOperationException($"Room {req.RoomNo} not found.");
        if (req.CleanDirty.HasValue) room.CleanDirty = req.CleanDirty;
        if (req.Inspected.HasValue) room.Inspected = req.Inspected;
        if (req.Status != null) room.Status = req.Status;
        await _db.SaveChangesAsync();
    }

    public async Task<List<BedStatusDto>> GetBedsByRoomAsync(string hotelCode, string roomNo)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var beds = await _db.HotelBeds
            .Where(b => b.HotelCode == hotelCode && b.RoomNo == roomNo && b.IsActive)
            .OrderBy(b => b.SortOrder).ToListAsync();
        var forecasts = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.RoomNo == roomNo && f.ForecastDate == today).ToListAsync();
        return beds.Select(b => new BedStatusDto
        {
            BedCode = b.BedCode, BedName = b.BedName, BedType = b.BedType,
            Status = forecasts.Any(f => f.BedCode == b.BedCode) ? "OCCUPIED" : "VACANT"
        }).ToList();
    }

    public async Task<BedStatusDto> UpsertBedAsync(string hotelCode, string roomNo, string bedCode, string bedName, string bedType)
    {
        var bed = await _db.HotelBeds.FirstOrDefaultAsync(b =>
            b.HotelCode == hotelCode && b.RoomNo == roomNo && b.BedCode == bedCode);
        if (bed == null) { bed = new HotelBed { HotelCode = hotelCode, RoomNo = roomNo, BedCode = bedCode }; _db.HotelBeds.Add(bed); }
        bed.BedName = bedName; bed.BedType = bedType;
        await _db.SaveChangesAsync();
        return new BedStatusDto { BedCode = bed.BedCode, BedName = bed.BedName, BedType = bed.BedType, Status = "VACANT" };
    }

    public async Task DeleteBedAsync(string hotelCode, string roomNo, string bedCode)
    {
        var bed = await _db.HotelBeds.FirstOrDefaultAsync(b =>
            b.HotelCode == hotelCode && b.RoomNo == roomNo && b.BedCode == bedCode);
        if (bed != null) { _db.HotelBeds.Remove(bed); await _db.SaveChangesAsync(); }
    }
}
