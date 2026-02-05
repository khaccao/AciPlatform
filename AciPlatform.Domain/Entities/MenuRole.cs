using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("MenuRoles")]
public class MenuRole
{
    [Key]
    public int Id { get; set; }

    public int? MenuId { get; set; }
    
    public int? UserRoleId { get; set; }

    public string? MenuCode { get; set; }

    public bool? View { get; set; } = false;
    public bool? Add { get; set; } = false;
    public bool? Edit { get; set; } = false;
    public bool? Delete { get; set; } = false;
    public bool? Approve { get; set; } = false;

    [ForeignKey("MenuId")]
    public virtual Menu? Menu { get; set; }

    [ForeignKey("UserRoleId")]
    public virtual UserRole? UserRole { get; set; }
}
