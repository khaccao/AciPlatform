using System.ComponentModel.DataAnnotations;

namespace AciPlatform.Domain.Entities.HoSoNhanSu;

public class Degree
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? School { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    public int? GraduationYear { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedDate { get; set; }
}
