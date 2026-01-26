using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.HoSoNhanSu;

public class Certificate
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Issuer { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
