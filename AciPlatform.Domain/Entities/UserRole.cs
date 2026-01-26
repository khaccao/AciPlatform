using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("UserRoles")]
public class UserRole
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(36)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(36)]
    public string? Title { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }

    public int? Order { get; set; } = 0;

    public int? UserCreated { get; set; }

    public bool? IsNotAllowDelete { get; set; } = false;
}
