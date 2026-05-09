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
        // Generate booking code
        var today = DateTime.Today.ToString("yyyyMMdd");
        var count = await _db.HotelBookings.CountAsync(b => b.HotelCode == req.HotelCode && b.CreatedDate.Date == DateTime.Today);
        var code = $"BK-{today}-{(count + 1):D3}";

        // Calculate prices from rooms
        var roomPrice = req.Rooms.Sum(r => r.PricePerNight * r.NightCount);
        var servicePrice = req.Services.Sum(s => s.UnitPrice * s.Quantity);
        var total = roomPrice + servicePrice - req.DiscountAmount;
        var nights = (int)(req.CheckOut.Date - req.CheckIn.Date).TotalDays;

        var booking = new HotelBooking
        {
            HotelCode = req.HotelCode,
            BookingCode = code,
            BookingType = req.BookingType,
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
                TotalPrice = r.PricePerNight * r.NightCount,
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

        // Block room forecast
        await BlockForecastForBookingAsync(booking.Id, req);

        return await GetBookingByIdAsync(booking.Id) ?? throw new Exception("Created booking not found");
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
        if (req.Status == "CHECKED_OUT") b.CheckOutActual = DateTime.Now;
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
    }

    public async Task<object> GetTodayDashboardAsync(string hotelCode)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        var checkIns = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.CheckIn.Date == today && b.Status == "CONFIRMED").CountAsync();
        var checkOuts = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.CheckOut.Date == today && b.Status == "CHECKED_IN").CountAsync();
        var inHouse = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_IN").CountAsync();
        var todayRevenue = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_OUT" && b.CheckOutActual.HasValue && b.CheckOutActual.Value.Date == today)
            .SumAsync(b => b.TotalAmount);
        return new { CheckInsToday = checkIns, CheckOutsToday = checkOuts, InHouse = inHouse, TodayRevenue = todayRevenue };
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
