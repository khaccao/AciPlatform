using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities;

[Table("UserMenus")]
public class UserMenu
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public int MenuId { get; set; }

    [MaxLength(36)]
    public string? MenuCode { get; set; }

    public bool View { get; set; } = false;
    public bool Add { get; set; } = false;
    public bool Edit { get; set; } = false;
    public bool Delete { get; set; } = false;
    public bool Approve { get; set; } = false;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int? CreatedBy { get; set; }

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }

    [ForeignKey("MenuId")]
    public virtual Menu? Menu { get; set; }
}
