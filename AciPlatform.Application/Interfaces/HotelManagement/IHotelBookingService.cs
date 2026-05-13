namespace AciPlatform.Application.Interfaces.HotelManagement;

// ── DTOs ──────────────────────────────────────────────────────

public record BookingRoomItem(string RoomNo, string? BedCode, string? GuestName, decimal PricePerNight, int NightCount);
public record BookingServiceItem(string ServiceCode, decimal Quantity, decimal UnitPrice, DateOnly? ServiceDate, string? Notes);

public class CreateBookingRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string BookingType { get; set; } = "FIT"; // FIT / GIT
    // Khách
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestIdCard { get; set; }
    public string? Nationality { get; set; } = "Việt Nam";
    // Thời gian
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    // Tài chính — nhập sẵn giá lúc booking
    public decimal DiscountAmount { get; set; } = 0;
    public decimal DepositAmount { get; set; } = 0;
    // Phòng và dịch vụ
    public List<BookingRoomItem> Rooms { get; set; } = new();
    public List<BookingServiceItem> Services { get; set; } = new();
    // GIT extra
    public string? GroupName { get; set; }
    public int GroupSize { get; set; } = 1;
    // Meta
    public string Source { get; set; } = "DIRECT";
    public string? Notes { get; set; }
    public string? SpecialRequests { get; set; }
    public int? CreatedBy { get; set; }
}

public class BookingDto
{
    public int Id { get; set; }
    public string BookingCode { get; set; } = string.Empty;
    public string BookingType { get; set; } = "FIT";
    public string GuestName { get; set; } = string.Empty;
    public string? GuestPhone { get; set; }
    public string? Nationality { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int NightCount { get; set; }
    public decimal RoomPrice { get; set; }
    public decimal ServicePrice { get; set; }
    public decimal VehiclePrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DepositAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string? GroupName { get; set; }
    public int GroupSize { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<BookingRoomDetailDto> Rooms { get; set; } = new();
    public List<BookingServiceDetailDto> Services { get; set; } = new();
}

public class BookingRoomDetailDto
{
    public string RoomNo { get; set; } = string.Empty;
    public string? BedCode { get; set; }
    public string? GuestName { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class BookingServiceDetailDto
{
    public string ServiceCode { get; set; } = string.Empty;
    public string? ServiceName { get; set; }
    public string? Category { get; set; }
    public decimal Quantity { get; set; }
    public string? Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class BookingFilterRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string? BookingType { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }
    public string? RoomNo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class UpdateBookingStatusRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public string Status { get; set; } = string.Empty; // CONFIRMED/CHECKED_IN/CHECKED_OUT/CANCELLED
    public string? CancelReason { get; set; }
    public decimal? PaidAmount { get; set; }
}

// ── Interface ─────────────────────────────────────────────────
public interface IHotelBookingService
{
    /// <summary>Tạo booking mới (FIT hoặc GIT) — lưu giá ngay</summary>
    Task<BookingDto> CreateBookingAsync(CreateBookingRequest request);

    /// <summary>Lấy danh sách bookings theo filter</summary>
    Task<(List<BookingDto> Items, int Total)> GetBookingsAsync(BookingFilterRequest filter);

    /// <summary>Lấy chi tiết booking</summary>
    Task<BookingDto?> GetBookingByIdAsync(int id);
    Task<BookingDto?> GetBookingByCodeAsync(string hotelCode, string bookingCode);

    /// <summary>Cập nhật trạng thái: Check-in / Check-out / Cancel</summary>
    Task UpdateStatusAsync(UpdateBookingStatusRequest request);

    /// <summary>Cập nhật thông tin booking (giá, notes, dịch vụ)</summary>
    Task<BookingDto> UpdateBookingAsync(int id, CreateBookingRequest request);
    Task<BookingDto> AddBookingServiceAsync(int bookingId, AddBookingServiceRequest req);

    /// <summary>Xóa booking (soft delete)</summary>
    Task DeleteBookingAsync(int id);

    /// <summary>Tính giá phòng (áp dụng seasonal pricing)</summary>
    Task<decimal> CalculateRoomPriceAsync(string hotelCode, string roomType, DateTime checkIn, DateTime checkOut);

    /// <summary>Dashboard today: check-in hôm nay, check-out hôm nay</summary>
    Task<object> GetTodayDashboardAsync(string hotelCode);

    /// <summary>Tạo invoice từ booking</summary>
    Task<HotelInvoiceDto> GenerateInvoiceAsync(int bookingId, string paymentMethod);

    // ── Catalog & Mapping ─────────────────────────────────────
    Task<List<HotelServiceDto>> GetServicesAsync(string hotelCode, string? category = null);
    Task<HotelServiceDto> UpsertServiceAsync(HotelServiceDto req);
    Task DeleteServiceAsync(int id);

    Task<List<HotelAreaDto>> GetAreasAsync(string hotelCode);
    Task<HotelAreaDto> UpsertAreaAsync(HotelAreaDto req);
    Task DeleteAreaAsync(int id);

    Task<List<HotelElementDto>> GetElementsAsync(string hotelCode, int? areaId = null);
    Task<HotelElementDto> UpsertElementAsync(HotelElementDto req);
    Task DeleteElementAsync(int id);
}

public class HotelAreaDto
{
    public int Id { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public string AreaName { get; set; } = string.Empty;
    public string? AreaType { get; set; }
    public string? Color { get; set; }
}

public class HotelElementDto
{
    public int Id { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public int AreaId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "ROOM";
    public string Status { get; set; } = "VC";
    public int? Capacity { get; set; }
    public string? Color { get; set; }
}

public class HotelInvoiceDto
{
    public int Id { get; set; }
    public string InvoiceCode { get; set; } = string.Empty;
    public string? GuestName { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMethod { get; set; } = "CASH";
    public string Status { get; set; } = "UNPAID";
    public DateTime IssuedDate { get; set; }
}

public class AddBookingServiceRequest
{
    public string ServiceCode { get; set; } = string.Empty;
    public string? ServiceName { get; set; }
    public string? Category { get; set; }
    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; } = 0;
}
