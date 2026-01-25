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
}
