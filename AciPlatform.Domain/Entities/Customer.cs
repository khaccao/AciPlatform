using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("Customers")]
public class Customer
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(255)]
    public string? Name { get; set; }

    [MaxLength(50)]
    public string? TaxCode { get; set; } // Mã số thuế (dùng cho Kế toán xuất hóa đơn)

    public bool IsSupplier { get; set; } = false; // Phân biệt Nhà cung cấp (331) hay Khách hàng (131)


    [MaxLength(255)]
    public string? Avatar { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }

    public int? Gender { get; set; }

    [MaxLength(50)]
    public string? Provider { get; set; }

    [MaxLength(255)]
    public string? ProviderId { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; } = false;

    // ── Hotel / Property Management ──────────────────────────────
    /// <summary>Đánh dấu Company này là một Khách sạn (Hotel = Company)</summary>
    public bool IsHotel { get; set; } = false;

    /// <summary>Loại khách sạn: HOTEL / RESORT / APARTMENT / MOTEL</summary>
    [MaxLength(50)]
    public string? HotelType { get; set; }

    /// <summary>Connection string đến PMS database của khách sạn này (multi-tenant Dapper)</summary>
    [MaxLength(1000)]
    public string? PmsConnectionString { get; set; }

    /// <summary>DMS Smart Lock App ID</summary>
    [MaxLength(50)]
    public string? DmsAppId { get; set; }

    /// <summary>DMS Smart Lock App Secret</summary>
    [MaxLength(100)]
    public string? DmsAppSecret { get; set; }
}

