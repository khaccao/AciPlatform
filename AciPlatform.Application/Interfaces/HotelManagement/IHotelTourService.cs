namespace AciPlatform.Application.Interfaces.HotelManagement;

// ── Tour DTOs ────────────────────────────────────────────────
public class HotelTourDto
{
    public int Id { get; set; }
    public string TourCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public string? TourNameEN { get; set; }
    public string TourType { get; set; } = string.Empty;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public int MaxPerson { get; set; }
    public int MinPerson { get; set; }
    public decimal PricePerPerson { get; set; }
    public decimal GroupPrice { get; set; }
    public int GroupDiscountFrom { get; set; }
    public string? Highlights { get; set; }
    public string? Itinerary { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? MeetingPoint { get; set; }
    public string Difficulty { get; set; } = "EASY";
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public int SortOrder { get; set; }
    public int AvailableSlots { get; set; }  // From nearest schedule
}

public class UpsertTourRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string TourCode { get; set; } = string.Empty;
    public string TourName { get; set; } = string.Empty;
    public string? TourNameEN { get; set; }
    public string TourType { get; set; } = "DAY_TRIP";
    public int DurationDays { get; set; } = 1;
    public int DurationNights { get; set; } = 0;
    public int MaxPerson { get; set; } = 10;
    public int MinPerson { get; set; } = 1;
    public decimal PricePerPerson { get; set; } = 0;
    public decimal GroupPrice { get; set; } = 0;
    public int GroupDiscountFrom { get; set; } = 5;
    public string? Highlights { get; set; }
    public string? Itinerary { get; set; }
    public string? Inclusions { get; set; }
    public string? Exclusions { get; set; }
    public string? MeetingPoint { get; set; }
    public string Difficulty { get; set; } = "EASY";
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int SortOrder { get; set; } = 0;
}

public class HotelTourGuideDto
{
    public int Id { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public string GuideCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Languages { get; set; }
    public string? Speciality { get; set; }
    public bool IsFreelance { get; set; }
    public decimal DailyRate { get; set; }
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    // HR integration fields
    public int? HrEmployeeId { get; set; }
    public string? IdCard { get; set; }
    public string? Address { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string ContractType { get; set; } = "FREELANCE"; // FREELANCE | FULLTIME | PARTTIME
    public string? ContractStatus { get; set; }
    public DateOnly? ContractFrom { get; set; }
    public DateOnly? ContractTo { get; set; }
    public decimal MonthlyBaseSalary { get; set; }
    public int TotalTours { get; set; }
    public decimal TotalEarned { get; set; }
    public DateOnly? LastTourDate { get; set; }
    public string? Rating { get; set; }
}

public class UpsertTourGuideRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string? GuideCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Languages { get; set; }
    public string? Speciality { get; set; }
    public bool IsFreelance { get; set; } = true;
    public decimal DailyRate { get; set; } = 0;
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;
    public int? HrEmployeeId { get; set; }
    public string? IdCard { get; set; }
    public string? Address { get; set; }
    public DateOnly? BirthDate { get; set; }
    public string ContractType { get; set; } = "FREELANCE";
    public DateOnly? ContractFrom { get; set; }
    public DateOnly? ContractTo { get; set; }
    public decimal MonthlyBaseSalary { get; set; }
}

// ── Guide Contract ────────────────────────────────────────────
public class GuideContractDto
{
    public int Id { get; set; }
    public int GuideId { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public string ContractCode { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal DailyRate { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateGuideContractRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int GuideId { get; set; }
    public string ContractType { get; set; } = "FREELANCE";
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal DailyRate { get; set; }
    public string? Notes { get; set; }
}

// ── Guide Salary/Payment ──────────────────────────────────────
public class GuideSalaryDto
{
    public int Id { get; set; }
    public int GuideId { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public int Month { get; set; }
    public int Year { get; set; }
    public int TourCount { get; set; }
    public decimal DailyRate { get; set; }
    public decimal TourIncome { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deductions { get; set; }
    public decimal TotalPay { get; set; }
    public string Status { get; set; } = "PENDING"; // PENDING | APPROVED | PAID
    public DateTime? PaidAt { get; set; }
    public string? Notes { get; set; }
}

public class CreateGuideSalaryRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int GuideId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deductions { get; set; }
    public string? Notes { get; set; }
}

// ── IHotelGuideService ────────────────────────────────────────
public interface IHotelGuideService
{
    // CRUD Guides
    Task<List<HotelTourGuideDto>> GetGuidesAsync(string hotelCode, bool? isActive = null);
    Task<HotelTourGuideDto?> GetGuideByIdAsync(int id);
    Task<HotelTourGuideDto> UpsertGuideAsync(UpsertTourGuideRequest req);
    Task DeleteGuideAsync(int id);
    Task ToggleGuideStatusAsync(int id, bool isActive);

    // Contracts
    Task<List<GuideContractDto>> GetContractsAsync(string hotelCode, int? guideId = null);
    Task<GuideContractDto> CreateContractAsync(CreateGuideContractRequest req);
    Task UpdateContractStatusAsync(int contractId, string status);

    // Salary/Payroll
    Task<List<GuideSalaryDto>> GetSalariesAsync(string hotelCode, int? month = null, int? year = null);
    Task<GuideSalaryDto> CalculateSalaryAsync(CreateGuideSalaryRequest req);
    Task ApproveSalaryAsync(int salaryId);
    Task MarkSalaryPaidAsync(int salaryId);

    // Stats for dashboard
    Task<object> GetGuideStatsAsync(string hotelCode, int guideId, int year);
}

public class TourScheduleDto
{
    public int Id { get; set; }
    public string TourCode { get; set; } = string.Empty;
    public DateOnly TourDate { get; set; }
    public int? GuideId { get; set; }
    public string? GuideName { get; set; }
    public int MaxSlots { get; set; }
    public int BookedSlots { get; set; }
    public int AvailableSlots => Math.Max(0, MaxSlots - BookedSlots);
    public decimal? PriceOverride { get; set; }
    public string Status { get; set; } = "OPEN";
    public string? Notes { get; set; }
}

public class UpsertScheduleRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string TourCode { get; set; } = string.Empty;
    public DateOnly TourDate { get; set; }
    public int? GuideId { get; set; }
    public int MaxSlots { get; set; } = 10;
    public decimal? PriceOverride { get; set; }
    public string? Notes { get; set; }
}

public class GroupMemberDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int MemberNo { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? GuestIdCard { get; set; }
    public string? Nationality { get; set; }
    public string? RoomNo { get; set; }
    public string? BedCode { get; set; }
    public decimal PaidAmount { get; set; }
    public string Status { get; set; } = "PENDING";
}

public class UpsertGroupMemberRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? GuestIdCard { get; set; }
    public string? Nationality { get; set; }
    public string? RoomNo { get; set; }
    public string? BedCode { get; set; }
}

// ── Report DTOs ─────────────────────────────────────────────
public class OccupancyReportDto
{
    public DateOnly Date { get; set; }
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int TotalBeds { get; set; }
    public int OccupiedBeds { get; set; }
    public decimal OccupancyPercent { get; set; }
    public decimal Revenue { get; set; }
    public decimal ADR { get; set; }  // Average Daily Rate
}

public class RevenueReportDto
{
    public string Period { get; set; } = string.Empty;
    public decimal RoomRevenue { get; set; }
    public decimal ServiceRevenue { get; set; }
    public decimal VehicleRevenue { get; set; }
    public decimal TotalRevenue { get; set; }
    public int BookingCount { get; set; }
    public int GuestCount { get; set; }
}

// ── Interfaces ───────────────────────────────────────────────
public interface IHotelTourService
{
    // Tours
    Task<List<HotelTourDto>> GetToursAsync(string hotelCode, string? tourType = null);
    Task<HotelTourDto?> GetTourByCodeAsync(string hotelCode, string tourCode);
    Task<HotelTourDto> UpsertTourAsync(UpsertTourRequest req);
    Task DeleteTourAsync(int id);
    Task ToggleTourAvailabilityAsync(string hotelCode, string tourCode, bool available);

    // Guides
    Task<List<HotelTourGuideDto>> GetGuidesAsync(string hotelCode);
    Task<HotelTourGuideDto?> GetGuideByIdAsync(int id);
    Task<HotelTourGuideDto> UpsertGuideAsync(UpsertTourGuideRequest req);
    Task DeleteGuideAsync(int id);

    // Schedules
    Task<List<TourScheduleDto>> GetSchedulesAsync(string hotelCode, string? tourCode, DateOnly? from, DateOnly? to);
    Task<TourScheduleDto> UpsertScheduleAsync(UpsertScheduleRequest req);
    Task DeleteScheduleAsync(int id);
    Task<List<TourScheduleDto>> GetAvailableSchedulesAsync(string hotelCode, DateOnly date);

    // Group Members
    Task<List<GroupMemberDto>> GetGroupMembersAsync(int bookingId);
    Task<GroupMemberDto> AddGroupMemberAsync(UpsertGroupMemberRequest req);
    Task<GroupMemberDto> UpdateGroupMemberAsync(int id, UpsertGroupMemberRequest req);
    Task DeleteGroupMemberAsync(int id);
}

public interface IHotelReportService
{
    Task<List<OccupancyReportDto>> GetOccupancyReportAsync(string hotelCode, DateOnly from, DateOnly to);
    Task<List<RevenueReportDto>> GetRevenueByMonthAsync(string hotelCode, int year);
    Task<RevenueReportDto> GetRevenueTodayAsync(string hotelCode);
    Task<object> GetServicePopularityAsync(string hotelCode, DateOnly from, DateOnly to);
    Task<object> GetVehicleUtilizationAsync(string hotelCode, DateOnly from, DateOnly to);
}
