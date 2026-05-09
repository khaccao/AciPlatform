using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.HoSoNhanSu;

public class UserCompany
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CompanyCode { get; set; } = string.Empty;

    // ── PMS Account Mapping (khi CompanyCode = HotelCode) ───────
    /// <summary>Tài khoản của user trong PMS Front Office</summary>
    [MaxLength(50)]
    public string? UserFO { get; set; }

    /// <summary>Tài khoản trong PMS Back Office</summary>
    [MaxLength(50)]
    public string? UserBO { get; set; }

    /// <summary>Tài khoản trong PMS POS</summary>
    [MaxLength(50)]
    public string? UserPOS { get; set; }
}

