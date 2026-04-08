using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Username { get; set; } = string.Empty;

    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;

    [MaxLength(255)]
    public string? FullName { get; set; }

    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    public string? UserRoleIds { get; set; } // Comma-separated list "1,3,5"

    public int? BranchId { get; set; }
    public int? DepartmentId { get; set; }
    public int? PositionDetailId { get; set; }
    public DateTime? BirthDay { get; set; }
    public int? Gender { get; set; }
    
    [MaxLength(255)]
    public string? Address { get; set; }
    
    public int? ProvinceId { get; set; }
    public int? DistrictId { get; set; }
    public int? WardId { get; set; }
    public string? Images { get; set; }
    
    [MaxLength(36)]
    public string? Identify { get; set; }
    public DateTime? IdentifyCreatedDate { get; set; }
    public DateTime? IdentifyExpiredDate { get; set; }
    
    [MaxLength(36)]
    public string? BankAccount { get; set; }
    [MaxLength(36)]
    public string? Bank { get; set; }
    
    [MaxLength(36)]
    public string? PersonalTaxCode { get; set; }
    [MaxLength(36)]
    public string? SocialInsuranceCode { get; set; }
    
    public double? Salary { get; set; }
    public int? YearCurrent { get; set; }
    public bool? Timekeeper { get; set; }
    public bool? RequestPassword { get; set; }
    public string? Avatar { get; set; }
    public string? FaceImage { get; set; }
    public int? TargetId { get; set; }
    public int Status { get; set; } = 1;
    public string? Language { get; set; }
    
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; }
    public DateTime? LastLogin { get; set; }
    
    public int? UserCreated { get; set; }
    public int? UserUpdated { get; set; }
    
    public bool IsDeleted { get; set; } = false;

    // 2FA Fields
    public bool TwoFactorEnabled { get; set; } = false;
    public string? TwoFactorSecret { get; set; }
    public string? TwoFactorRecoveryCodes { get; set; } // Semicolon separated list
}
