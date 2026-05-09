using AciPlatform.Application.Interfaces.HotelManagement;
using AciPlatform.Domain.Entities.Hotel;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Services.HotelManagement;

public class HotelTourService : IHotelTourService
{
    private readonly HotelDbContext _db;
    public HotelTourService(HotelDbContext db) => _db = db;

    public async Task<List<HotelTourDto>> GetToursAsync(string hotelCode, string? tourType = null)
    {
        var q = _db.HotelTours.Where(t => t.HotelCode == hotelCode);
        if (tourType != null) q = q.Where(t => t.TourType == tourType);
        return await q.OrderBy(t => t.SortOrder).ThenBy(t => t.TourName).Select(t => ToTourDto(t, 0)).ToListAsync();
    }

    public async Task<HotelTourDto?> GetTourByCodeAsync(string hotelCode, string tourCode)
    {
        var t = await _db.HotelTours.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.TourCode == tourCode);
        return t == null ? null : ToTourDto(t, 0);
    }

    public async Task<HotelTourDto> UpsertTourAsync(UpsertTourRequest req)
    {
        var t = await _db.HotelTours.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.HotelCode == req.HotelCode && x.TourCode == req.TourCode);
        if (t == null) { t = new HotelTour { HotelCode = req.HotelCode, TourCode = req.TourCode }; _db.HotelTours.Add(t); }
        t.TourName = req.TourName; t.TourNameEN = req.TourNameEN; t.TourType = req.TourType;
        t.DurationDays = req.DurationDays; t.DurationNights = req.DurationNights;
        t.MaxPerson = req.MaxPerson; t.MinPerson = req.MinPerson;
        t.PricePerPerson = req.PricePerPerson; t.GroupPrice = req.GroupPrice; t.GroupDiscountFrom = req.GroupDiscountFrom;
        t.Highlights = req.Highlights; t.Itinerary = req.Itinerary; t.Inclusions = req.Inclusions;
        t.Exclusions = req.Exclusions; t.MeetingPoint = req.MeetingPoint; t.Difficulty = req.Difficulty;
        t.ImageUrl = req.ImageUrl; t.IsAvailable = req.IsAvailable; t.SortOrder = req.SortOrder;
        t.IsDeleted = false; t.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync(); return ToTourDto(t, 0);
    }

    public async Task DeleteTourAsync(int id)
    { var t = await _db.HotelTours.FindAsync(id);
      if (t != null) { t.IsDeleted = true; t.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task ToggleTourAvailabilityAsync(string hotelCode, string tourCode, bool available)
    { var t = await _db.HotelTours.FirstOrDefaultAsync(x => x.HotelCode == hotelCode && x.TourCode == tourCode);
      if (t != null) { t.IsAvailable = available; t.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<List<HotelTourGuideDto>> GetGuidesAsync(string hotelCode)
        => await _db.HotelTourGuides.Where(g => g.HotelCode == hotelCode).OrderBy(g => g.Name)
            .Select(g => ToGuideDto(g)).ToListAsync();

    public async Task<HotelTourGuideDto?> GetGuideByIdAsync(int id)
    { var g = await _db.HotelTourGuides.FindAsync(id); return g == null ? null : ToGuideDto(g); }

    public async Task<HotelTourGuideDto> UpsertGuideAsync(UpsertTourGuideRequest req)
    {
        var entity = new HotelTourGuide
        {
            HotelCode = req.HotelCode, Name = req.Name, Phone = req.Phone, Email = req.Email,
            Languages = req.Languages, Speciality = req.Speciality, IsFreelance = req.IsFreelance,
            DailyRate = req.DailyRate, Bio = req.Bio, IsActive = req.IsActive
        };
        _db.HotelTourGuides.Add(entity); await _db.SaveChangesAsync();
        return ToGuideDto(entity);
    }

    public async Task DeleteGuideAsync(int id)
    { var g = await _db.HotelTourGuides.FindAsync(id);
      if (g != null) { g.IsDeleted = true; g.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); } }

    public async Task<List<TourScheduleDto>> GetSchedulesAsync(string hotelCode, string? tourCode, DateOnly? from, DateOnly? to)
    {
        var q = _db.HotelTourSchedules.Where(s => s.HotelCode == hotelCode);
        if (tourCode != null) q = q.Where(s => s.TourCode == tourCode);
        if (from.HasValue) q = q.Where(s => s.TourDate >= from.Value);
        if (to.HasValue) q = q.Where(s => s.TourDate <= to.Value);
        var schedules = await q.OrderBy(s => s.TourDate).ToListAsync();
        var guides = await _db.HotelTourGuides.Where(g => g.HotelCode == hotelCode).ToListAsync();
        return schedules.Select(s => { var guide = guides.FirstOrDefault(g => g.Id == s.GuideId);
            return new TourScheduleDto { Id = s.Id, TourCode = s.TourCode, TourDate = s.TourDate,
                GuideId = s.GuideId, GuideName = guide?.Name, MaxSlots = s.MaxSlots, BookedSlots = s.BookedSlots,
                PriceOverride = s.PriceOverride, Status = s.Status, Notes = s.Notes }; }).ToList();
    }

    public async Task<TourScheduleDto> UpsertScheduleAsync(UpsertScheduleRequest req)
    {
        var s = await _db.HotelTourSchedules.FirstOrDefaultAsync(x =>
            x.HotelCode == req.HotelCode && x.TourCode == req.TourCode && x.TourDate == req.TourDate);
        if (s == null) { s = new HotelTourSchedule { HotelCode = req.HotelCode, TourCode = req.TourCode, TourDate = req.TourDate }; _db.HotelTourSchedules.Add(s); }
        s.GuideId = req.GuideId; s.MaxSlots = req.MaxSlots; s.PriceOverride = req.PriceOverride; s.Notes = req.Notes;
        await _db.SaveChangesAsync();
        return new TourScheduleDto { Id = s.Id, TourCode = s.TourCode, TourDate = s.TourDate,
            GuideId = s.GuideId, MaxSlots = s.MaxSlots, BookedSlots = s.BookedSlots, PriceOverride = s.PriceOverride, Status = s.Status };
    }

    public async Task DeleteScheduleAsync(int id)
    { var s = await _db.HotelTourSchedules.FindAsync(id); if (s != null) { _db.HotelTourSchedules.Remove(s); await _db.SaveChangesAsync(); } }

    public async Task<List<TourScheduleDto>> GetAvailableSchedulesAsync(string hotelCode, DateOnly date)
    {
        var schedules = await _db.HotelTourSchedules
            .Where(s => s.HotelCode == hotelCode && s.TourDate == date && s.Status == "OPEN" && s.BookedSlots < s.MaxSlots)
            .ToListAsync();
        return schedules.Select(s => new TourScheduleDto { Id = s.Id, TourCode = s.TourCode, TourDate = s.TourDate,
            MaxSlots = s.MaxSlots, BookedSlots = s.BookedSlots, PriceOverride = s.PriceOverride, Status = s.Status }).ToList();
    }

    public async Task<List<GroupMemberDto>> GetGroupMembersAsync(int bookingId)
        => await _db.HotelGroupMembers.Where(m => m.BookingId == bookingId).OrderBy(m => m.MemberNo)
            .Select(m => ToMemberDto(m)).ToListAsync();

    public async Task<GroupMemberDto> AddGroupMemberAsync(UpsertGroupMemberRequest req)
    {
        var count = await _db.HotelGroupMembers.CountAsync(m => m.BookingId == req.BookingId);
        var m = new HotelGroupMember { HotelCode = req.HotelCode, BookingId = req.BookingId, MemberNo = count + 1,
            GuestName = req.GuestName, GuestPhone = req.GuestPhone, GuestIdCard = req.GuestIdCard,
            Nationality = req.Nationality, RoomNo = req.RoomNo, BedCode = req.BedCode };
        _db.HotelGroupMembers.Add(m); await _db.SaveChangesAsync(); return ToMemberDto(m);
    }

    public async Task<GroupMemberDto> UpdateGroupMemberAsync(int id, UpsertGroupMemberRequest req)
    {
        var m = await _db.HotelGroupMembers.FindAsync(id) ?? throw new InvalidOperationException("Member not found.");
        m.GuestName = req.GuestName; m.GuestPhone = req.GuestPhone; m.GuestIdCard = req.GuestIdCard;
        m.Nationality = req.Nationality; m.RoomNo = req.RoomNo; m.BedCode = req.BedCode;
        await _db.SaveChangesAsync(); return ToMemberDto(m);
    }

    public async Task DeleteGroupMemberAsync(int id)
    { var m = await _db.HotelGroupMembers.FindAsync(id); if (m != null) { _db.HotelGroupMembers.Remove(m); await _db.SaveChangesAsync(); } }

    private static HotelTourDto ToTourDto(HotelTour t, int slots) => new()
    {
        Id = t.Id, TourCode = t.TourCode, TourName = t.TourName, TourNameEN = t.TourNameEN, TourType = t.TourType,
        DurationDays = t.DurationDays, DurationNights = t.DurationNights, MaxPerson = t.MaxPerson, MinPerson = t.MinPerson,
        PricePerPerson = t.PricePerPerson, GroupPrice = t.GroupPrice, GroupDiscountFrom = t.GroupDiscountFrom,
        Highlights = t.Highlights, Itinerary = t.Itinerary, Inclusions = t.Inclusions, Exclusions = t.Exclusions,
        MeetingPoint = t.MeetingPoint, Difficulty = t.Difficulty, ImageUrl = t.ImageUrl,
        IsAvailable = t.IsAvailable, SortOrder = t.SortOrder, AvailableSlots = slots
    };

    private static HotelTourGuideDto ToGuideDto(HotelTourGuide g) => new()
    {
        Id = g.Id, HotelCode = g.HotelCode, Name = g.Name, Phone = g.Phone, Email = g.Email,
        Languages = g.Languages, Speciality = g.Speciality, IsFreelance = g.IsFreelance,
        DailyRate = g.DailyRate, Bio = g.Bio, IsActive = g.IsActive
    };

    private static GroupMemberDto ToMemberDto(HotelGroupMember m) => new()
    {
        Id = m.Id, BookingId = m.BookingId, MemberNo = m.MemberNo, GuestName = m.GuestName,
        GuestPhone = m.GuestPhone, GuestIdCard = m.GuestIdCard, Nationality = m.Nationality,
        RoomNo = m.RoomNo, BedCode = m.BedCode, PaidAmount = m.PaidAmount, Status = m.Status
    };
}

public class HotelReportService : IHotelReportService
{
    private readonly HotelDbContext _db;
    public HotelReportService(HotelDbContext db) => _db = db;

    public async Task<List<OccupancyReportDto>> GetOccupancyReportAsync(string hotelCode, DateOnly from, DateOnly to)
    {
        var result = new List<OccupancyReportDto>();
        var totalRooms = await _db.PmsRooms.CountAsync(r => r.HotelCode == hotelCode && r.IsActive);
        var totalBeds = await _db.HotelBeds.CountAsync(b => b.HotelCode == hotelCode && b.IsActive);
        var bookings = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status != "CANCELLED"
                && DateOnly.FromDateTime(b.CheckIn) <= to && DateOnly.FromDateTime(b.CheckOut) >= from)
            .Include(b => b.Rooms).ToListAsync();

        for (var d = from; d <= to; d = d.AddDays(1))
        {
            var dayBookings = bookings.Where(b =>
                DateOnly.FromDateTime(b.CheckIn) <= d && DateOnly.FromDateTime(b.CheckOut) > d).ToList();
            var occupiedRooms = dayBookings.SelectMany(b => b.Rooms.Select(r => r.RoomNo)).Distinct().Count();
            var occupiedBeds = dayBookings.SelectMany(b => b.Rooms.Where(r => r.BedCode != null)).Count();
            var revenue = bookings.Where(b => DateOnly.FromDateTime(b.CheckOut) == d && b.Status == "CHECKED_OUT").Sum(b => b.TotalAmount);
            result.Add(new OccupancyReportDto
            {
                Date = d, TotalRooms = totalRooms, OccupiedRooms = occupiedRooms,
                TotalBeds = totalBeds, OccupiedBeds = occupiedBeds,
                OccupancyPercent = totalRooms > 0 ? Math.Round((decimal)occupiedRooms / totalRooms * 100, 1) : 0,
                Revenue = revenue,
                ADR = occupiedRooms > 0 ? Math.Round(revenue / occupiedRooms, 0) : 0
            });
        }
        return result;
    }

    public async Task<List<RevenueReportDto>> GetRevenueByMonthAsync(string hotelCode, int year)
    {
        var bookings = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_OUT" && b.CheckOutActual.HasValue && b.CheckOutActual.Value.Year == year)
            .ToListAsync();
        return Enumerable.Range(1, 12).Select(m => {
            var monthBks = bookings.Where(b => b.CheckOutActual!.Value.Month == m).ToList();
            return new RevenueReportDto {
                Period = $"{year}-{m:D2}", RoomRevenue = monthBks.Sum(b => b.RoomPrice),
                ServiceRevenue = monthBks.Sum(b => b.ServicePrice), VehicleRevenue = monthBks.Sum(b => b.VehiclePrice),
                TotalRevenue = monthBks.Sum(b => b.TotalAmount),
                BookingCount = monthBks.Count, GuestCount = monthBks.Sum(b => b.GroupSize)
            };
        }).ToList();
    }

    public async Task<RevenueReportDto> GetRevenueTodayAsync(string hotelCode)
    {
        var today = DateTime.Today;
        var bks = await _db.HotelBookings
            .Where(b => b.HotelCode == hotelCode && b.Status == "CHECKED_OUT"
                && b.CheckOutActual.HasValue && b.CheckOutActual.Value.Date == today)
            .ToListAsync();
        return new RevenueReportDto
        {
            Period = today.ToString("yyyy-MM-dd"), RoomRevenue = bks.Sum(b => b.RoomPrice),
            ServiceRevenue = bks.Sum(b => b.ServicePrice), VehicleRevenue = bks.Sum(b => b.VehiclePrice),
            TotalRevenue = bks.Sum(b => b.TotalAmount), BookingCount = bks.Count, GuestCount = bks.Sum(b => b.GroupSize)
        };
    }

    public async Task<object> GetServicePopularityAsync(string hotelCode, DateOnly from, DateOnly to)
    {
        var services = await _db.HotelBookingServices
            .Where(s => s.HotelCode == hotelCode)
            .GroupBy(s => new { s.ServiceCode, s.ServiceName, s.Category })
            .Select(g => new { g.Key.ServiceCode, g.Key.ServiceName, g.Key.Category, Count = g.Count(), Revenue = g.Sum(x => x.TotalPrice) })
            .OrderByDescending(x => x.Revenue)
            .Take(20).ToListAsync();
        return services;
    }

    public async Task<object> GetVehicleUtilizationAsync(string hotelCode, DateOnly from, DateOnly to)
    {
        var fromDt = from.ToDateTime(TimeOnly.MinValue);
        var toDt = to.ToDateTime(TimeOnly.MaxValue);
        var rentals = await _db.HotelVehicleRentals
            .Where(r => r.HotelCode == hotelCode && r.RentFrom >= fromDt && r.RentFrom <= toDt)
            .GroupBy(r => r.VehicleCode)
            .Select(g => new { VehicleCode = g.Key, TotalRentals = g.Count(), TotalDays = g.Sum(x => x.TotalDays), Revenue = g.Sum(x => x.TotalAmount) })
            .OrderByDescending(x => x.Revenue).ToListAsync();
        return rentals;
    }
}
