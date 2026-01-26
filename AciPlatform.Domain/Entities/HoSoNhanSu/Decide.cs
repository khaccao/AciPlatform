using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.HoSoNhanSu;

public class Decide
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DecisionTypeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiredDate { get; set; }

    [MaxLength(255)]
    public string? FileUrl { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
