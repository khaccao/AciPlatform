namespace AciPlatform.Application.Interfaces.HotelManagement;

// ── DTOs ──────────────────────────────────────────────────────

public class RoomStatusDto
{
    public string RoomNo { get; set; } = string.Empty;
    public string? RoomType { get; set; }    // KHEPKIN / TAPTHE
    public string? Floor { get; set; }
    public string Status { get; set; } = "VACANT";
    public int? CleanDirty { get; set; }
    public int? Inspected { get; set; }
    public List<BedStatusDto> Beds { get; set; } = new();
    // Current booking info (if OCCUPIED)
    public string? CurrentGuest { get; set; }
    public DateTime? CheckOut { get; set; }
    public bool IsCheckoutToday { get; set; }
}

public class BedStatusDto
{
    public string BedCode { get; set; } = string.Empty;
    public string? BedName { get; set; }
    public string BedType { get; set; } = string.Empty;
    public string Status { get; set; } = "VACANT";
    public string? GuestName { get; set; }
    public bool IsAvailable { get; set; } = true;
}

public class RoomAvailabilityRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string? RoomType { get; set; }    // Filter by type
}

public class RoomAvailabilityResult
{
    public string RoomNo { get; set; } = string.Empty;
    public string? RoomType { get; set; }
    public string? Floor { get; set; }
    public bool IsAvailable { get; set; }
    public List<BedAvailability> Beds { get; set; } = new();
    public decimal PricePerNight { get; set; }
}

public class BedAvailability
{
    public string BedCode { get; set; } = string.Empty;
    public string? BedName { get; set; }
    public string? BedType { get; set; }
    public string Status { get; set; } = "VC";
    public bool IsAvailable { get; set; }
}

public class BlockRoomRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string RoomNo { get; set; } = string.Empty;
    public string? BedCode { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string BlockType { get; set; } = "HOLD"; // HOLD/MAINTENANCE
    public string? Note { get; set; }
    public int? UserId { get; set; }
}

public class UpdateRoomStatusRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string RoomNo { get; set; } = string.Empty;
    public int? CleanDirty { get; set; }
    public int? Inspected { get; set; }
    public string? Status { get; set; }
}

public class UpdateBedStatusRequest
{
    public string HotelCode { get; set; } = string.Empty;
    public string RoomNo { get; set; } = string.Empty;
    public string BedCode { get; set; } = string.Empty;
    public string Status { get; set; } = "VC";
}

public class RoomRackDto
{
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public List<RoomRackDateDto> Dates { get; set; } = new();
    public List<RoomRackRoomDto> Rooms { get; set; } = new();
}

public class RoomRackDateDto
{
    public DateOnly Date { get; set; }
    public string Label { get; set; } = string.Empty;
    public bool IsToday { get; set; }
    public bool IsWeekend { get; set; }
}

public class RoomRackRoomDto
{
    public int Id { get; set; }
    public string RoomNo { get; set; } = string.Empty;
    public string? RoomType { get; set; }
    public string? RoomTypeName { get; set; }
    public string? Floor { get; set; }
    public string Status { get; set; } = "VACANT";
    public List<RoomRackCellDto> Cells { get; set; } = new();
}

public class RoomRackCellDto
{
    public DateOnly Date { get; set; }
    public string Status { get; set; } = "VACANT";
    public int? BookingId { get; set; }
    public string? BookingCode { get; set; }
    public string? GuestName { get; set; }
    public string? GuestPhone { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public decimal? TotalAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public string? Source { get; set; }
    public string? BlockType { get; set; }
    public string? Note { get; set; }
    public bool IsStart { get; set; }
    public bool IsEnd { get; set; }
    public int SpanDays { get; set; } = 1;
}

public class MoveRoomRackBookingRequest
{
    public int BookingId { get; set; }
    public string FromRoomNo { get; set; } = string.Empty;
    public string ToRoomNo { get; set; } = string.Empty;
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
}

// ── Interface ─────────────────────────────────────────────────
public interface IHotelRoomService
{
    /// <summary>Sơ đồ phòng theo tầng — trạng thái hiện tại</summary>
    Task<List<RoomStatusDto>> GetRoomStatusMapAsync(string hotelCode);

    /// <summary>Kiểm tra phòng/giường trống trong khoảng thời gian</summary>
    Task<List<RoomAvailabilityResult>> CheckAvailabilityAsync(RoomAvailabilityRequest request);

    /// <summary>Room Forecast calendar — blocking view (like room rack)</summary>
    Task<List<object>> GetRoomForecastAsync(string hotelCode, DateTime fromDate, DateTime toDate);
    Task<RoomRackDto> GetRoomRackAsync(string hotelCode, DateTime fromDate, int days);
    Task MoveRoomRackBookingAsync(string hotelCode, MoveRoomRackBookingRequest request);

    /// <summary>Block phòng/giường (HOLD/MAINTENANCE) không qua booking</summary>
    Task BlockRoomAsync(BlockRoomRequest request);

    /// <summary>Unblock phòng/giường</summary>
    Task UnblockRoomAsync(string hotelCode, string roomNo, string? bedCode, DateTime fromDate, DateTime toDate);

    /// <summary>Cập nhật trạng thái Housekeeping (Clean/Dirty/Inspected)</summary>
    Task UpdateRoomStatusAsync(UpdateRoomStatusRequest request);

    /// <summary>Danh sách giường trong phòng tập thể</summary>
    Task<List<BedStatusDto>> GetBedsByRoomAsync(string hotelCode, string roomNo);

    /// <summary>CRUD Beds</summary>
    Task<BedStatusDto> UpsertBedAsync(string hotelCode, string roomNo, string bedCode, string bedName, string bedType, string? status = null);
    Task UpdateBedStatusAsync(UpdateBedStatusRequest request);
    Task DeleteBedAsync(string hotelCode, string roomNo, string bedCode);
}
