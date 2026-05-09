using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;

namespace AciPlatform.Application.Interfaces.Hotel;

public class HotelDto
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? HotelType { get; set; }
    public bool IsActive { get; set; }
    public string? PmsConnectionString { get; set; }
    public string? DmsAppId { get; set; }
    public string? DmsAppSecret { get; set; }
}

public class HotelUserDto
{
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }
    public string? UserFO { get; set; }
    public string? UserBO { get; set; }
    public string? UserPOS { get; set; }
}

public class AssignHotelUserRequest
{
    public int UserId { get; set; }
    public string HotelCode { get; set; } = string.Empty;
    public string? UserFO { get; set; }
    public string? UserBO { get; set; }
    public string? UserPOS { get; set; }
}

public class UpsertHotelRequest
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? HotelType { get; set; }
    public string? PmsConnectionString { get; set; }
    public string? DmsAppId { get; set; }
    public string? DmsAppSecret { get; set; }
}

public interface IHotelService
{
    /// <summary>Lấy danh sách tất cả Hotels (Company IsHotel=true)</summary>
    Task<List<HotelDto>> GetAllHotelsAsync();

    /// <summary>Lấy khách sạn theo Code</summary>
    Task<HotelDto?> GetHotelByCodeAsync(string code);

    /// <summary>Lấy danh sách khách sạn mà user có quyền truy cập</summary>
    Task<List<HotelDto>> GetHotelsByUserAsync(int userId);

    /// <summary>Tạo hoặc cập nhật khách sạn (đồng thời tạo/cập nhật Customer)</summary>
    Task<HotelDto> UpsertHotelAsync(UpsertHotelRequest request);

    /// <summary>Gán user vào Hotel (CompanyCode = HotelCode), kèm tài khoản PMS</summary>
    Task AssignUserToHotelAsync(AssignHotelUserRequest request);

    /// <summary>Xóa user khỏi Hotel</summary>
    Task RemoveUserFromHotelAsync(int userId, string hotelCode);

    /// <summary>Lấy danh sách users đã gán vào một Hotel</summary>
    Task<List<HotelUserDto>> GetUsersOfHotelAsync(string hotelCode);

    /// <summary>Lấy UserFO của user tại hotel (dùng trong Housekeeping)</summary>
    Task<string?> GetUserFOAsync(int userId, string hotelCode);
}
