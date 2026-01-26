using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.LuongPhucLoi;

public class SalaryType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Code { get; set; }

    public decimal BaseAmount { get; set; }

    [MaxLength(255)]
    public string? Formula { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
