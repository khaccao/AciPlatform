using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("Menus")]
public class Menu
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(36)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Name { get; set; }

    [MaxLength(255)]
    public string? NameEN { get; set; }

    [MaxLength(255)]
    public string? NameKO { get; set; }

    [MaxLength(36)]
    public string? CodeParent { get; set; }

    public bool? IsParent { get; set; }

    public int? Order { get; set; } = 0;

    [MaxLength(255)]
    public string? Note { get; set; }
}
