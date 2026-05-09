namespace AciPlatform.Application.Interfaces.HotelManagement;

// ── DTOs ──────────────────────────────────────────────────────

public class HotelVehicleDto
{
    public int Id { get; set; }
    public string VehicleCode { get; set; } = string.Empty;
    public string? BienSo { get; set; }
    public string TenXe { get; set; } = string.Empty;
    public string VehicleType { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public int? YearMade { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal DepositRequired { get; set; }
    public int FuelLevel { get; set; }
    public string Condition { get; set; } = "GOOD";
    public string Status { get; set; } = "AVAILABLE";
    public string? ImageUrl { get; set; }
    public string? Notes { get; set; }
}

public class CreateVehicleRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string VehicleCode { get; set; } = string.Empty;
    public string? BienSo { get; set; }
    public string TenXe { get; set; } = string.Empty;
    public string VehicleType { get; set; } = "MOTORBIKE_MANUAL";
    public string? ServiceCode { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public string? Color { get; set; }
    public int? YearMade { get; set; }
    public decimal PricePerDay { get; set; } = 0;
    public decimal DepositRequired { get; set; } = 0;
    public string? Notes { get; set; }
}

public class CreateRentalRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int? BookingId { get; set; }
    public string VehicleCode { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? GuestIdCard { get; set; }
    public DateTime RentFrom { get; set; }
    public DateTime RentTo { get; set; }
    public decimal DepositAmount { get; set; } = 0;
    public int FuelLevelOut { get; set; } = 100;
    public string ConditionOut { get; set; } = "GOOD";
    public string? Notes { get; set; }
    public int? CreatedBy { get; set; }
}

public class ReturnVehicleRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int RentalId { get; set; }
    public DateTime ActualReturnDate { get; set; }
    public int FuelLevelIn { get; set; }
    public string ConditionIn { get; set; } = "GOOD";
    public decimal DamageFee { get; set; } = 0;
    public decimal DepositReturned { get; set; } = 0;
    public string? DamageNotes { get; set; }
}

public class HotelVehicleRentalDto
{
    public int Id { get; set; }
    public string RentalCode { get; set; } = string.Empty;
    public string VehicleCode { get; set; } = string.Empty;
    public string? BienSo { get; set; }
    public string? TenXe { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public DateTime RentFrom { get; set; }
    public DateTime RentTo { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public decimal TotalDays { get; set; }
    public decimal PricePerDay { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DepositAmount { get; set; }
    public decimal DepositReturned { get; set; }
    public decimal DamageFee { get; set; }
    public string Status { get; set; } = "ACTIVE";
    public bool IsOverdue { get; set; }
}

// ── Interface ─────────────────────────────────────────────────
public interface IHotelVehicleService
{
    // Kho xe
    Task<List<HotelVehicleDto>> GetVehiclesAsync(string hotelCode, string? status = null);
    Task<HotelVehicleDto?> GetVehicleByCodeAsync(string hotelCode, string vehicleCode);
    Task<HotelVehicleDto> CreateVehicleAsync(CreateVehicleRequest request);
    Task<HotelVehicleDto> UpdateVehicleAsync(int id, CreateVehicleRequest request);
    Task DeleteVehicleAsync(int id);
    Task UpdateVehicleStatusAsync(string hotelCode, string vehicleCode, string status, int? fuelLevel, string? condition);

    // Thuê xe
    Task<HotelVehicleRentalDto> CreateRentalAsync(CreateRentalRequest request);
    Task<HotelVehicleRentalDto> ReturnVehicleAsync(ReturnVehicleRequest request);
    Task<List<HotelVehicleRentalDto>> GetActiveRentalsAsync(string hotelCode);
    Task<List<HotelVehicleRentalDto>> GetRentalHistoryAsync(string hotelCode, DateTime? from, DateTime? to);
    Task<HotelVehicleRentalDto?> GetRentalByIdAsync(int id);

    /// <summary>Kiểm tra xe có trống trong khoảng thời gian không</summary>
    Task<List<HotelVehicleDto>> GetAvailableVehiclesAsync(string hotelCode, DateTime from, DateTime to, string? vehicleType);
}

// Guest Service
public class HotelGuestDto
{
    public int Id { get; set; }
    public string? GuestCode { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? IdCard { get; set; }
    public string IdType { get; set; } = "CCCD";
    public string? Nationality { get; set; }
    public string? Address { get; set; }
    public string? PreferRoomType { get; set; }
    public string? PreferVehicle { get; set; }
    public int TotalVisits { get; set; }
    public decimal TotalSpend { get; set; }
    public DateOnly? LastVisitDate { get; set; }
    public string Source { get; set; } = "DIRECT";
    public bool IsVIP { get; set; }
    public string? Notes { get; set; }
}

public class UpsertGuestRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? IdCard { get; set; }
    public string IdType { get; set; } = "CCCD";
    public string? Nationality { get; set; }
    public string? Address { get; set; }
    public string? Notes { get; set; }
    public string Source { get; set; } = "DIRECT";
}

public interface IHotelGuestService
{
    Task<List<HotelGuestDto>> SearchGuestsAsync(string hotelCode, string? keyword, int page = 1);
    Task<HotelGuestDto?> GetGuestByPhoneAsync(string hotelCode, string phone);
    Task<HotelGuestDto?> GetGuestByIdAsync(int id);
    Task<HotelGuestDto> UpsertGuestAsync(UpsertGuestRequest request);
    Task<List<BookingDto>> GetGuestBookingHistoryAsync(int guestId);
    Task DeleteGuestAsync(int id);
}

// Services Catalog
public class HotelServiceDto
{
    public int Id { get; set; }
    public string ServiceCode { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string? ServiceNameEN { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? SubCategory { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public bool IsAvailable { get; set; }
    public int SortOrder { get; set; }
}

public class UpsertServiceRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string ServiceCode { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string? ServiceNameEN { get; set; }
    public string Category { get; set; } = "OTHER";
    public string? SubCategory { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public decimal UnitPrice { get; set; } = 0;
    public bool IsAvailable { get; set; } = true;
    public int SortOrder { get; set; } = 0;
}

public interface IHotelServiceCatalogService
{
    Task<List<HotelServiceDto>> GetServicesAsync(string hotelCode, string? category = null);
    Task<HotelServiceDto?> GetServiceByCodeAsync(string hotelCode, string serviceCode);
    Task<HotelServiceDto> UpsertServiceAsync(UpsertServiceRequest request);
    Task DeleteServiceAsync(int id);
    Task ToggleAvailabilityAsync(string hotelCode, string serviceCode, bool isAvailable);
}
