using AciPlatform.Application.Interfaces.HotelManagement;
using AciPlatform.Domain.Entities.Hotel;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Services.HotelManagement;

public class HotelBookingService : IHotelBookingService
{
    private readonly HotelDbContext _db;
    public HotelBookingService(HotelDbContext db) => _db = db;

    public async Task<BookingDto> CreateBookingAsync(CreateBookingRequest req)
    {
        if (req.Rooms.Count == 0)
            throw new InvalidOperationException("Booking must have at least one room or bed.");

        var nights = Math.Max(1, (int)(req.CheckOut.Date - req.CheckIn.Date).TotalDays);
        await ValidateBookingAvailabilityAsync(req);
        var guest = await SyncGuestProfileAsync(req);

        // Generate booking code
        var today = DateTime.Today.ToString("yyyyMMdd");
        var count = await _db.HotelBookings.CountAsync(b => b.HotelCode == req.HotelCode && b.CreatedDate.Date == DateTime.Today);
        var code = $"BK-{today}-{(count + 1):D3}";

        // Calculate prices from rooms
        var roomPrice = req.Rooms.Sum(r => r.PricePerNight * (r.NightCount > 0 ? r.NightCount : nights));
        var servicePrice = req.Services.Sum(s => s.UnitPrice * s.Quantity);
        var total = roomPrice + servicePrice - req.DiscountAmount;

        var booking = new HotelBooking
        {
            HotelCode = req.HotelCode,
            BookingCode = code,
            BookingType = req.BookingType,
            GuestId = guest.Id,
            GuestName = req.GuestName,
            GuestPhone = req.GuestPhone,
            GuestIdCard = req.GuestIdCard,
            Nationality = req.Nationality,
            CheckIn = req.CheckIn,
            CheckOut = req.CheckOut,
            NightCount = nights,
            RoomPrice = roomPrice,
            ServicePrice = servicePrice,
            DiscountAmount = req.DiscountAmount,
            TotalAmount = total,
            DepositAmount = req.DepositAmount,
            Status = "CONFIRMED",
            Source = req.Source,
            GroupName = req.GroupName,
            GroupSize = req.GroupSize,
            Notes = req.Notes,
            SpecialRequests = req.SpecialRequests,
            CreatedBy = req.CreatedBy
        };
        _db.HotelBookings.Add(booking);
        await _db.SaveChangesAsync();

        // Add rooms
        foreach (var r in req.Rooms)
        {
            _db.HotelBookingRooms.Add(new HotelBookingRoom
            {
                HotelCode = req.HotelCode,
                BookingId = booking.Id,
                RoomNo = r.RoomNo,
                BedCode = r.BedCode,
                GuestName = r.GuestName ?? req.GuestName,
                CheckIn = req.CheckIn,
                CheckOut = req.CheckOut,
                NightCount = nights,
                PricePerNight = r.PricePerNight,
                TotalPrice = r.PricePerNight * (r.NightCount > 0 ? r.NightCount : nights),
                Status = "BOOKED"
            });
        }

        // Add services (denormalize price at booking time)
        foreach (var s in req.Services)
        {
            var svc = await _db.HotelServices.FirstOrDefaultAsync(x =>
                x.HotelCode == req.HotelCode && x.ServiceCode == s.ServiceCode);
            _db.HotelBookingServices.Add(new AciPlatform.Domain.Entities.Hotel.HotelBookingService
            {
                HotelCode = req.HotelCode,
                BookingId = booking.Id,
                ServiceCode = s.ServiceCode,
                ServiceName = svc?.ServiceName,
                Category = svc?.Category,
                Quantity = s.Quantity,
                Unit = svc?.Unit,
                UnitPrice = s.UnitPrice,
                TotalPrice = s.UnitPrice * s.Quantity,
                ServiceDate = s.ServiceDate,
                Notes = s.Notes
            });
        }
        await _db.SaveChangesAsync();

        await BlockForecastForBookingAsync(booking.Id, req);

        return await GetBookingByIdAsync(booking.Id) ?? throw new Exception("Created booking not found");
    }

    private async Task<HotelGuest> SyncGuestProfileAsync(CreateBookingRequest req)
    {
        var phone = req.GuestPhone?.Trim();
        var idCard = req.GuestIdCard?.Trim();
        var g = await _db.HotelGuests.FirstOrDefaultAsync(x => x.HotelCode == req.HotelCode &&
            ((!string.IsNullOrEmpty(phone) && x.Phone == phone) ||
             (!string.IsNullOrEmpty(idCard) && x.IdCard == idCard)));
        
        if (g == null)
        {
            g = new HotelGuest { HotelCode = req.HotelCode, CreatedDate = DateTime.Now };
            _db.HotelGuests.Add(g);
        }
        
        g.FullName = req.GuestName;
        g.Phone = phone;
        g.Email = req.GuestEmail;
        g.IdCard = idCard;
        g.Nationality = req.Nationality;
        g.TotalVisits++;
        g.LastVisitDate = DateOnly.FromDateTime(DateTime.Today);
        g.Source = req.Source;
        g.Notes = string.IsNullOrWhiteSpace(req.Notes) ? g.Notes : req.Notes;
        g.UpdatedDate = DateTime.Now;
        
        await _db.SaveChangesAsync();
        return g;
    }

    private async Task ValidateBookingAvailabilityAsync(CreateBookingRequest req)
    {
        var from = DateOnly.FromDateTime(req.CheckIn.Date);
        var to = DateOnly.FromDateTime(req.CheckOut.Date.AddDays(-1));
        var duplicated = req.Rooms
            .GroupBy(r => $"{r.RoomNo}|{r.BedCode ?? ""}")
            .FirstOrDefault(g => g.Count() > 1);
        if (duplicated != null)
            throw new InvalidOperationException("Duplicate room/bed in booking request.");

        foreach (var room in req.Rooms)
        {
            var bedStatus = string.IsNullOrEmpty(room.BedCode)
                ? null
                : await _db.HotelBeds
                    .Where(b => b.HotelCode == req.HotelCode && b.RoomNo == room.RoomNo && b.BedCode == room.BedCode && b.IsActive)
                    .Select(b => b.Status)
                    .FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(room.BedCode) && bedStatus == null)
                throw new InvalidOperationException($"Bed {room.RoomNo}/{room.BedCode} not found.");
            if (bedStatus == "OOS")
                throw new InvalidOperationException($"Bed {room.RoomNo}/{room.BedCode} is out of service.");
            if (bedStatus == "VD" || bedStatus == "OD")
                throw new InvalidOperationException($"Bed {room.RoomNo}/{room.BedCode} is currently dirty ({bedStatus}) and cannot be booked.");

            if (string.IsNullOrEmpty(room.BedCode))
            {
                var roomStatus = await _db.HotelElements
                    .Where(e => e.HotelCode == req.HotelCode && e.Name == room.RoomNo && e.Type == "ROOM" && e.IsActive)
                    .Select(e => e.Status)
                    .FirstOrDefaultAsync();
                    
                if (roomStatus == "VD" || roomStatus == "OD")
                    throw new InvalidOperationException($"Room {room.RoomNo} is currently dirty ({roomStatus}) and cannot be booked.");
            }

            var hasConflict = await _db.HotelRoomForecasts.AnyAsync(f =>
                f.HotelCode == req.HotelCode
                && f.RoomNo == room.RoomNo
                && f.ForecastDate >= from
                && f.ForecastDate <= to
                && (f.BedCode == null || f.BedCode == room.BedCode || room.BedCode == null));

            if (hasConflict)
            {
                var label = string.IsNullOrEmpty(room.BedCode) ? room.RoomNo : $"{room.RoomNo}/{room.BedCode}";
                throw new InvalidOperationException($"{label} is not available in selected dates.");
            }
        }
    }

    private async Task BlockForecastForBookingAsync(int bookingId, CreateBookingRequest req)
    {
        var from = DateOnly.FromDateTime(req.CheckIn.Date);
        var to = DateOnly.FromDateTime(req.CheckOut.Date.AddDays(-1));

        foreach (var room in req.Rooms)
        {
            for (var d = from; d <= to; d = d.AddDays(1))
            {
                if (!await _db.HotelRoomForecasts.AnyAsync(f =>
                    f.HotelCode == req.HotelCode && f.RoomNo == room.RoomNo
                    && f.BedCode == room.BedCode && f.ForecastDate == d))
                {
                    _db.HotelRoomForecasts.Add(new HotelRoomForecast
                    {
                        HotelCode = req.HotelCode,
                        RoomNo = room.RoomNo,
                        BedCode = room.BedCode,
                        ForecastDate = d,
                        BookingId = bookingId,
                        BlockType = "BOOKING"
                    });
                }
            }
        }
        await _db.SaveChangesAsync();
    }

        public async Task<BookingDto> AddBookingServiceAsync(int bookingId, AddBookingServiceRequest req)
    {
        var b = await _db.HotelBookings.Include(x => x.Services).FirstOrDefaultAsync(x => x.Id == bookingId) 
            ?? throw new InvalidOperationException("Booking not found.");
        
        var svc = new AciPlatform.Domain.Entities.Hotel.HotelBookingService
        {
            HotelCode = b.HotelCode,
            BookingId = bookingId,
            ServiceCode = req.ServiceCode,
            ServiceName = req.ServiceName,
            Category = req.Category,
            Quantity = req.Quantity,
            UnitPrice = req.UnitPrice,
            TotalPrice = req.Quantity * req.UnitPrice
        };
        
        b.Services.Add(svc);
        b.ServicePrice += svc.TotalPrice;
        b.TotalAmount += svc.TotalPrice;
        b.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        
        return ToDto(b);
    }

    public async Task<(List<BookingDto> Items, int Total)> GetBookingsAsync(BookingFilterRequest filter)
    {
        var q = _db.HotelBookings.Where(b => b.HotelCode == filter.HotelCode).AsQueryable();
        if (!string.IsNullOrEmpty(filter.Status)) q = q.Where(b => b.Status == filter.Status);
        if (!string.IsNullOrEmpty(filter.BookingType)) q = q.Where(b => b.BookingType == filter.BookingType);
        if (filter.FromDate.HasValue) q = q.Where(b => b.CheckIn >= filter.FromDate.Value);
        if (filter.ToDate.HasValue) q = q.Where(b => b.CheckIn <= filter.ToDate.Value);
        if (!string.IsNullOrEmpty(filter.GuestName)) q = q.Where(b => b.GuestName.Contains(filter.GuestName));
        if (!string.IsNullOrEmpty(filter.GuestPhone)) q = q.Where(b => b.GuestPhone == filter.GuestPhone);

        var total = await q.CountAsync();
        var items = await q.OrderByDescending(b => b.CreatedDate)
            .Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize)
            .Include(b => b.Rooms).Include(b => b.Services)
            .ToListAsync();

        return (items.Select(ToDto).ToList(), total);
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int id)
    {
        var b = await _db.HotelBookings.Include(x => x.Rooms).Include(x => x.Services)
            .FirstOrDefaultAsync(x => x.Id == id);
        return b == null ? null : ToDto(b);
    }

    public async Task<BookingDto?> GetBookingByCodeAsync(string hotelCode, string code)
    {
        var b = await _db.HotelBookings.Include(x => x.Rooms).Include(x => x.Services)
            .FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.BookingCode == code);
        return b == null ? null : ToDto(b);
    }

    public async Task UpdateStatusAsync(UpdateBookingStatusRequest req)
    {
        var b = await _db.HotelBookings.FindAsync(req.BookingId)
            ?? throw new InvalidOperationException("Booking not found.");
        b.Status = req.Status;
        if (req.Status == "CHECKED_IN") b.CheckInActual = DateTime.Now;
        if (req.Status == "CHECKED_OUT") {
            b.CheckOutActual = DateTime.Now;
            // Update guest spend
            var g = await _db.HotelGuests.FirstOrDefaultAsync(x => x.HotelCode == b.HotelCode && x.Phone == b.GuestPhone);
            if (g != null) {
                g.TotalSpend += b.TotalAmount;
                await _db.SaveChangesAsync();
            }
        }
        if (req.Status == "CANCELLED") { b.CancelledAt = DateTime.Now; b.CancelReason = req.CancelReason; }
        if (req.PaidAmount.HasValue) b.PaidAmount = req.PaidAmount.Value;
        b.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();

        // If CHECKED_OUT or CANCELLED, release forecast
        if (req.Status is "CHECKED_OUT" or "CANCELLED")
        {
            var forecasts = await _db.HotelRoomForecasts.Where(f => f.BookingId == req.BookingId).ToListAsync();
            _db.HotelRoomForecasts.RemoveRange(forecasts);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<BookingDto> UpdateBookingAsync(int id, CreateBookingRequest req)
    {
        var b = await _db.HotelBookings.Include(x => x.Rooms).Include(x => x.Services)
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Booking not found.");

        var roomPrice = req.Rooms.Sum(r => r.PricePerNight * r.NightCount);
        var servicePrice = req.Services.Sum(s => s.UnitPrice * s.Quantity);
        b.GuestName = req.GuestName; b.GuestPhone = req.GuestPhone;
        b.CheckIn = req.CheckIn; b.CheckOut = req.CheckOut;
        b.NightCount = (int)(req.CheckOut.Date - req.CheckIn.Date).TotalDays;
        b.RoomPrice = roomPrice; b.ServicePrice = servicePrice;
        b.DiscountAmount = req.DiscountAmount;
        b.TotalAmount = roomPrice + servicePrice - req.DiscountAmount;
        b.Notes = req.Notes; b.GroupName = req.GroupName; b.GroupSize = req.GroupSize;
        b.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return ToDto(b);
    }

    public async Task DeleteBookingAsync(int id)
    {
        var b = await _db.HotelBookings.FindAsync(id);
        if (b != null) { b.IsDeleted = true; b.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    public async Task<decimal> CalculateRoomPriceAsync(string hotelCode, string roomType, DateTime checkIn, DateTime checkOut)
    {
        var nights = (int)(checkOut.Date - checkIn.Date).TotalDays;
        var from = DateOnly.FromDateTime(checkIn.Date);
        var to = DateOnly.FromDateTime(checkOut.Date.AddDays(-1));
        var pricing = await _db.HotelSeasonalPricings
            .Where(p => p.HotelCode == hotelCode && p.IsActive && p.StartDate <= to && p.EndDate >= from)
            .FirstOrDefaultAsync();
        var multiplier = pricing?.PriceMultiplier ?? 1.0m;
        var basePrice = roomType == "KHEPKIN" ? 250000m : 100000m;
        return basePrice * multiplier * nights;
    }    public async Task<object> GetTodayDashboardAsync(string hotelCode)
    {
        var targetDate = DateTime.Today;
        using var cmd = _db.Database.GetDbConnection().CreateCommand();
        cmd.CommandText = "SP_GetRoomStatusDashboard";
        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        var p1 = cmd.CreateParameter(); p1.ParameterName = "@HotelCode"; p1.Value = hotelCode; cmd.Parameters.Add(p1);
        var p2 = cmd.CreateParameter(); p2.ParameterName = "@TargetDate"; p2.Value = targetDate; cmd.Parameters.Add(p2);

        if (cmd.Connection!.State != System.Data.ConnectionState.Open)
            await cmd.Connection.OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var data = new Dictionary<string, object?>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var val = reader.GetValue(i);
                data[reader.GetName(i)] = (val == DBNull.Value) ? null : val;
            }
            return data;
        }

        return new { };
    }

    // ── Catalog Implementation ──────────────────────────────
    public async Task<List<HotelServiceDto>> GetServicesAsync(string hotelCode, string? category = null)
    {
        var q = _db.HotelServices.Where(s => s.HotelCode == hotelCode && !s.IsDeleted);
        if (!string.IsNullOrEmpty(category)) q = q.Where(s => s.Category == category);
        return await q.Select(s => new HotelServiceDto { 
            Id = s.Id, HotelCode = s.HotelCode, ServiceCode = s.ServiceCode, 
            ServiceName = s.ServiceName, Category = s.Category, UnitPrice = s.UnitPrice, 
            Unit = s.Unit, IsAvailable = s.IsAvailable 
        }).ToListAsync();
    }
    public async Task<HotelServiceDto> UpsertServiceAsync(HotelServiceDto req) {
        var s = await _db.HotelServices.FindAsync(req.Id);
        if (s == null) { s = new AciPlatform.Domain.Entities.Hotel.HotelService { HotelCode = req.HotelCode, ServiceCode = req.ServiceCode }; _db.HotelServices.Add(s); }
        s.ServiceName = req.ServiceName; s.Category = req.Category; s.UnitPrice = req.UnitPrice; s.Unit = req.Unit; s.IsAvailable = req.IsAvailable; s.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return new HotelServiceDto { 
            Id = s.Id, HotelCode = s.HotelCode, ServiceCode = s.ServiceCode, 
            ServiceName = s.ServiceName, Category = s.Category, UnitPrice = s.UnitPrice, 
            Unit = s.Unit, IsAvailable = s.IsAvailable 
        };
    }
    public async Task DeleteServiceAsync(int id) {
        var s = await _db.HotelServices.FindAsync(id);
        if (s != null) { s.IsDeleted = true; await _db.SaveChangesAsync(); }
    }

    public async Task<List<HotelAreaDto>> GetAreasAsync(string hotelCode) {
        return await _db.HotelAreas.Where(a => a.HotelCode == hotelCode && a.IsActive)
            .Select(a => new HotelAreaDto { 
                Id = a.Id, HotelCode = a.HotelCode, ParentId = a.ParentId, 
                AreaName = a.AreaName, AreaType = a.AreaType, Color = a.Color 
            }).ToListAsync();
    }
    public async Task<HotelAreaDto> UpsertAreaAsync(HotelAreaDto req) {
        var a = await _db.HotelAreas.FindAsync(req.Id);
        if (a == null) { a = new HotelArea { HotelCode = req.HotelCode }; _db.HotelAreas.Add(a); }
        a.ParentId = req.ParentId; a.AreaName = req.AreaName; a.AreaType = req.AreaType; a.Color = req.Color; a.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return new HotelAreaDto { 
            Id = a.Id, HotelCode = a.HotelCode, ParentId = a.ParentId, 
            AreaName = a.AreaName, AreaType = a.AreaType, Color = a.Color 
        };
    }
    public async Task DeleteAreaAsync(int id) {
        var a = await _db.HotelAreas.FindAsync(id);
        if (a != null) { a.IsActive = false; await _db.SaveChangesAsync(); }
    }

    public async Task<List<HotelElementDto>> GetElementsAsync(string hotelCode, int? areaId = null) {
        var q = _db.HotelElements.Where(e => e.HotelCode == hotelCode && e.IsActive);
        if (areaId.HasValue) q = q.Where(e => e.AreaId == areaId.Value);
        return await q.Select(e => new HotelElementDto { 
            Id = e.Id, HotelCode = e.HotelCode, AreaId = e.AreaId, 
            Name = e.Name, Type = e.Type, Capacity = e.Capacity, Color = e.Color, Status = e.Status
        }).ToListAsync();
    }
    public async Task<HotelElementDto> UpsertElementAsync(HotelElementDto req) {
        var e = await _db.HotelElements.FindAsync(req.Id);
        if (e == null) { e = new HotelElement { HotelCode = req.HotelCode }; _db.HotelElements.Add(e); }
        e.AreaId = req.AreaId; e.Name = req.Name; e.Type = req.Type; e.Capacity = req.Capacity; e.Color = req.Color; 
        e.Status = req.Status ?? e.Status;
        e.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return new HotelElementDto { 
            Id = e.Id, HotelCode = e.HotelCode, AreaId = e.AreaId, 
            Name = e.Name, Type = e.Type, Capacity = e.Capacity, Color = e.Color, Status = e.Status
        };
    }
    public async Task DeleteElementAsync(int id) {
        var e = await _db.HotelElements.FindAsync(id);
        if (e != null) { e.IsActive = false; await _db.SaveChangesAsync(); }
    }

    public async Task<HotelInvoiceDto> GenerateInvoiceAsync(int bookingId, string paymentMethod)
    {
        var b = await _db.HotelBookings.FindAsync(bookingId)
            ?? throw new InvalidOperationException("Booking not found.");
        var today = DateTime.Today.ToString("yyyyMMdd");
        var count = await _db.HotelInvoices.CountAsync(i => i.HotelCode == b.HotelCode && i.IssuedDate.Date == DateTime.Today);
        var inv = new HotelInvoice
        {
            HotelCode = b.HotelCode,
            InvoiceCode = $"INV-{today}-{(count + 1):D3}",
            BookingId = bookingId,
            GuestName = b.GuestName,
            RoomAmount = b.RoomPrice,
            ServiceAmount = b.ServicePrice,
            VehicleAmount = b.VehiclePrice,
            DiscountAmount = b.DiscountAmount,
            TotalAmount = b.TotalAmount,
            PaidAmount = b.PaidAmount,
            PaymentMethod = paymentMethod,
            Status = b.PaidAmount >= b.TotalAmount ? "PAID" : b.PaidAmount > 0 ? "PARTIAL" : "UNPAID"
        };
        _db.HotelInvoices.Add(inv);
        await _db.SaveChangesAsync();
        return new HotelInvoiceDto { Id = inv.Id, InvoiceCode = inv.InvoiceCode, GuestName = inv.GuestName,
            TotalAmount = inv.TotalAmount, PaidAmount = inv.PaidAmount, PaymentMethod = inv.PaymentMethod,
            Status = inv.Status, IssuedDate = inv.IssuedDate };
    }

    private static BookingDto ToDto(HotelBooking b) => new()
    {
        Id = b.Id, BookingCode = b.BookingCode, BookingType = b.BookingType,
        GuestName = b.GuestName, GuestPhone = b.GuestPhone, Nationality = b.Nationality,
        CheckIn = b.CheckIn, CheckOut = b.CheckOut, NightCount = b.NightCount,
        RoomPrice = b.RoomPrice, ServicePrice = b.ServicePrice, VehiclePrice = b.VehiclePrice,
        DiscountAmount = b.DiscountAmount, TotalAmount = b.TotalAmount,
        PaidAmount = b.PaidAmount, DepositAmount = b.DepositAmount,
        Status = b.Status, Source = b.Source, GroupName = b.GroupName, GroupSize = b.GroupSize,
        Notes = b.Notes, CreatedDate = b.CreatedDate,
        Rooms = b.Rooms.Select(r => new BookingRoomDetailDto
        {
            RoomNo = r.RoomNo, BedCode = r.BedCode, GuestName = r.GuestName,
            PricePerNight = r.PricePerNight, TotalPrice = r.TotalPrice, Status = r.Status
        }).ToList(),
        Services = b.Services.Select(s => new BookingServiceDetailDto
        {
            ServiceCode = s.ServiceCode, ServiceName = s.ServiceName, Category = s.Category,
            Quantity = s.Quantity, Unit = s.Unit, UnitPrice = s.UnitPrice, TotalPrice = s.TotalPrice
        }).ToList()
    };
}
