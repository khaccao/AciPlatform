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
        var bookingIds = forecasts.Where(f => f.BookingId.HasValue).Select(f => f.BookingId!.Value).Distinct().ToList();
        var forecastBookings = await _db.HotelBookings
            .Where(b => bookingIds.Contains(b.Id) && b.Status != "CANCELLED")
            .ToListAsync();
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
                    GuestName = forecastBookings.FirstOrDefault(x =>
                        forecasts.Any(f => f.BookingId == x.Id && f.RoomNo == r.So && f.BedCode == b.BedCode))?.GuestName,
                    BedCode = b.BedCode,
                    BedName = b.BedName,
                    BedType = b.BedType,
                    Status = forecasts.Any(f => f.RoomNo == r.So && f.BedCode == b.BedCode) ? "OC" : b.Status,
                    IsAvailable = !forecasts.Any(f => f.RoomNo == r.So && f.BedCode == b.BedCode) && b.Status != "OOS"
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
            var roomBlocked = blocked.Any(f => f.RoomNo == r.So);
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
                    BedType = b.BedType,
                    Status = b.Status,
                    IsAvailable = !blocked.Any(f => f.RoomNo == r.So && f.BedCode == null)
                        && b.Status != "OOS"
                        && !blocked.Any(f => f.RoomNo == r.So && f.BedCode == b.BedCode)
                }).ToList()
            };
        }).ToList();
    }    public async Task<List<object>> GetRoomForecastAsync(string hotelCode, DateTime fromDate, DateTime toDate)
    {
        var result = new List<object>();
        using var cmd = _db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SP_GetRoomForecast";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        var p1 = cmd.CreateParameter(); p1.ParameterName = "@HotelCode"; p1.Value = hotelCode; cmd.Parameters.Add(p1);
        var p2 = cmd.CreateParameter(); p2.ParameterName = "@FromDate"; p2.Value = fromDate; cmd.Parameters.Add(p2);
        var p3 = cmd.CreateParameter(); p3.ParameterName = "@ToDate"; p3.Value = toDate; cmd.Parameters.Add(p3);

        if (cmd.Connection!.State != System.Data.ConnectionState.Open)
            await cmd.Connection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        var rows = new List<dynamic>();
        while (await reader.ReadAsync())
        {
            rows.Add(new
            {
                RoomType = reader["RoomType"]?.ToString(),
                RoomTypeName = reader["RoomTypeName"]?.ToString(),
                TotalRooms = reader["TotalRooms"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalRooms"]),
                Date = reader["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["Date"]),
                AvailableCount = reader["AvailableCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["AvailableCount"])
            });
        }

        var types = rows.Select(r => new { r.RoomType, r.RoomTypeName, r.TotalRooms }).Distinct().ToList();
        foreach (var type in types)
        {
            var dates = rows.Where(r => r.RoomType == type.RoomType)
                .Select(r => new { Date = r.Date, AvailableCount = r.AvailableCount })
                .OrderBy(r => r.Date).ToList();
            
            result.Add(new
            {
                RoomType = type.RoomType,
                RoomTypeName = type.RoomTypeName,
                TotalRooms = type.TotalRooms,
                Dates = dates
            });
        }

        return result;
    }

    public async Task<RoomRackDto> GetRoomRackAsync(string hotelCode, DateTime fromDate, int days)
    {
        days = Math.Clamp(days, 7, 62);
        var from = DateOnly.FromDateTime(fromDate.Date);
        var to = from.AddDays(days - 1);
        var today = DateOnly.FromDateTime(DateTime.Today);

        var rooms = await _db.PmsRooms
            .Where(r => r.HotelCode == hotelCode && r.IsActive)
            .OrderBy(r => r.Floor).ThenBy(r => r.So)
            .ToListAsync();
        var roomTypes = await _db.PmsRoomTypes.Where(t => t.HotelCode == hotelCode).ToListAsync();
        var bookingRooms = await _db.HotelBookingRooms
            .Where(br => br.HotelCode == hotelCode
                && br.CheckIn.Date <= to.ToDateTime(TimeOnly.MinValue)
                && br.CheckOut.Date >= from.ToDateTime(TimeOnly.MinValue))
            .ToListAsync();
        var bookingIds = bookingRooms.Select(br => br.BookingId).Distinct().ToList();
        var bookings = await _db.HotelBookings
            .Where(b => bookingIds.Contains(b.Id) && b.Status != "CANCELLED")
            .ToListAsync();
        var forecasts = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.ForecastDate >= from && f.ForecastDate <= to)
            .ToListAsync();

        var dates = Enumerable.Range(0, days)
            .Select(i =>
            {
                var date = from.AddDays(i);
                return new RoomRackDateDto
                {
                    Date = date,
                    Label = date.ToString("dd/MM"),
                    IsToday = date == today,
                    IsWeekend = date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
                };
            }).ToList();

        var rackRooms = rooms.Select(room =>
        {
            var roomType = roomTypes.FirstOrDefault(t => t.Ma == room.Ma);
            var cells = dates.Select(d =>
            {
                var bookingRoom = bookingRooms.FirstOrDefault(br =>
                    br.RoomNo == room.So
                    && DateOnly.FromDateTime(br.CheckIn.Date) <= d.Date
                    && DateOnly.FromDateTime(br.CheckOut.Date) > d.Date
                    && bookings.Any(b => b.Id == br.BookingId));
                var booking = bookingRoom == null ? null : bookings.FirstOrDefault(b => b.Id == bookingRoom.BookingId);
                var forecast = forecasts.FirstOrDefault(f => f.RoomNo == room.So && f.BedCode == null && f.ForecastDate == d.Date);

                if (booking != null && bookingRoom != null)
                {
                    var checkInDate = DateOnly.FromDateTime(bookingRoom.CheckIn.Date);
                    var checkOutDate = DateOnly.FromDateTime(bookingRoom.CheckOut.Date);
                    return new RoomRackCellDto
                    {
                        Date = d.Date,
                        Status = booking.Status,
                        BookingId = booking.Id,
                        BookingCode = booking.BookingCode,
                        GuestName = bookingRoom.GuestName ?? booking.GuestName,
                        GuestPhone = booking.GuestPhone,
                        CheckIn = bookingRoom.CheckIn,
                        CheckOut = bookingRoom.CheckOut,
                        TotalAmount = booking.TotalAmount,
                        PaidAmount = booking.PaidAmount,
                        Source = booking.Source,
                        IsStart = checkInDate == d.Date,
                        IsEnd = checkOutDate.AddDays(-1) == d.Date,
                        SpanDays = Math.Max(1, checkOutDate.DayNumber - checkInDate.DayNumber)
                    };
                }

                if (forecast != null)
                {
                    return new RoomRackCellDto
                    {
                        Date = d.Date,
                        Status = forecast.BlockType,
                        BlockType = forecast.BlockType,
                        Note = forecast.BlockNote
                    };
                }

                return new RoomRackCellDto { Date = d.Date, Status = room.Status ?? "VACANT" };
            }).ToList();

            return new RoomRackRoomDto
            {
                Id = room.Id,
                RoomNo = room.So ?? "",
                RoomType = room.Ma,
                RoomTypeName = roomType?.Ten,
                Floor = room.Floor,
                Status = room.Status ?? "VACANT",
                Cells = cells
            };
        }).ToList();

        return new RoomRackDto { FromDate = from, ToDate = to, Dates = dates, Rooms = rackRooms };
    }

    public async Task MoveRoomRackBookingAsync(string hotelCode, MoveRoomRackBookingRequest req)
    {
        var booking = await _db.HotelBookings
            .Include(b => b.Rooms)
            .FirstOrDefaultAsync(b => b.Id == req.BookingId && b.HotelCode == hotelCode)
            ?? throw new InvalidOperationException("Booking not found.");
        var room = await _db.PmsRooms.FirstOrDefaultAsync(r => r.HotelCode == hotelCode && r.So == req.ToRoomNo && r.IsActive)
            ?? throw new InvalidOperationException("Target room not found.");
        var bookingRoom = booking.Rooms.FirstOrDefault(r => r.RoomNo == req.FromRoomNo)
            ?? booking.Rooms.FirstOrDefault()
            ?? throw new InvalidOperationException("Booking room not found.");

        bookingRoom.RoomNo = req.ToRoomNo;
        bookingRoom.RoomType = room.Ma;
        if (req.CheckIn.HasValue) bookingRoom.CheckIn = req.CheckIn.Value;
        if (req.CheckOut.HasValue) bookingRoom.CheckOut = req.CheckOut.Value;
        bookingRoom.NightCount = Math.Max(1, (bookingRoom.CheckOut.Date - bookingRoom.CheckIn.Date).Days);

        if (booking.Rooms.Count == 1)
        {
            booking.CheckIn = bookingRoom.CheckIn;
            booking.CheckOut = bookingRoom.CheckOut;
            booking.NightCount = bookingRoom.NightCount;
        }

        var forecasts = await _db.HotelRoomForecasts
            .Where(f => f.HotelCode == hotelCode && f.BookingId == booking.Id && f.RoomNo == req.FromRoomNo)
            .ToListAsync();
        foreach (var forecast in forecasts)
        {
            forecast.RoomNo = req.ToRoomNo;
        }

        booking.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
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
            Status = forecasts.Any(f => f.BedCode == b.BedCode) ? "OC" : b.Status,
            IsAvailable = !forecasts.Any(f => f.BedCode == b.BedCode) && b.Status != "OOS"
        }).ToList();
    }

    public async Task<BedStatusDto> UpsertBedAsync(string hotelCode, string roomNo, string bedCode, string bedName, string bedType, string? status = null)
    {
        var bed = await _db.HotelBeds.FirstOrDefaultAsync(b =>
            b.HotelCode == hotelCode && b.RoomNo == roomNo && b.BedCode == bedCode);
        if (bed == null) { bed = new HotelBed { HotelCode = hotelCode, RoomNo = roomNo, BedCode = bedCode }; _db.HotelBeds.Add(bed); }
        bed.BedName = bedName; bed.BedType = bedType;
        if (!string.IsNullOrWhiteSpace(status)) bed.Status = status;
        await _db.SaveChangesAsync();
        return new BedStatusDto { BedCode = bed.BedCode, BedName = bed.BedName, BedType = bed.BedType, Status = bed.Status, IsAvailable = bed.Status != "OOS" };
    }

    public async Task UpdateBedStatusAsync(UpdateBedStatusRequest req)
    {
        var bed = await _db.HotelBeds.FirstOrDefaultAsync(b =>
            b.HotelCode == req.HotelCode && b.RoomNo == req.RoomNo && b.BedCode == req.BedCode)
            ?? throw new InvalidOperationException($"Bed {req.RoomNo}/{req.BedCode} not found.");
        bed.Status = req.Status;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteBedAsync(string hotelCode, string roomNo, string bedCode)
    {
        var bed = await _db.HotelBeds.FirstOrDefaultAsync(b =>
            b.HotelCode == hotelCode && b.RoomNo == roomNo && b.BedCode == bedCode);
        if (bed != null) { _db.HotelBeds.Remove(bed); await _db.SaveChangesAsync(); }
    }
}
